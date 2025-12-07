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
        }

        // Eventos de los botones de prueba de conexión
        private async void btnTestItunes_Click(object sender, EventArgs e)
        {
            lblEstadoItunes.Text = "Probando conexión iTunes...";
            lblEstadoItunes.ForeColor = Color.DarkGoldenrod;

            bool ok = await MetadatosAPI.ProbarConexionITunes();

            lblEstadoItunes.Text = ok ? "iTunes funciona correctamente." : "Error con iTunes.";
            lblEstadoItunes.ForeColor = ok ? Color.Green : Color.Red;
        }

        // Eventos de los botones de prueba de conexión
        private async void btnTestSpotify_Click(object sender, EventArgs e)
        {
            lblEstadoSpotify.Text = "Probando conexión Spotify...";
            lblEstadoSpotify.ForeColor = Color.DarkGoldenrod;

            bool ok = await MetadatosAPI.ProbarConexionSpotify();

            lblEstadoSpotify.Text = ok ? "Spotify funciona correctamente." : "Error con Spotify.";
            lblEstadoSpotify.ForeColor = ok ? Color.Green : Color.Red;
        }

        // Evento del botón Guardar
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
