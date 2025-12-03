using Microsoft.VisualBasic;
using MusicManager.Data;
using MusicManager.Forms;
using MusicManager.Modelos;
using MusicManager.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TagLib;
using TagLib.IFD.Tags;
using File = System.IO.File;
using Timer = System.Windows.Forms.Timer;

namespace MusicManager
{
    public partial class FrmMain : Form
    {
        private string carpetaMusica = "";                  // Ruta de la carpeta seleccionada
        private WaveOutEvent reproductor;                   // Reproductor de audio NAudio
        private WaveStream archivoAudio;                    // Archivo de audio cargado
        private EstadoApp estadoApp;                        // Estado actual de la aplicación
        private GestorMusica gm;                            // Gestor de música para operaciones con BD
        private Timer timerProgreso;                        // Timer para actualizar el progreso de la canción
        private bool usuarioMoviendoTrackbar = false;       // Indica si el usuario está moviendo el trackbar
        private VolumeSampleProvider volumeProvider;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            reproductor = new WaveOutEvent();                                       // Inicializar reproductor NAudio
            gm = new GestorMusica(Program.appMusic.LaConexion);                     // Inicializar gestor de música con la conexión de la aplicación
            estadoApp = Program.appMusic.estadoApp;                                 // Obtener estado actual de la aplicación

            timerProgreso = new Timer();
            timerProgreso.Interval = 200;
            timerProgreso.Tick += TimerProgreso_Tick;

            RefrescarControles();                                                   // Refrescar controles según el estado

            lblReproduciendo.Text = "No se está reproduciendo ninguna canción.";
#if DEBUG
            consolaDeDepuraciónToolStripMenuItem.Visible = true;
#else
            consolaDeDepuraciónToolStripMenuItem.Visible = false;
#endif
        }

        /// <summary>
        /// Manejo del evento FormClosing para cerrar la conexión a la base de datos si está abierta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (Program.appMusic.conectado)
            {
                var resultado = MessageBox.Show(
                    "Hay una conexión abierta. ¿Deseas cerrarla y salir?",
                    "Conexión abierta",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (resultado == DialogResult.Yes)
                {
                    Program.appMusic.DesconectarDB();
                }
                else
                {
                    e.Cancel = true;
                }
            }
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

            if (!Directory.Exists(carpetaMusica))
                return;

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
            try
            {
                // Parar lo anterior
                reproductor?.Stop();
                archivoAudio?.Dispose();
                archivoAudio = null;

                WaveStream reader;
                if (ruta.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                    reader = new Mp3FileReader(ruta);
                else
                    reader = new AudioFileReader(ruta);

                archivoAudio = reader;

                var sampleProvider = archivoAudio.ToSampleProvider();
                volumeProvider = new VolumeSampleProvider(sampleProvider)
                {
                    Volume = trackVolumen.Value / 100f
                };

                reproductor.Init(volumeProvider);
                reproductor.Play();

                lblReproduciendo.Text = "Reproduciendo: " + Path.GetFileName(ruta);

                // CONFIGURAR PROGRESO
                trackProgreso.Minimum = 0;
                trackProgreso.Maximum = (int)archivoAudio.TotalTime.TotalSeconds;
                trackProgreso.Value = 0;

                lblTiempo.Text = $"00:00 / {archivoAudio.TotalTime:mm\\:ss}";

                timerProgreso.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reproducir: " + ex.Message);
            }
        }



        private void DetenerReproductor()
        {
            try
            {
                timerProgreso.Stop();

                reproductor?.Stop();

                archivoAudio?.Dispose();
                archivoAudio = null;

                lblReproduciendo.Text = "Reproduciendo: (ninguno)";
                lblTiempo.Text = "00:00 / 00:00";

                if (trackProgreso != null)
                {
                    trackProgreso.Value = 0;
                    trackProgreso.Maximum = 0;
                }
            }
            catch { }
        }


        private void btnPausa_Click(object sender, EventArgs e) => reproductor.Pause();

        private void btnStop_Click(object sender, EventArgs e)
        {
            DetenerReproductor();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                // Si no hay audio cargado en NAudio, lo cargamos desde la fila actual
                if (archivoAudio == null)
                {
                    if (dgvCanciones.CurrentRow == null)
                    {
                        MessageBox.Show("Selecciona una canción para reproducir.");
                        return;
                    }

                    string ruta = dgvCanciones.CurrentRow.Cells["colRuta"].Value.ToString();
                    ReproducirCancion(ruta);
                    return;
                }

                // Si hay audio cargado, simplemente reproducimos
                reproductor.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reproducir: " + ex.Message);
            }
        }

        private void TimerProgreso_Tick(object sender, EventArgs e)
        {
            if (archivoAudio == null)
                return;

            if (!usuarioMoviendoTrackbar)
            {
                trackProgreso.Value = (int)archivoAudio.CurrentTime.TotalSeconds;
            }

            lblTiempo.Text =
                $"{archivoAudio.CurrentTime:mm\\:ss} / {archivoAudio.TotalTime:mm\\:ss}";

            // Detectar fin de canción
            if (archivoAudio.CurrentTime >= archivoAudio.TotalTime)
            {
                DetenerReproductor();
            }
        }

        private void trackProgreso_MouseDown(object sender, MouseEventArgs e)
        {
            usuarioMoviendoTrackbar = true;
        }

        private void trackProgreso_MouseUp(object sender, MouseEventArgs e)
        {
            if (archivoAudio != null)
                archivoAudio.CurrentTime = TimeSpan.FromSeconds(trackProgreso.Value);

            usuarioMoviendoTrackbar = false;
        }


        private void trackVolumen_ValueChanged(object sender, EventArgs e)
        {
            if (volumeProvider != null)
                volumeProvider.Volume = trackVolumen.Value / 100f;
        }

        private async void btnDescargarMetadatos_Click(object sender, EventArgs e)
        {
            DetenerReproductorSiActivo();

            if (dgvCanciones.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una canción.");
                return;
            }

            string titulo = dgvCanciones.CurrentRow.Cells["colTitulo"].Value.ToString();
            string artista = dgvCanciones.CurrentRow.Cells["colArtista"].Value.ToString();

            var meta = await BuscarMetadatosConFallback(titulo, artista);

            if (meta == null)
                return;

            dgvCanciones.CurrentRow.Cells["colTitulo"].Value = meta.Titulo;
            dgvCanciones.CurrentRow.Cells["colArtista"].Value = meta.Artista;
            dgvCanciones.CurrentRow.Cells["colAlbum"].Value = meta.Album;
            dgvCanciones.CurrentRow.Cells["colGenero"].Value = meta.Genero;
            dgvCanciones.CurrentRow.Cells["colAnno"].Value = meta.Anio;

            string ruta = dgvCanciones.CurrentRow.Cells["colRuta"].Value.ToString();

            gm.ActualizarMetadatosEnArchivo(
                ruta,
                meta.Titulo,
                meta.Artista,
                meta.Album,
                meta.Genero,
                meta.Anio
            );

            GuardarFilaEnBD(dgvCanciones.CurrentRow);

            MessageBox.Show("Metadatos actualizados correctamente.");

            ActualizarEstadisticas();
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
            DetenerReproductorSiActivo();

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
                string rutaAntigua = row.Cells["colRuta"].Value.ToString();

                string nuevaRuta = Organizador.OrganizarArchivo(
                    rutaAntigua,
                    carpetaMusica,
                    row.Cells["colArtista"].Value.ToString(),
                    row.Cells["colAlbum"].Value.ToString(),
                    row.Cells["colGenero"].Value.ToString(),
                    row.Cells["colAnno"].Value.ToString(),
                    modo
                );

                if (string.IsNullOrWhiteSpace(nuevaRuta))
                    continue;

                // ACTUALIZAR GRID
                row.Cells["colRuta"].Value = nuevaRuta;

                // ACTUALIZAR BASE DE DATOS
                gm.ActualizarRutaCancion(rutaAntigua, nuevaRuta);
            }

            MessageBox.Show("Música organizada.");
        }

        private void GuardarFilaEnBD(DataGridViewRow row)
        {
            try
            {
                string titulo = row.Cells["colTitulo"].Value?.ToString() ?? "";
                string artista = row.Cells["colArtista"].Value?.ToString() ?? "";
                string album = row.Cells["colAlbum"].Value?.ToString() ?? "";
                string genero = row.Cells["colGenero"].Value?.ToString() ?? "";
                string anno = row.Cells["colAnno"].Value?.ToString() ?? "0";
                string ruta = row.Cells["colRuta"].Value?.ToString() ?? "";

                ruta = Path.GetFullPath(ruta).Trim().ToLower();

                if (!File.Exists(ruta))
                    return;

                var tag = TagLib.File.Create(ruta);
                string duracion = tag.Properties.Duration.ToString(@"mm\:ss");

                int idArtista = gm.GetOrCreateArtista(artista);
                int idGenero = gm.GetOrCreateGenero(genero);
                int idAlbum = gm.GetOrCreateAlbum(album, idArtista, int.Parse(anno));

                int? idExistente = gm.GetCancionPorRuta(ruta);

                var c = new Cancion
                {
                    titulo = titulo,
                    duracion = duracion,
                    id_artista = idArtista,
                    id_album = idAlbum,
                    id_genero = idGenero,
                    anio = int.Parse(anno),
                    ruta_archivo = ruta
                };

                if (idExistente == null)
                {
                    gm.InsertarCancion(c);
                }
                else
                {
                    c.id_cancion = idExistente.Value;
                    gm.ActualizarCancion(c);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar metadatos en BD: " + ex.Message);
            }
        }

        private void ActualizarEstadisticas()
        {
            try
            {
                int total = dgvCanciones.Rows.Count;
                lblTotalCanciones.Text = "Total canciones: " + total;

                HashSet<string> artistas = new();
                HashSet<string> albumes = new();
                HashSet<string> generos = new();
                double duracionTotalMin = 0;

                foreach (DataGridViewRow row in dgvCanciones.Rows)
                {
                    // Evitar filas nulas o nuevas
                    if (row.IsNewRow) continue;

                    string artista = row.Cells["colArtista"]?.Value?.ToString() ?? "";
                    string album = row.Cells["colAlbum"]?.Value?.ToString() ?? "";
                    string genero = row.Cells["colGenero"]?.Value?.ToString() ?? "";
                    string ruta = row.Cells["colRuta"]?.Value?.ToString() ?? "";

                    if (!string.IsNullOrWhiteSpace(artista))
                        artistas.Add(artista);

                    if (!string.IsNullOrWhiteSpace(album))
                        albumes.Add(album);

                    if (!string.IsNullOrWhiteSpace(genero))
                        generos.Add(genero);

                    // Duración segura
                    try
                    {
                        if (File.Exists(ruta))
                        {
                            var tag = TagLib.File.Create(ruta);
                            duracionTotalMin += tag.Properties.Duration.TotalMinutes;
                        }
                    }
                    catch { }
                }

                lblTotalArtistas.Text = $"Artistas distintos: {artistas.Count}";
                lblTotalAlbumes.Text = $"Álbumes distintos: {albumes.Count}";
                lblTotalGeneros.Text = $"Géneros distintos: {generos.Count}";
                lblDuracionTotal.Text = $"Duración total: {Math.Round(duracionTotalMin)} min";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en estadísticas: " + ex.Message);
            }
        }


        private async Task<MetadatosAPI.MetadataResult> BuscarMetadatosConFallback(string titulo, string artista)
        {
            var lista = await MetadatosAPI.BuscarLista(titulo, artista);

            if (lista.Count > 0)
            {
                using var frm = new FrmSeleccionarMetadatos(lista);
                if (frm.ShowDialog() == DialogResult.OK)
                    return frm.Seleccionado;
                return null;
            }

            string manual = Microsoft.VisualBasic.Interaction.InputBox(
                "No se encontraron coincidencias.\n\nIntroduce una búsqueda manual (ej: Beyoncé - Halo):",
                "Buscar manualmente",
                titulo + " - " + artista
            );

            if (string.IsNullOrWhiteSpace(manual))
                return null;

            var datos = MetadatosAPI.ParsearBusqueda(manual);
            lista = await MetadatosAPI.BuscarLista(datos.titulo, datos.artista);

            if (lista.Count == 0)
            {
                MessageBox.Show("No se encontraron coincidencias ni siquiera con búsqueda manual.");
                return null;
            }

            using (var frm = new FrmSeleccionarMetadatos(lista))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    return frm.Seleccionado;
            }

            return null;
        }

        /// <summary>
        /// Metodo que refresca el estado de los botones de la barra de herramientas
        /// </summary>
        public void RefreshToolBar()
        {
            if (estadoApp == EstadoApp.SinConexion)
            {
                panelLateral.Enabled = false;
            }
            else
            {
                panelLateral.Enabled = true;
            }
        }


        /// <summary>
        /// Metodo que refresca el estado de la barra de estado
        /// </summary>
        public void RefreshStatusBar()
        {
            if (estadoApp == EstadoApp.SinConexion)
            {
                lbConexionDB.Text = "Sin conexión a la base de datos";
            }
            else if (estadoApp == EstadoApp.Conectado)
            {
                lbConexionDB.Text = "Conectado a la base de datos";
            }
            else if (estadoApp == EstadoApp.Error)
            {
                lbConexionDB.Text = "Error en la aplicación";
            }
            else if (estadoApp == EstadoApp.Iniciando)
            {
                lbConexionDB.Text = "Iniciando aplicación...";
            }
        }

        /// <summary>
        /// Metodo que refresca todos los controles de la ventana principal
        /// </summary>
        public void RefrescarControles()
        {
            RefreshToolBar();
            RefreshStatusBar();
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConfig frmConfig = new FrmConfig();
            frmConfig.Show();
        }

        private void consolaDeDepuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            FrmConsola frmConsola = new FrmConsola();
            frmConsola.Show();
#else

            // Opcional: mensaje si alguien lo ejecuta de alguna manera
            MessageBox.Show("Esta consola solo está disponible en modo Depuración.",
                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Program.appMusic.RegistrarLog("FrmMain", "Intento de abrir consola en modo no depuración.");
#endif
        }

        private void btnSincronizar_Click(object sender, EventArgs e)
        {
            DetenerReproductorSiActivo();

            if (!Directory.Exists(carpetaMusica))
            {
                MessageBox.Show("Selecciona primero la carpeta con tu música.");
                return;
            }

            var rutasFisicas = Directory.GetFiles(carpetaMusica, "*.mp3", SearchOption.AllDirectories);
            var rutasBD = gm.GetTodasLasRutas();

            List<string> nuevas = new();
            List<string> perdidas = new();

            // Normalizar todo para comparar correctamente
            var rutasFisicasNorm = rutasFisicas
                .Select(r => gm.NormalizarRuta(r))
                .ToHashSet();

            var rutasBDNorm = rutasBD
                .Select(r => gm.NormalizarRuta(r))
                .ToHashSet();

            // Detectar canciones nuevas (están en carpeta pero NO en BD)
            foreach (var rutaFisica in rutasFisicas)
            {
                if (!rutasBDNorm.Contains(gm.NormalizarRuta(rutaFisica)))
                    nuevas.Add(rutaFisica);
            }

            // Detectar canciones perdidas (están en BD pero NO en carpeta)
            foreach (var rutaBD in rutasBD)
            {
                if (!rutasFisicasNorm.Contains(gm.NormalizarRuta(rutaBD)))
                    perdidas.Add(rutaBD);
            }

            // Mostrar ventana de sincronización
            FrmSincronizar frm = new FrmSincronizar(nuevas, perdidas, gm);
            frm.ShowDialog();
        }

        private void btnEditarMetadatos_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una canción.");
                return;
            }

            using var frm = new FrmEditarMetadatos(dgvCanciones.CurrentRow, gm);
            frm.ShowDialog();
        }

        private void configuraciónAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FrmConfigAPI();
            f.ShowDialog();
        }

        private void DetenerReproductorSiActivo()
        {
            try
            {
                if (reproductor != null)
                {
                    reproductor.Stop();
                }

                if (archivoAudio != null)
                {
                    archivoAudio.Dispose();
                    archivoAudio = null;
                }

                lblReproduciendo.Text = "Reproduciendo: (ninguno)";
            }
            catch { }
        }

        private void salirDelProgramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAcerca f = new frmAcerca();
            f.ShowDialog();
        }

        private void txtBuscarCancion_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscarCancion.Text.ToLower();

            foreach (DataGridViewRow row in dgvCanciones.Rows)
            {
                if (row.IsNewRow) continue;

                bool visible =
                    row.Cells["colTitulo"].Value.ToString().ToLower().Contains(filtro) ||
                    row.Cells["colArtista"].Value.ToString().ToLower().Contains(filtro) ||
                    row.Cells["colAlbum"].Value.ToString().ToLower().Contains(filtro) ||
                    row.Cells["colGenero"].Value.ToString().ToLower().Contains(filtro);

                row.Visible = visible;
            }
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscarCancion.Text = "";
        }
    }
}
