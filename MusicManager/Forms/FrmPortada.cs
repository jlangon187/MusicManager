using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicManager.Forms
{
    public partial class FrmPortada : Form
    {
        public FrmPortada()
        {
            InitializeComponent();
        }

        // Establece la imagen de la portada en el PictureBox
        internal void SetImagen(Image image)
        {
            pbPortada.Image = image;
        }

        // Cierra el formulario al hacer clic en el PictureBox
        private void pbPortada_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
