#if DEBUG
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MusicManager.Forms
{
    public partial class FrmConsola : Form
    {
        private readonly string rutaLog;
        private readonly Timer timerAutoRefresh;

        public FrmConsola()
        {
            InitializeComponent();

            rutaLog = Path.Combine(Program.appMusic.rutaBase, "logs", "app.log");

            // Configurar el TextBox
            txtConsola.ScrollBars = ScrollBars.Both;
            txtConsola.ReadOnly = true;
            txtConsola.Font = new Font("Consolas", 9);
            txtConsola.WordWrap = false;

            // Configurar el Timer
            timerAutoRefresh = new Timer();
            timerAutoRefresh.Interval = 3000; // cada 3 segundos
            timerAutoRefresh.Tick += (s, e) => CargarLog();

            // Cargar log al iniciar
            CargarLog();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarLog();
        }

        private void chkAutoActualizar_CheckedChanged(object sender, EventArgs e)
        {
            bool auto = chkAutoActualizar.Checked;
            btnActualizar.Enabled = !auto;

            if (auto)
                timerAutoRefresh.Start();
            else
                timerAutoRefresh.Stop();
        }

        private void CargarLog()
        {
            try
            {
                if (File.Exists(rutaLog))
                {
                    txtConsola.Text = File.ReadAllText(rutaLog);
                    txtConsola.SelectionStart = txtConsola.Text.Length;
                    txtConsola.ScrollToCaret();
                }
                else
                {
                    txtConsola.Text = "[Sin archivo de log disponible]";
                }
            }
            catch (Exception ex)
            {
                txtConsola.Text = $"Error al leer log: {ex.Message}";
                Program.appMusic.RegistrarLog("Error al leer log", ex.Message);
            }
        }
    }
}
#endif
