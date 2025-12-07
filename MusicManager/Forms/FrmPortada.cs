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

        internal void SetImagen(Image image)
        {
            pbPortada.Image = image;
        }

        private void pbPortada_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
