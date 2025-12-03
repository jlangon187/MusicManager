using MusicManager.Modelos;
using MusicManager.Data;
using System;
using System.IO;
using System.Windows.Forms;
using TagLib;

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
                ruta = row.Cells["colRuta"].Value.ToString();
                txtRuta.Text = ruta;

                tag = TagLib.File.Create(ruta);

                txtTitulo.Text = tag.Tag.Title;
                txtArtista.Text = tag.Tag.FirstPerformer;
                txtAlbum.Text = tag.Tag.Album;
                txtGenero.Text = tag.Tag.FirstGenre;
                txtAnio.Text = tag.Tag.Year.ToString();

                txtDuracion.Text = tag.Properties.Duration.ToString(@"mm\:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando metadatos: " + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // === GUARDAR EN ARCHIVO ===
                tag.Tag.Title = txtTitulo.Text;
                tag.Tag.Performers = new[] { txtArtista.Text };
                tag.Tag.Album = txtAlbum.Text;
                tag.Tag.Genres = new[] { txtGenero.Text };

                if (int.TryParse(txtAnio.Text, out int anio))
                    tag.Tag.Year = (uint)anio;

                tag.Save();

                // === GUARDAR EN BD ===
                int idArtista = gestor.GetOrCreateArtista(txtArtista.Text);
                int idGenero = gestor.GetOrCreateGenero(txtGenero.Text);
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
                row.Cells["colGenero"].Value = txtGenero.Text;
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
    }
}
