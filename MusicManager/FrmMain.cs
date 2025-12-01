using MusicManager.Utils;
using Microsoft.VisualBasic;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TagLib;

namespace MusicManager
{
    public partial class FrmMain : Form
    {
        private string carpetaMusica = "";
        private WaveOutEvent reproductor;
        private AudioFileReader archivoAudio;

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

            if (Directory.Exists(carpetaMusica))
            {
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
                        ActualizarEstadisticas();
                    }
                    catch
                    {
                        dgvCanciones.Rows.Add(Path.GetFileName(ruta), "??", "??", "??", "??", ruta);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que se ejecuta al hacer doble clic en una fila del DataGridView para reproducir la canción seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCanciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCanciones.CurrentRow != null)
            {
                string ruta = dgvCanciones.CurrentRow.Cells["colRuta"].Value.ToString();
                ReproducirCancion(ruta);
            }
        }

        /// <summary>
        /// Metodo para reproducir una canción dada su ruta.
        /// </summary>
        /// <param name="ruta"></param>
        private void ReproducirCancion(string ruta)
        {
            reproductor.Stop();

            archivoAudio?.Dispose();

            archivoAudio = new AudioFileReader(ruta);
            reproductor.Init(archivoAudio);
            reproductor.Play();

            lblReproduciendo.Text = "Reproduciendo: " + Path.GetFileName(ruta);
        }

        private void btnPausa_Click(object sender, EventArgs e)
        {
            reproductor.Pause();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            reproductor.Play();
        }

        /// <summary>
        /// Metodo que se ejecuta al hacer clic en el boton "Descargar Metadatos" para actualizar los metadatos de las canciones usando una API externa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (frm.ShowDialog() == DialogResult.OK)
            {
                var meta = frm.Seleccionado;

                // Actualizar grid
                dgvCanciones.CurrentRow.Cells["colTitulo"].Value = meta.Titulo;
                dgvCanciones.CurrentRow.Cells["colArtista"].Value = meta.Artista;
                dgvCanciones.CurrentRow.Cells["colAlbum"].Value = meta.Album;
                dgvCanciones.CurrentRow.Cells["colGenero"].Value = meta.Genero;
                dgvCanciones.CurrentRow.Cells["colAnno"].Value = meta.Anio;

                // Guardar automáticamente en BD
                GuardarFilaEnBD(dgvCanciones.CurrentRow);
            }
        }

        /// <summary>
        /// Metodo para filtrar las canciones mostradas en el DataGridView según el texto de búsqueda.
        /// </summary>
        /// <param name="texto"></param>
        private void FiltrarCanciones(string texto)
        {
            texto = texto.ToLower();

            foreach (DataGridViewRow row in dgvCanciones.Rows)
            {
                bool visible =
                    row.Cells["colTitulo"].Value.ToString().ToLower().Contains(texto) ||
                    row.Cells["colArtista"].Value.ToString().ToLower().Contains(texto) ||
                    row.Cells["colAlbum"].Value.ToString().ToLower().Contains(texto) ||
                    row.Cells["colGenero"].Value.ToString().ToLower().Contains(texto) ||
                    row.Cells["colAnno"].Value.ToString().ToLower().Contains(texto) ||
                    row.Cells["colRuta"].Value.ToString().ToLower().Contains(texto);

                row.Visible = visible;
            }
        }

        /// <summary>
        /// Metodo que se ejecuta al cambiar el texto en el cuadro de búsqueda para filtrar las canciones mostradas en el DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            FiltrarCanciones(txtBuscar.Text);
        }

        /// <summary>
        /// Metodo que se ejecuta al hacer clic en el boton "Organizar Música" para organizar los archivos de música en carpetas basadas en sus metadatos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrganizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(carpetaMusica))
            {
                MessageBox.Show("Primero selecciona una carpeta.");
                return;
            }

            if (dgvCanciones.Rows.Count == 0)
            {
                MessageBox.Show("No hay canciones cargadas.");
                return;
            }

            // Elegir modo de organización
            var opciones =
                "1. Artista / Álbum\n" +
                "2. Género / Año\n" +
                "3. Año / Artista\n\n" +
                "Elige un modo (1-3):";

            string input = Microsoft.VisualBasic.Interaction.InputBox(opciones, "Organizar Música", "1");

            if (!int.TryParse(input, out int modo) || modo < 1 || modo > 3)
            {
                MessageBox.Show("Modo inválido.");
                return;
            }

            foreach (DataGridViewRow row in dgvCanciones.Rows)
            {
                string ruta = row.Cells["colRuta"].Value.ToString();
                string titulo = row.Cells["colTitulo"].Value?.ToString() ?? "";
                string artista = row.Cells["colArtista"].Value?.ToString() ?? "";
                string album = row.Cells["colAlbum"].Value?.ToString() ?? "";
                string genero = row.Cells["colGenero"].Value?.ToString() ?? "";
                string año = row.Cells["colAnno"].Value?.ToString() ?? "";

                Organizador.OrganizarArchivo(ruta, carpetaMusica, artista, album, genero, año, modo);
            }

            MessageBox.Show("Música organizada correctamente.");
        }

        private void ActualizarEstadisticas()
        {
            int total = dgvCanciones.Rows.Count;
            lblTotalCanciones.Text = "Total canciones: " + total;

            // Artistas distintos
            var artistas = new HashSet<string>();
            var albumes = new HashSet<string>();
            var generos = new HashSet<string>();
            double duracionTotalMin = 0;

            foreach (DataGridViewRow row in dgvCanciones.Rows)
            {
                artistas.Add(row.Cells["colArtista"].Value?.ToString() ?? "");
                albumes.Add(row.Cells["colAlbum"].Value?.ToString() ?? "");
                generos.Add(row.Cells["colGenero"].Value?.ToString() ?? "");

                // Duración
                if (row.Cells["colRuta"].Value != null)
                {
                    try
                    {
                        var tag = TagLib.File.Create(row.Cells["colRuta"].Value.ToString());
                        var duracion = tag.Properties.Duration.TotalMinutes;
                        duracionTotalMin += duracion;
                    }
                    catch { }
                }
            }

            lblTotalArtistas.Text = "Artistas distintos: " + artistas.Count;
            lblTotalAlbumes.Text = "Álbumes distintos: " + albumes.Count;
            lblTotalGeneros.Text = "Géneros distintos: " + generos.Count;
            lblDuracionTotal.Text = "Duración total: " + Math.Round(duracionTotalMin) + " min";
        }

        private int GetOrCreateAlbum(string album, int idArtista, string anno)
        {
            if (string.IsNullOrWhiteSpace(album))
                album = "Desconocido";

            var cmd = Program.appMusic.LaConexion.CreateCommand();
            cmd.CommandText = "SELECT id_album FROM album WHERE titulo=@t AND id_artista=@a LIMIT 1";
            cmd.Parameters.AddWithValue("@t", album);
            cmd.Parameters.AddWithValue("@a", idArtista);

            object res = cmd.ExecuteScalar();
            if (res != null)
                return Convert.ToInt32(res);

            var cmd2 = Program.appMusic.LaConexion.CreateCommand();
            cmd2.CommandText =
                "INSERT INTO album (titulo, anio_lanzamiento, id_artista) " +
                "VALUES (@t, @y, @a)";
            cmd2.Parameters.AddWithValue("@t", album);
            cmd2.Parameters.AddWithValue("@y", anno);
            cmd2.Parameters.AddWithValue("@a", idArtista);
            cmd2.ExecuteNonQuery();

            return (int)cmd2.LastInsertedId;
        }

        private void GuardarCancion(string titulo, string ruta, int idAlbum, int idGenero, string anno)
{
    if (string.IsNullOrWhiteSpace(titulo))
        titulo = System.IO.Path.GetFileNameWithoutExtension(ruta);

    var cmd = Program.appMusic.LaConexion.CreateCommand();
    cmd.CommandText =
        "INSERT INTO cancion (titulo, ruta, id_album, id_genero, anio) " +
        "VALUES (@t, @r, @al, @g, @y)";

    cmd.Parameters.AddWithValue("@t", titulo);
    cmd.Parameters.AddWithValue("@r", ruta);
    cmd.Parameters.AddWithValue("@al", idAlbum);
    cmd.Parameters.AddWithValue("@g", idGenero);
    cmd.Parameters.AddWithValue("@y", anno);

    cmd.ExecuteNonQuery();
}


        /// <summary>
        /// Metodo para guardar una fila del DataGridView en la base de datos.
        /// </summary>
        /// <param name="row"></param>
        private void GuardarFilaEnBD(DataGridViewRow row)
        {
            string titulo = row.Cells["colTitulo"].Value?.ToString() ?? "";
            string artista = row.Cells["colArtista"].Value?.ToString() ?? "";
            string album = row.Cells["colAlbum"].Value?.ToString() ?? "";
            string genero = row.Cells["colGenero"].Value?.ToString() ?? "";
            string anno = row.Cells["colAnno"].Value?.ToString() ?? "0";
            string ruta = row.Cells["colRuta"].Value?.ToString() ?? "";

            // 1. Artista
            int idArtista = BDHelper.GetOrCreate("artista", "nombre", artista);

            // 2. Género
            int idGenero = BDHelper.GetOrCreate("genero", "nombre", genero);

            // 3. Álbum
            int idAlbum = GetOrCreateAlbum(album, idArtista, anno);

            // 4. Canción
            GuardarCancion(titulo, ruta, idAlbum, idGenero, anno);
        }
    }
}
