using Microsoft.VisualBasic;
using MusicManager.Modelos;
using MusicManager.Data;
using MusicManager.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TagLib;
using Modelos;

namespace MusicManager
{
    public partial class FrmMain : Form
    {
        private string carpetaMusica = "";
        private WaveOutEvent reproductor;
        private AudioFileReader archivoAudio;

        private GestorMusica gm;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            reproductor = new WaveOutEvent();
        }

        private void btnSeleccionarCarpeta_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    carpetaMusica = dlg.SelectedPath;
                    CargarCancionesDeCarpeta();
                }
            }
        }

        private void CargarCancionesDeCarpeta()
        {
            dgvCanciones.Rows.Clear();

            if (!Directory.Exists(carpetaMusica)) return;

            var archivos = Directory.GetFiles(carpetaMusica, "*.mp3", SearchOption.AllDirectories);

            foreach (var ruta in archivos)
            {
                try
                {
                    var tag = TagLib.File.Create(ruta);

                    string titulo = tag.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta);
                    string artista = tag.Tag.FirstPerformer ?? "Desconocido";
                    string album = tag.Tag.Album ?? "Desconocido";
                    string genero = tag.Tag.FirstGenre ?? "Desconocido";
                    uint año = tag.Tag.Year;

                    dgvCanciones.Rows.Add(titulo, artista, album, genero, año, ruta);
                }
                catch
                {
                    dgvCanciones.Rows.Add(Path.GetFileName(ruta), "??", "??", "??", "??", ruta);
                }
            }

            ActualizarEstadisticas();
        }

        private void dgvCanciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCanciones.CurrentRow != null)
            {
                string ruta = dgvCanciones.CurrentRow.Cells["colRuta"].Value.ToString();
                ReproducirCancion(ruta);
            }
        }

        private void ReproducirCancion(string ruta)
        {
            reproductor.Stop();
            archivoAudio?.Dispose();

            archivoAudio = new AudioFileReader(ruta);
            reproductor.Init(archivoAudio);
            reproductor.Play();

            lblReproduciendo.Text = "Reproduciendo: " + Path.GetFileName(ruta);
        }

        private void btnPausa_Click(object sender, EventArgs e) => reproductor.Pause();
        private void btnPlay_Click(object sender, EventArgs e) => reproductor.Play();

        private async void btnDescargarMetadatos_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una canción.");
                return;
            }

            string titulo = dgvCanciones.CurrentRow.Cells["colTitulo"].Value.ToString();
            string artista = dgvCanciones.CurrentRow.Cells["colArtista"].Value.ToString();

            var lista = await MetadatosAPI.BuscarLista(titulo, artista);
            if (lista.Count == 0)
            {
                MessageBox.Show("No se encontraron coincidencias.");
                return;
            }

            FrmSeleccionarMetadatos frm = new FrmSeleccionarMetadatos(lista);
            if (frm.ShowDialog() != DialogResult.OK)
                return;

            var meta = frm.Seleccionado;

            // Actualizar grid
            dgvCanciones.CurrentRow.Cells["colTitulo"].Value = meta.Titulo;
            dgvCanciones.CurrentRow.Cells["colArtista"].Value = meta.Artista;
            dgvCanciones.CurrentRow.Cells["colAlbum"].Value = meta.Album;
            dgvCanciones.CurrentRow.Cells["colGenero"].Value = meta.Genero;
            dgvCanciones.CurrentRow.Cells["colAnno"].Value = meta.Anio;

            // ✔ Guardar en BD usando GestorMusica
            GuardarFilaEnBD(dgvCanciones.CurrentRow);

            ActualizarEstadisticas();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            FiltrarCanciones(txtBuscar.Text);
        }

        private void FiltrarCanciones(string txt)
        {
            txt = txt.ToLower();

            foreach (DataGridViewRow row in dgvCanciones.Rows)
            {
                bool visible =
                    row.Cells["colTitulo"].Value.ToString().ToLower().Contains(txt) ||
                    row.Cells["colArtista"].Value.ToString().ToLower().Contains(txt) ||
                    row.Cells["colAlbum"].Value.ToString().ToLower().Contains(txt) ||
                    row.Cells["colGenero"].Value.ToString().ToLower().Contains(txt) ||
                    row.Cells["colAnno"].Value.ToString().ToLower().Contains(txt) ||
                    row.Cells["colRuta"].Value.ToString().ToLower().Contains(txt);

                row.Visible = visible;
            }
        }

        private void btnOrganizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(carpetaMusica))
            {
                MessageBox.Show("Primero selecciona una carpeta.");
                return;
            }

            var input = Interaction.InputBox(
                "1. Artista / Álbum\n2. Género / Año\n3. Año / Artista\n\nElige un modo (1-3):",
                "Organizar Música",
                "1"
            );

            if (!int.TryParse(input, out int modo) || modo < 1 || modo > 3)
            {
                MessageBox.Show("Modo inválido.");
                return;
            }

            foreach (DataGridViewRow row in dgvCanciones.Rows)
            {
                Organizador.OrganizarArchivo(
                    row.Cells["colRuta"].Value.ToString(),
                    carpetaMusica,
                    row.Cells["colArtista"].Value.ToString(),
                    row.Cells["colAlbum"].Value.ToString(),
                    row.Cells["colGenero"].Value.ToString(),
                    row.Cells["colAnno"].Value.ToString(),
                    modo
                );
            }

            MessageBox.Show("Música organizada.");
        }

        private void GuardarFilaEnBD(DataGridViewRow row)
        {
            string titulo = row.Cells["colTitulo"].Value?.ToString() ?? "";
            string artista = row.Cells["colArtista"].Value?.ToString() ?? "";
            string album = row.Cells["colAlbum"].Value?.ToString() ?? "";
            string genero = row.Cells["colGenero"].Value?.ToString() ?? "";
            string anno = row.Cells["colAnno"].Value?.ToString() ?? "0";
            string ruta = row.Cells["colRuta"].Value?.ToString() ?? "";

            // Artista
            int idArtista = gm.GetOrCreateArtista(artista);

            // Género
            int idGenero = gm.GetOrCreateGenero(genero);

            // Álbum
            int idAlbum = gm.GetOrCreateAlbum(album, idArtista, int.Parse(anno));

            // Canción
            gm.InsertarCancion(new Cancion
            {
                titulo = titulo,
                ruta = ruta,
                id_album = idAlbum,
                id_genero = idGenero,
                anio = int.Parse(anno)
            });
        }

        private void ActualizarEstadisticas()
        {
            int total = dgvCanciones.Rows.Count;
            lblTotalCanciones.Text = "Total canciones: " + total;

            HashSet<string> artistas = new();
            HashSet<string> albumes = new();
            HashSet<string> generos = new();
            double duracionTotalMin = 0;

            foreach (DataGridViewRow row in dgvCanciones.Rows)
            {
                artistas.Add(row.Cells["colArtista"].Value.ToString());
                albumes.Add(row.Cells["colAlbum"].Value.ToString());
                generos.Add(row.Cells["colGenero"].Value.ToString());

                try
                {
                    var tag = TagLib.File.Create(row.Cells["colRuta"].Value.ToString());
                    duracionTotalMin += tag.Properties.Duration.TotalMinutes;
                }
                catch { }
            }

            lblTotalArtistas.Text = "Artistas distintos: " + artistas.Count;
            lblTotalAlbumes.Text = "Álbumes distintos: " + albumes.Count;
            lblTotalGeneros.Text = "Géneros distintos: " + generos.Count;
            lblDuracionTotal.Text = "Duración total: " + Math.Round(duracionTotalMin) + " min";
        }
    }
}
