using MusicManager.Modelos;
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
        private readonly GestorMusica gestor;
        private readonly DataGridViewRow row;

        private string ruta;
        private TagLib.File tag;

        public FrmEditarMetadatos(DataGridViewRow row, GestorMusica gestor)
        {
            InitializeComponent();
            this.row = row;
            this.gestor = gestor;
        }

        private void FrmEditarMetadatos_Load(object sender, EventArgs e)
        {
            try
            {
                cbGenero.DropDownStyle = ComboBoxStyle.DropDownList;

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
                pbPortada.Image = null; // No hay portada
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // === GUARDAR EN ARCHIVO ===
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

                // === GUARDAR EN BD ===
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

                // === ACTUALIZAR GRID ===
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtAnio_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir tecla de borrar (Backspace)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir solo dígitos
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
