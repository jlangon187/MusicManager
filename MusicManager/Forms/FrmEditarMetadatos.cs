using MusicManager.Data;
using System;
using System.IO;
using System.Windows.Forms;
using TagLib;
using MusicManager.Utils;

namespace MusicManager.Forms
{
    public partial class FrmEditarMetadatos : Form
    {
        private readonly GestorMusica gestor;                   // Gestor de música para operaciones relacionadas con la música
        private readonly DataGridViewRow row;                   // Fila del DataGridView que contiene la canción a editar
        private string ruta;                                    // Ruta del archivo de música
        private TagLib.File tag;                                // Objeto TagLib para manipular los metadatos del archivo de música

        public FrmEditarMetadatos(DataGridViewRow row, GestorMusica gestor)
        {
            InitializeComponent();
            this.row = row;
            this.gestor = gestor;
        }

        /// <summary>
        /// Evento Load del formulario para cargar los metadatos de la canción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEditarMetadatos_Load(object sender, EventArgs e)
        {
            try
            {
                cbGenero.DropDownStyle = ComboBoxStyle.DropDownList;            // Establecer estilo del ComboBox de género

                ruta = row.Cells["colRuta"].Value.ToString();
                txtRuta.Text = ruta;

                tag = TagLib.File.Create(ruta);

                txtTitulo.Text = tag.Tag.Title;
                txtArtista.Text = tag.Tag.FirstPerformer;
                txtAlbum.Text = tag.Tag.Album;

                // Cargar todos los géneros en el ComboBox
                cbGenero.Items.Clear();
                var generos = gestor.ObtenerTodosGeneros();
                foreach (var genero in generos)
                {
                    cbGenero.Items.Add(genero);
                }
                if (tag.Tag.Genres.Length > 0)
                {
                    cbGenero.Text = tag.Tag.Genres[0];
                }
                else
                {
                    cbGenero.Text = string.Empty;
                }

                txtAnio.Text = tag.Tag.Year.ToString();
                txtDuracion.Text = tag.Properties.Duration.ToString(@"mm\:ss");
                MostrarPortadaSeleccionada();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando metadatos: " + ex.Message);
            }
        }

        /// <summary>
        /// Muestra la portada del álbum seleccionada
        /// </summary>
        private void MostrarPortadaSeleccionada()
        {
            if (txtAlbum == null)
                return;

            string ruta = row.Cells["colRuta"].Value.ToString();
            ruta = gestor.NormalizarRuta(ruta);

            // Obtener la canción desde la BD
            var cancion = gestor.ObtenerCancionPorRuta(ruta);
            if (cancion == null)
                return;

            // Obtener URL portada
            string portadaUrl = gestor.ObtenerPortadaAlbum(cancion.id_album);

            if (string.IsNullOrWhiteSpace(portadaUrl))
            {
                pbPortada.Image = null;
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var bytes = client.GetByteArrayAsync(portadaUrl).Result;
                    using (var ms = new MemoryStream(bytes))
                    {
                        pbPortada.Image = Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                pbPortada.Image = null;
            }
        }

        /// <summary>
        /// Evento Click del botón Guardar para actualizar los metadatos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (Validacion.ValidarCampoObligatorio(txtTitulo.Text, "Título", out string mensajeError) == false)
                {
                    MessageBox.Show(mensajeError);
                    return;
                }
                tag.Tag.Title = txtTitulo.Text;
                if (Validacion.ValidarCampoObligatorio(txtArtista.Text, "Artista", out mensajeError) == false)
                {
                    MessageBox.Show(mensajeError);
                    return;
                }
                tag.Tag.Performers = new[] { txtArtista.Text };
                if (Validacion.ValidarCampoObligatorio(txtAlbum.Text, "Álbum", out mensajeError) == false)
                {
                    MessageBox.Show("El campo Álbum es obligatorio. Se establecerá como Desconocido'.");
                    txtAlbum.Text = "Desconocido";
                    return;
                }

                tag.Tag.Album = txtAlbum.Text;
                tag.Tag.Genres = new[] { cbGenero.Text };

                if (Validacion.ValidarAnio(txtAnio.Text, out int anio))
                {
                    tag.Tag.Year = (uint)anio;
                }
                else
                {
                    MessageBox.Show("Año inválido. Introduzca un año válido.");
                    tag.Tag.Year = 0;
                }
                tag.Save();

                int idArtista = gestor.GetOrCreateArtista(txtArtista.Text);
                int idGenero = gestor.GetOrCreateGenero(cbGenero.Text);
                int idAlbum = gestor.GetOrCreateAlbum(txtAlbum.Text, idArtista, int.Parse(txtAnio.Text));

                int? idExistente = gestor.GetCancionPorRuta(ruta);

                var c = new Cancion
                {
                    id_cancion = idExistente ?? 0,
                    titulo = txtTitulo.Text,
                    duracion = txtDuracion.Text,
                    id_artista = idArtista,
                    id_album = idAlbum,
                    id_genero = idGenero,
                    anio = anio,
                    ruta_archivo = ruta
                };

                if (idExistente == null)
                    gestor.InsertarCancion(c);
                else
                    gestor.ActualizarCancion(c);

                row.Cells["colTitulo"].Value = txtTitulo.Text;
                row.Cells["colArtista"].Value = txtArtista.Text;
                row.Cells["colAlbum"].Value = txtAlbum.Text;
                row.Cells["colGenero"].Value = cbGenero.Text;
                row.Cells["colAnno"].Value = txtAnio.Text;

                MessageBox.Show("Metadatos actualizados correctamente.");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando metadatos: " + ex.Message);
            }
        }

        /// <summary>
        /// Meneja el evento Click del botón Cancelar para cerrar el formulario sin guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Maneja la validación para que el campo Año solo acepte dígitos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAnio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        /// <summary>
        /// Botón para añadir un nuevo género
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string nuevoGenero = Microsoft.VisualBasic.Interaction.InputBox("Introduzca el nuevo género:", "Añadir Género", "");
            if (!string.IsNullOrWhiteSpace(nuevoGenero))
            {
                if (!cbGenero.Items.Contains(nuevoGenero))
                {
                    cbGenero.Items.Add(nuevoGenero);
                    cbGenero.SelectedItem = nuevoGenero;
                }
                else
                {
                    MessageBox.Show("El género ya existe en la lista.");
                }
            }
        }
    }
}
