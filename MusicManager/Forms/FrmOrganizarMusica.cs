using MusicManager.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MusicManager.Forms
{
    public partial class FrmOrganizarMusica : Form
    {
        private DataGridView grid;                                      // Referencia al DataGridView con las canciones
        private string carpetaBase;                                     // Carpeta base para organizar la música

        public int ModoSeleccionado => cbModo.SelectedIndex + 1;        // Modo seleccionado (1 a 4)   

        /// <summary>
        /// Constructor del formulario de organización de música.
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="carpeta"></param>
        public FrmOrganizarMusica(DataGridView dgv, string carpeta)
        {
            InitializeComponent();

            ThemeManager.ApplyTheme(this);    // Aplicar tema al formulario

            grid = dgv;                     // Asignar referencia al DataGridView
            carpetaBase = carpeta;          // Asignar carpeta base

            // Configurar ComboBox con modos de organización

            cbModo.Items.Add("1. Artista / Álbum");
            cbModo.Items.Add("2. Género / Año");
            cbModo.Items.Add("3. Año / Artista");
            cbModo.Items.Add("4. Artista / Año / Álbum");

            cbModo.SelectedIndex = 0;

            // Actualizar vista previa inicial
            ActualizarVistaPrevia();
        }

        /// <summary>
        /// Manejador del evento de cambio de selección en el ComboBox de modos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbModo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ActualizarVistaPrevia();
            }
            catch (Exception ex)
            {
                Program.appMusic.RegistrarLog("Error al actualizar vista previa en FrmOrganizarMusica: ", ex.Message);
            }
        }

        /// <summary>
        /// Método para actualizar la vista previa de las rutas antigua y nueva.
        /// </summary>
        private void ActualizarVistaPrevia()
        {
            if (grid.Rows.Count == 0)
            {
                txtRutaAntigua.Text = "No hay canciones cargadas.";
                txtRutaNueva.Text = "";
                return;
            }

            // Tomar la primera canción como ejemplo
            var row = grid.Rows[0];
            string ruta = row.Cells["colRuta"].Value.ToString();
            string titulo = row.Cells["colTitulo"].Value.ToString();
            string artista = row.Cells["colArtista"].Value.ToString();
            string album = row.Cells["colAlbum"].Value.ToString();
            string genero = row.Cells["colGenero"].Value.ToString();
            string anio = row.Cells["colAnno"].Value.ToString();

            txtRutaAntigua.Text = RecortarRuta(ruta, 2);

            // Calcular carpeta destino
            string carpeta = Organizador.ObtenerCarpetaDestino(
                carpetaBase, artista, album, genero, anio, ModoSeleccionado);

            string nombreArchivo = Path.GetFileName(ruta);
            string nuevaRuta = Path.Combine(carpeta, nombreArchivo);

            if (ModoSeleccionado == 4)
            {
                txtRutaNueva.Text = RecortarRuta(nuevaRuta, 5);
            }
            else
            {
                txtRutaNueva.Text = RecortarRuta(nuevaRuta, 4);
            }

            toolTip1.SetToolTip(txtRutaAntigua, ruta);
            toolTip1.SetToolTip(txtRutaNueva, nuevaRuta);

        }

        /// <summary>
        /// Método para recortar una ruta de archivo y mostrar solo los últimos niveles.
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="niveles"></param>
        /// <returns></returns>
        public static string RecortarRuta(string ruta, int niveles = 3)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return ruta;

            var partes = ruta.Split(Path.DirectorySeparatorChar);

            if (partes.Length <= niveles)
                return ruta;

            string final = string.Join(Path.DirectorySeparatorChar, partes[^niveles..]);
            return $"...{Path.DirectorySeparatorChar}{final}";
        }

        /// <summary>
        /// Evento del botón Aceptar para confirmar la organización.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Evento del botón Cerrar para cancelar la organización.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
