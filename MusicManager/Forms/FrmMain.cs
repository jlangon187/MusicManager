using Microsoft.VisualBasic;
using MusicManager.Data;
using MusicManager.Forms;
using MusicManager.Modelos;
using MusicManager.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TagLib;
using File = System.IO.File;
using Timer = System.Windows.Forms.Timer;
using System.Drawing.Configuration;

namespace MusicManager
{
    public partial class FrmMain : Form
    {
        #region CAMPOS PRIVADOS
        
        private string carpetaMusica = "";                  // Ruta de la carpeta seleccionada
        private WaveOutEvent reproductor;                   // Reproductor de audio NAudio
        private WaveStream archivoAudio;                    // Archivo de audio cargado
        private EstadoApp estadoApp;                        // Estado actual de la aplicación
        private GestorMusica gm;                            // Gestor de música para operaciones con BD
        private Timer timerProgreso;                        // Timer para actualizar el progreso de la canción
        private bool usuarioMoviendoTrackbar = false;       // Indica si el usuario está moviendo el trackbar
        private VolumeSampleProvider volumeProvider;        // Control de volumen NAudio
        
        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor del formulario principal
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region EVENTOS

        /// <summary>
        /// Manejo del evento Load del formulario principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Aplicar estilo global
            ThemeManager.ApplyTheme(this);

            reproductor = new WaveOutEvent();
            gm = new GestorMusica(Program.appMusic.LaConexion);
            estadoApp = Program.appMusic.estadoApp;

            timerProgreso = new Timer();
            timerProgreso.Interval = 200;
            timerProgreso.Tick += TimerProgreso_Tick;

            progressBarSync.Visible = false;
            pbPortada.Visible = false;

            RefrescarControles();

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
                    return;
                }
            }
            try
            {
                reproductor?.Stop();
                reproductor?.Dispose();
                archivoAudio?.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// metodo para abrir el formulario de configuración
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConfig frmConfig = new FrmConfig();
            frmConfig.Show();
        }

        /// <summary>
        /// Metodo para abrir el formulario de configuración de API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configuraciónAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FrmConfigAPI();
            f.ShowDialog();
        }

        /// <summary>
        /// Metodo para reproducir la canción al hacer doble clic en una fila del DataGridView
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
        /// Metodo para abrir la consola de depuración en modo DEBUG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Metodo para abrir el formulario de "Acerca de"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAcerca f = new frmAcerca();
            f.ShowDialog();
        }

        /// <summary>
        /// Metodo para salir del programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void salirDelProgramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region BOTONES E INTERACCIONES UI

        /// <summary>
        /// Metodo para seleccionar la carpeta de música, que cargue las caciones y que la interfaz no se congele
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSeleccionarCarpeta_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                progressBarSync.Visible = true;


                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    carpetaMusica = dlg.SelectedPath;

                    await EjecutarAccionUIAsync(btnSeleccionarCarpeta, async () =>
                    {
                        await SincronizarTodoAsync();
                    });
                }
                else
                {
                    progressBarSync.Visible = false;
                    return;
                }
            }
        }

        /// <summary>
        /// Metodo para descargar metadatos de la canción seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDescargarMetadatos_Click(object sender, EventArgs e)
        {
            await EjecutarAccionUIAsync(btnDescargarMetadatos, async () =>
            {
                DetenerReproductorSiActivo();
                pnLateral.Enabled = false;
                progressBarSync.Visible = true;
                progressBarSync.Style = ProgressBarStyle.Marquee;
                progressBarSync.MarqueeAnimationSpeed = 30;
                try
                {
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

                    GuardarFilaEnBD(dgvCanciones.CurrentRow, meta.PortadaUrl);

                    MessageBox.Show("Metadatos actualizados correctamente.");

                    MostrarPortadaSeleccionada();

                    ActualizarEstadisticas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al descargar metadatos: " + ex.Message);
                }
                finally
                {
                    pnLateral.Enabled = true;
                    progressBarSync.Visible = false;
                    progressBarSync.MarqueeAnimationSpeed = 0;
                }
            });
        }

        /// <summary>
        /// Metodo para organizar la música según los metadatos en carpetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnOrganizar_Click(object sender, EventArgs e)
        {
            DetenerReproductorSiActivo();

            if (string.IsNullOrEmpty(carpetaMusica))
            {
                MessageBox.Show("Primero selecciona una carpeta.");
                return;
            }

            using (var frm = new FrmOrganizarMusica(dgvCanciones, carpetaMusica))
            {
                // Form para elegir el modo y ver ejemplo
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                int modo = frm.ModoSeleccionado;

                int movidos = 0;

                foreach (DataGridViewRow row in dgvCanciones.Rows)
                {
                    if (row.IsNewRow) continue;

                    string rutaAntigua = row.Cells["colRuta"].Value.ToString();
                    string artista = row.Cells["colArtista"].Value.ToString();
                    string album = row.Cells["colAlbum"].Value.ToString();
                    string genero = row.Cells["colGenero"].Value.ToString();
                    string anio = row.Cells["colAnno"].Value.ToString();

                    // Calcular nueva carpeta
                    string carpetaDestino = Organizador.ObtenerCarpetaDestino(
                        carpetaMusica, artista, album, genero, anio, modo);

                    string nombreArchivo = Path.GetFileName(rutaAntigua);
                    string rutaNueva = Path.Combine(carpetaDestino, nombreArchivo);

                    // No mover si es igual
                    if (rutaNueva.Equals(rutaAntigua, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    try
                    {
                        Directory.CreateDirectory(carpetaDestino);
                        File.Move(rutaAntigua, rutaNueva);
                        row.Cells["colRuta"].Value = rutaNueva;

                        gm.ActualizarRutaCancion(rutaAntigua, rutaNueva);
                        movidos++;
                    }
                    catch (Exception ex)
                    {
                        Program.appMusic.RegistrarLog("Organizar", ex.Message);
                    }
                }

                await CargarCancionesDeCarpetaAsync();
                ActualizarEstadisticas();

                MessageBox.Show(
                    $"Organización completada.\nArchivos movidos: {movidos}",
                    "Organización finalizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        /// <summary>
        /// Metodo para editar los metadatos de la canción seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditarMetadatos_Click(object sender, EventArgs e)
        {
            if (dgvCanciones.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una canción.");
                return;
            }

            DetenerReproductorSiActivo();
            using var frm = new FrmEditarMetadatos(dgvCanciones.CurrentRow, gm);
            frm.ShowDialog();
        }

        /// <summary>
        /// Metodo para sincronizar toda la música de la carpeta seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSincronizaTodo_Click(object sender, EventArgs e)
        {
            await EjecutarAccionUIAsync(btnSincronizaTodo, async () =>
            {
                DetenerReproductor();
                await SincronizarTodoAsync();
            });
        }

        /// <summary>
        /// Metodo para limpiar el cuadro de búsqueda de canciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscarCancion.Text = "";
        }

        /// <summary>
        /// Metodo para filtrar las canciones mostradas según el texto de búsqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    row.Cells["colGenero"].Value.ToString().ToLower().Contains(filtro) ||
                    row.Cells["colAnno"].Value.ToString().Contains(filtro);

                row.Visible = visible;
            }
        }

        /// <summary>
        /// Metodo para mostrar la portada del álbum de la canción seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCanciones_SelectionChanged(object sender, EventArgs e)
        {
            MostrarPortadaSeleccionada();
        }

        /// <summary>
        /// Metodo para ampliar la portada al hacer clic en ella
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbPortada_Click(object sender, EventArgs e)
        {
            FrmPortada f = new FrmPortada();
            if (pbPortada.Image != null)
            {
                f.SetImagen(pbPortada.Image);
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hay portada disponible.");
            }
        }

        /// <summary>
        /// Metodo para mostrar la portada del álbum de la canción seleccionada
        /// </summary>
        private void MostrarPortadaSeleccionada()
        {
            if (dgvCanciones.CurrentRow == null)
                return;

            string ruta = dgvCanciones.CurrentRow.Cells["colRuta"].Value.ToString();
            ruta = gm.NormalizarRuta(ruta);

            // Obtener la canción desde la BD
            var cancion = gm.ObtenerCancionPorRuta(ruta);
            if (cancion == null)
                return;

            // Obtener URL portada
            string portadaUrl = gm.ObtenerPortadaAlbum(cancion.id_album);

            if (string.IsNullOrWhiteSpace(portadaUrl))
            {
                pbPortada.Visible = false;
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
                        pbPortada.Visible = true;
                        pbPortada.Image = Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                pbPortada.Image = null;
            }
        }

        #endregion

        #region REPRODUCTOR AUDIO NAudio

        /// <summary>
        /// Evento para pausar la reproducción de la canción actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPausa_Click(object sender, EventArgs e) => reproductor.Pause();

        /// <summary>
        /// Evento clic del botón de detener canción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            DetenerReproductor();
        }

        /// <summary>
        /// Evento clic del botón de reproducir canción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Metodo para reproducir una canción desde una ruta dada
        /// </summary>
        /// <param name="ruta"></param>
        private void ReproducirCancion(string ruta)
        {
            try
            {
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

        /// <summary>
        /// Parado del reproductor y limpieza de recursos
        /// </summary>
        private void DetenerReproductor()
        {
            try
            {
                timerProgreso.Stop();

                reproductor?.Stop();

                archivoAudio?.Dispose();
                archivoAudio = null;

                lblReproduciendo.Text = "Reproduciendo: (ninguna)";
                lblTiempo.Text = "00:00 / 00:00";

                if (trackProgreso != null)
                {
                    trackProgreso.Value = 0;
                    trackProgreso.Maximum = 0;
                }
            }
            catch { }
        }

        /// <summary>
        /// Parado del reproductor si está activo, para usar antes de acciones que requieran detener la música
        /// </summary>
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

                lblReproduciendo.Text = "Reproduciendo: (ninguna)";
            }
            catch { }
        }

        /// <summary>
        /// Timer para actualizar el progreso de la canción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerProgreso_Tick(object sender, EventArgs e)
        {
            if (archivoAudio == null)
                return;

            if (!usuarioMoviendoTrackbar)
            {
                int seg = (int)archivoAudio.CurrentTime.TotalSeconds;
                if (seg <= trackProgreso.Maximum)
                    trackProgreso.Value = seg;
            }

            lblTiempo.Text =
                $"{archivoAudio.CurrentTime:mm\\:ss} / {archivoAudio.TotalTime:mm\\:ss}";

            // Detectar fin de canción
            if (archivoAudio.CurrentTime >= archivoAudio.TotalTime)
            {
                DetenerReproductor();
            }
        }

        /// <summary>
        /// Progreso manual del trackbar por parte del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackProgreso_MouseUp(object sender, MouseEventArgs e)
        {
            if (archivoAudio != null)
                archivoAudio.CurrentTime = TimeSpan.FromSeconds(trackProgreso.Value);

            usuarioMoviendoTrackbar = false;
        }

        /// <summary>
        /// Progreso manual del trackbar por parte del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackProgreso_MouseDown(object sender, MouseEventArgs e)
        {
            usuarioMoviendoTrackbar = true;
        }

        /// <summary>
        /// Volumen cambiado por el usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackVolumen_ValueChanged(object sender, EventArgs e)
        {
            if (volumeProvider != null)
                volumeProvider.Volume = trackVolumen.Value / 100f;
        }

        #endregion

        #region SINCRONIZACION DE BASE DE DATOS Y CARPETAS

        /// <summary>
        /// Metodo asíncrono para ejecutar la sincronización completa
        /// </summary>
        /// <returns>Retorna una tarea asíncrona</returns>
        private async Task SincronizarTodoAsync()
        {
            // Ejecutar sincronización de BD + lectura de archivos en background
            await Task.Run(() => SincronizarTodo());

            // Cargar las canciones al DataGridView de forma asíncrona
            await CargarCancionesDeCarpetaAsync();
        }

        /// <summary>
        /// Metodo asíncrono para cargar las canciones desde la carpeta seleccionada
        /// </summary>
        /// <returns>Retorna una tarea asíncrona</returns>
        private async Task CargarCancionesDeCarpetaAsync()
        {
            dgvCanciones.Enabled = false;
            dgvCanciones.SuspendLayout();

            // Leer canciones en segundo plano ---
            var lista = await Task.Run(() =>
            {
                var resultado = new List<object[]>();

                if (!Directory.Exists(carpetaMusica))
                    return resultado;

                foreach (var ruta in Directory.GetFiles(carpetaMusica, "*.mp3", SearchOption.AllDirectories))
                {
                    try
                    {
                        var tag = TagLib.File.Create(ruta);

                        string titulo = tag.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta);
                        string artista = tag.Tag.FirstPerformer ?? "Desconocido";
                        string album = tag.Tag.Album ?? "Desconocido";
                        string genero = tag.Tag.FirstGenre ?? "Desconocido";
                        uint año = tag.Tag.Year;

                        resultado.Add(new object[] { titulo, artista, album, genero, año, ruta });
                    }
                    catch
                    {
                        resultado.Add(new object[] { Path.GetFileName(ruta), "??", "??", "??", "??", ruta });
                    }
                }

                return resultado;
            });

            // Muestra las canciones en el DataGridView
            dgvCanciones.Rows.Clear();

            foreach (var fila in lista)
                dgvCanciones.Rows.Add(fila);

            dgvCanciones.ResumeLayout();
            dgvCanciones.Enabled = true;

            ActualizarEstadisticas();
        }

        /// <summary>
        /// Metodo para sincronizar toda la música de la carpeta seleccionada
        /// </summary>
        private void SincronizarTodo()
        {
            if (string.IsNullOrEmpty(carpetaMusica) || !Directory.Exists(carpetaMusica))
            {
                MessageBox.Show("Selecciona primero una carpeta.");
                return;
            }

            // Listas para mostrar resumen al usuario
            List<string> nuevas = new();
            List<string> actualizadas = new();
            List<string> eliminadas = new();

            var rutasFisicasNorm = Directory
                .GetFiles(carpetaMusica, "*.mp3", SearchOption.AllDirectories)
                .Select(r => gm.NormalizarRuta(r))
                .ToList();

            var rutasBDNorm = gm.GetTodasLasRutas()
                .Select(r => gm.NormalizarRuta(r))
                .ToList();

            nuevas = rutasFisicasNorm
                .Except(rutasBDNorm)
                .ToList();

            foreach (var ruta in nuevas)
                gm.InsertarCancionDesdeArchivo(ruta);

            // Recorrer las nuevas rutas para actualizar la barra de progreso
            foreach (var rutaBD in rutasBDNorm)
            {
                string ruta = rutaBD; // Normalizada

                // Si el archivo ya no existe, eliminar de BD
                if (!File.Exists(ruta))
                {
                    gm.EliminarCancionPorRuta(ruta);
                    eliminadas.Add(ruta);
                    continue;
                }

                TagLib.File tag = null;
                try { tag = TagLib.File.Create(ruta); }
                catch { continue; }

                string titulo = tag.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta);
                string artista = tag.Tag.FirstPerformer ?? "Desconocido";
                string album = tag.Tag.Album ?? "Desconocido";
                string genero = tag.Tag.FirstGenre ?? "Desconocido";
                int anio = (int)tag.Tag.Year;
                string duracion = tag.Properties.Duration.ToString(@"mm\:ss");

                var cancionBD = gm.ObtenerCancionPorRuta(ruta);
                if (cancionBD == null)
                    continue;

                int idArtista = gm.GetOrCreateArtista(artista);
                int idGenero = gm.GetOrCreateGenero(genero);
                int idAlbum = gm.GetOrCreateAlbum(album, idArtista, anio);

                // Compara y actualiza si hay cambios
                bool cambiado =
                    !string.Equals(cancionBD.titulo, titulo, StringComparison.InvariantCultureIgnoreCase) ||
                    !string.Equals(cancionBD.duracion, duracion) ||
                    cancionBD.id_artista != idArtista ||
                    cancionBD.id_album != idAlbum ||
                    cancionBD.id_genero != idGenero ||
                    cancionBD.anio != anio;

                if (cambiado)
                {
                    gm.ActualizarCancion(new Cancion
                    {
                        id_cancion = cancionBD.id_cancion,
                        titulo = titulo,
                        duracion = duracion,
                        id_artista = idArtista,
                        id_album = idAlbum,
                        id_genero = idGenero,
                        anio = anio,
                        ruta_archivo = ruta
                    });

                    actualizadas.Add(ruta);
                }
            }
            gm.LimpiarHuerfanos();

            MostrarResumenSincronizacion(nuevas, actualizadas, eliminadas);
        }

        /// <summary>
        /// Metodo para mostrar un resumen de la sincronización al usuario
        /// </summary>
        /// <param name="nuevas"></param>
        /// <param name="actualizadas"></param>
        /// <param name="eliminadas"></param>
        private void MostrarResumenSincronizacion(List<string> nuevas, List<string> actualizadas, List<string> eliminadas)
        {
            string resumen =
                "Sincronización completada:\n\n" +
                $"✔ Nuevas añadidas: {nuevas.Count}\n" +
                $"✔ Actualizadas: {actualizadas.Count}\n" +
                $"✔ Eliminadas: {eliminadas.Count}\n";

            MessageBox.Show(this, resumen, "Sincronización completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Metodo para guardar los metadatos de una fila en la base de datos
        /// </summary>
        /// <param name="row"></param>
        /// <param name="portada"></param>
        private void GuardarFilaEnBD(DataGridViewRow row, string portada)
        {
            try
            {
                string titulo = row.Cells["colTitulo"].Value?.ToString() ?? "";
                string artista = row.Cells["colArtista"].Value?.ToString() ?? "";
                string album = row.Cells["colAlbum"].Value?.ToString() ?? "";
                string genero = row.Cells["colGenero"].Value?.ToString() ?? "";
                string anno = row.Cells["colAnno"].Value?.ToString() ?? "0";
                string ruta = row.Cells["colRuta"].Value?.ToString() ?? "";

                ruta = gm.NormalizarRuta(ruta);

                if (!File.Exists(ruta))
                    return;

                string duracion = "";
                try
                {
                    var tag = TagLib.File.Create(ruta);
                    duracion = tag.Properties.Duration.ToString(@"mm\:ss");
                }
                catch
                {
                    duracion = "00:00";
                }

                int idArtista = gm.GetOrCreateArtista(artista);
                int idGenero = gm.GetOrCreateGenero(genero);
                int idAlbum = gm.GetOrCreateAlbum(album, idArtista, int.Parse(anno), portada ?? "");

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
                Program.appMusic.RegistrarLog("FrmMain", "Error al guardar metadatos en BD: " + ex.Message);
                MessageBox.Show("Error al guardar metadatos en BD: " + ex.Message);
            }
        }

        #endregion

        #region MENUS Y BARRAS DE HERRAMIENTAS

        /// <summary>
        /// Metodo que refresca el estado de los botones de la barra de herramientas
        /// </summary>
        public void RefreshToolBar()
        {
            if (estadoApp == EstadoApp.SinConexion)
            {
                pnLateral.Enabled = false;
            }
            else
            {
                pnLateral.Enabled = true;
            }
        }

        /// <summary>
        /// Metodo que refresca el estado de la barra de estado
        /// </summary>
        public void RefreshStatusBar()
        {
            estadoApp = Program.appMusic.estadoApp;

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

        /// <summary>
        /// Metodo para actualizar las estadísticas mostradas en la interfaz
        /// </summary>
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

        #endregion

        #region METODOS AUXILIARES

        /// <summary>
        /// Metodo genérico para ejecutar una acción que afecta a la UI,
        /// </summary>
        /// <param name="boton"></param>
        /// <param name="accion"></param>
        /// <returns>Retorna una tarea asíncrona</returns>
        private async Task EjecutarAccionUIAsync(Button boton, Func<Task> accion)
        {
            string textoOriginal = boton?.Text;

            try
            {
                if (boton != null)
                {
                    boton.Enabled = false;
                    boton.Text = "Sincronizando...";
                }

                Cursor = Cursors.WaitCursor;

                progressBarSync.Visible = true;

                progressBarSync.Refresh();
                await Task.Yield();

                progressBarSync.Value = 100;

                await accion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                progressBarSync.Value = 0;
                progressBarSync.Visible = false;

                if (boton != null)
                {
                    boton.Enabled = true;
                    boton.Text = textoOriginal;
                }

                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Metodo para buscar metadatos con fallback a búsqueda manual si no hay resultados
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="artista"></param>
        /// <returns>Retorna los metadatos seleccionados o null si se cancela</returns>
        private async Task<MetadatosAPI.MetadataResult> BuscarMetadatosConFallback(string titulo, string artista)
        {
            var datosIniciales = MetadatosAPI.ParsearBusqueda($"{titulo} {artista}");
            var lista = await MetadatosAPI.BuscarLista(datosIniciales.titulo, datosIniciales.artista);

            if (lista.Count > 0)
            {
                using var frm = new FrmSeleccionarMetadatos(lista);
                if (frm.ShowDialog() == DialogResult.OK)
                    return frm.Seleccionado;

                return null;
            }

            // Si no hay resultados, pedir búsqueda manual al usuario
            string manual = Microsoft.VisualBasic.Interaction.InputBox(
                "No se encontraron coincidencias.\n\nIntroduce una búsqueda manual (ej: Beyoncé - Halo):",
                "Buscar manualmente",
                datosIniciales.titulo + " - " + datosIniciales.artista
            );

            if (string.IsNullOrWhiteSpace(manual))
                return null;

            // Re-parsear búsqueda manual del usuario
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

        #endregion
    }
}
