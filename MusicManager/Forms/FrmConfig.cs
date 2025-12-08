using MusicManager.App;
using MusicManager.Utils;
using System.Text.Json;

namespace MusicManager.Forms
{
    public partial class FrmConfig : Form
    {
        public FrmConfig()
        {
            InitializeComponent();
            this.Load += FrmConnection_Load;
        }

        private void btnConexion_Click(object sender, EventArgs e)
        {
            try
            {
                SetControlesEstadoConexion(true);

                if (Program.appMusic.conectado)
                {
                    // Desconecta
                    Program.appMusic.DesconectarDB();
                    if (Application.OpenForms["FrmMain"] is FrmMain mainForm)
                    {
                        mainForm.RefreshToolBar();
                        mainForm.RefreshStatusBar();
                    }

                }
                else
                {
                    // Actualiza config con los valores del formulario
                    Program.appMusic.configConexion = new ConfiguracionConexion
                    {
                        servidor = txtServidor.Text,
                        puerto = int.Parse(txtPuerto.Text),
                        usuario = txtUsuario.Text,
                        password = txtPassword.Text,
                        baseDatos = txtBaseDatos.Text
                    };

                    // Intenta conectar
                    Program.appMusic.ConectarDB();
                    if (Application.OpenForms["FrmMain"] is FrmMain mainForm)
                    {
                        mainForm.RefreshToolBar();
                        mainForm.RefreshStatusBar();
                    }

                }

                // Refrescar estado visual local
                if (Program.appMusic.conectado)
                {
                    tsStatusLabel.Text = "Conexión establecida correctamente.";
                    tsStatusLabel.ForeColor = Color.Green;
                    btnConexion.Text = "Cerrar conexión";
                }
                else
                {
                    tsStatusLabel.Text = "Conexión cerrada.";
                    tsStatusLabel.ForeColor = Color.Black;
                    btnConexion.Text = "Conectar";
                }

                // Actualizar el FrmMain si existe
                FrmMain frmMain = Application.OpenForms["FrmMain"] as FrmMain;

                if (frmMain != null)
                {
                    frmMain.Invoke(new Action(() =>
                    {
                        frmMain.RefreshToolBar();
                        frmMain.RefreshStatusBar();
                    }));
                }
            }
            catch (Exception ex)
            {
                tsStatusLabel.Text = "Error: " + ex.Message;
                tsStatusLabel.ForeColor = Color.Red;
                Program.appMusic.RegistrarLog("FrmConfig.btnConexion_Click", ex.Message);
            }
            finally
            {
                SetControlesEstadoConexion(false);
            }
        }

        private void SetControlesEstadoConexion(bool enProceso)
        {
            pnData.Enabled = !enProceso;
            btnConexion.Enabled = !enProceso;
            tsProgressBarConexion.Visible = enProceso;
            tsProgressBarConexion.Style = enProceso ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        }


        private void GuardarConfiguracionEnArchivo(string aRuta, ConfiguracionConexion aConfig)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string jsonText = JsonSerializer.Serialize(aConfig, options);

            File.WriteAllText(aRuta, jsonText);
            tsLbRutaConfig.Text = aRuta;

        }

        private void tsBtnCargar_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Archivo JSON|*.json",
                Title = "Seleccionar archivo de configuración"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Desactivar controles mientras carga
                    SetControlesEstadoConexion(true);

                    // Configura la conexión (esto carga el JSON e intenta conectar)
                    Program.appMusic.ConfiguraYConectaDB(dlg.FileName);

                    // Muestra los datos siempre que haya cargado el JSON, aunque no haya conexión
                    if (Program.appMusic.configConexion != null)
                    {
                        txtServidor.Text = Program.appMusic.configConexion.servidor;
                        txtPuerto.Text = Program.appMusic.configConexion.puerto.ToString();
                        txtUsuario.Text = Program.appMusic.configConexion.usuario;
                        txtPassword.Text = Program.appMusic.configConexion.password;
                        txtBaseDatos.Text = Program.appMusic.configConexion.baseDatos;
                        tsLbRutaConfig.Text = dlg.FileName;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo cargar el archivo de configuración.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Reactivar controles
                    SetControlesEstadoConexion(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar el archivo: " + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.appMusic.RegistrarLog("Error al cargar el archivo", ex.Message);
                }
            }
        }


        private void tsBtnGuardar_Click(object sender, EventArgs e)
        {

            using SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Archivo JSON|*.json",
                Title = "Guardar configuración como..."
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ConfiguracionConexion config = new ConfiguracionConexion
                {
                    servidor = txtServidor.Text,
                    puerto = int.Parse(txtPuerto.Text),
                    usuario = txtUsuario.Text,
                    password = txtPassword.Text,
                    baseDatos = txtBaseDatos.Text
                };

                try
                {
                    GuardarConfiguracionEnArchivo(dlg.FileName, config);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.appMusic.RegistrarLog("Error al guardar el archivo", ex.Message);
                }
            }
        }

        private void FrmConnection_Load(object sender, EventArgs e)
        {
            // Aplica el tema visual actual
            ThemeManager.ApplyTheme(this);

            // Si hay configuración cargada, la mostramos
            if (Program.appMusic.configConexion != null)
            {
                txtServidor.Text = Program.appMusic.configConexion.servidor;
                txtPuerto.Text = Program.appMusic.configConexion.puerto.ToString();
                txtUsuario.Text = Program.appMusic.configConexion.usuario;
                txtPassword.Text = Program.appMusic.configConexion.password;
                txtBaseDatos.Text = Program.appMusic.configConexion.baseDatos;
            }

            // Muestra el estado actual de la conexión global
            ActualizarEstadoConexion();

            // Desactiva el indicador de progreso al cargar
            tsProgressBarConexion.Visible = false;
        }

        private void ActualizarEstadoConexion()
        {
            if (Program.appMusic.conectado)
            {
                tsStatusLabel.Text = "Conexión establecida correctamente.";
                tsStatusLabel.ForeColor = Color.Green;
                btnConexion.Text = "Cerrar conexión";
            }
            else
            {
                tsStatusLabel.Text = "Conexión cerrada.";
                tsStatusLabel.ForeColor = Color.Black;
                btnConexion.Text = "Conectar";
            }
        }
    }
}
