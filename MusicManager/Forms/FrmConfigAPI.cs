using MusicManager.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusicManager.Forms
{
    public partial class FrmConfigAPI : Form
    {
        public FrmConfigAPI()
        {
            InitializeComponent();
            CargarEstadoActual();
        }

        private void CargarEstadoActual()
        {
            chkItunes.Checked = MetadatosAPI.UsarITunes;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            MetadatosAPI.ConfigurarUsoAPIs(
                chkItunes.Checked
            );

            lblEstado.Text = "Configuración guardada.";
            lblEstado.ForeColor = Color.Green;
        }

        private async void btnTestItunes_Click(object sender, EventArgs e)
        {
            lblEstado.Text = "Probando conexión iTunes...";
            lblEstado.ForeColor = Color.DarkGoldenrod;

            bool ok = await MetadatosAPI.ProbarConexionITunes();

            lblEstado.Text = ok ? "iTunes funciona correctamente." : "Error con iTunes.";
            lblEstado.ForeColor = ok ? Color.Green : Color.Red;
        }
    }
}
