using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Forms
{
    public partial class frmAcerca : Form
    {
        // Constructor
        public frmAcerca()
        {
            InitializeComponent();
        }

        // Evento Load
        private void frmAcerca_Load(object sender, EventArgs e)
        {
            lblPayPal.Text = "Ayúdame con una donación";
            lblPayPal.Links.Clear();
            lblPayPal.Links.Add(0, lblPayPal.Text.Length, "https://www.paypal.com/es/home");
        }

        // Evento LinkClicked
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblPayPal.LinkVisited = true;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Link.LinkData.ToString(),
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el enlace.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
