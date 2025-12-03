namespace MusicManager.Forms
{
    partial class FrmEditarMetadatos
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblArtista;
        private System.Windows.Forms.Label lblAlbum;
        private System.Windows.Forms.Label lblGenero;
        private System.Windows.Forms.Label lblAnio;
        private System.Windows.Forms.Label lblDuracion;
        private System.Windows.Forms.Label lblRuta;

        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.TextBox txtArtista;
        private System.Windows.Forms.TextBox txtAlbum;
        private System.Windows.Forms.TextBox txtGenero;
        private System.Windows.Forms.TextBox txtAnio;
        private System.Windows.Forms.TextBox txtDuracion;
        private System.Windows.Forms.TextBox txtRuta;

        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblArtista = new System.Windows.Forms.Label();
            this.lblAlbum = new System.Windows.Forms.Label();
            this.lblGenero = new System.Windows.Forms.Label();
            this.lblAnio = new System.Windows.Forms.Label();
            this.lblDuracion = new System.Windows.Forms.Label();
            this.lblRuta = new System.Windows.Forms.Label();

            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.txtArtista = new System.Windows.Forms.TextBox();
            this.txtAlbum = new System.Windows.Forms.TextBox();
            this.txtGenero = new System.Windows.Forms.TextBox();
            this.txtAnio = new System.Windows.Forms.TextBox();
            this.txtDuracion = new System.Windows.Forms.TextBox();
            this.txtRuta = new System.Windows.Forms.TextBox();

            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ===========================================================
            // Labels
            // ===========================================================

            this.lblTitulo.Text = "Título:";
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.AutoSize = true;

            this.lblArtista.Text = "Artista:";
            this.lblArtista.Location = new System.Drawing.Point(20, 60);
            this.lblArtista.AutoSize = true;

            this.lblAlbum.Text = "Álbum:";
            this.lblAlbum.Location = new System.Drawing.Point(20, 100);
            this.lblAlbum.AutoSize = true;

            this.lblGenero.Text = "Género:";
            this.lblGenero.Location = new System.Drawing.Point(20, 140);
            this.lblGenero.AutoSize = true;

            this.lblAnio.Text = "Año:";
            this.lblAnio.Location = new System.Drawing.Point(20, 180);
            this.lblAnio.AutoSize = true;

            this.lblDuracion.Text = "Duración:";
            this.lblDuracion.Location = new System.Drawing.Point(20, 220);
            this.lblDuracion.AutoSize = true;

            this.lblRuta.Text = "Ruta del archivo:";
            this.lblRuta.Location = new System.Drawing.Point(20, 260);
            this.lblRuta.AutoSize = true;

            // ===========================================================
            // TextBoxes
            // ===========================================================

            this.txtTitulo.Location = new System.Drawing.Point(150, 18);
            this.txtTitulo.Size = new System.Drawing.Size(300, 23);

            this.txtArtista.Location = new System.Drawing.Point(150, 58);
            this.txtArtista.Size = new System.Drawing.Size(300, 23);

            this.txtAlbum.Location = new System.Drawing.Point(150, 98);
            this.txtAlbum.Size = new System.Drawing.Size(300, 23);

            this.txtGenero.Location = new System.Drawing.Point(150, 138);
            this.txtGenero.Size = new System.Drawing.Size(300, 23);

            this.txtAnio.Location = new System.Drawing.Point(150, 178);
            this.txtAnio.Size = new System.Drawing.Size(100, 23);

            this.txtDuracion.Location = new System.Drawing.Point(150, 218);
            this.txtDuracion.Size = new System.Drawing.Size(100, 23);
            this.txtDuracion.ReadOnly = true;

            this.txtRuta.Location = new System.Drawing.Point(150, 258);
            this.txtRuta.Size = new System.Drawing.Size(480, 23);
            this.txtRuta.ReadOnly = true;

            // ===========================================================
            // Buttons
            // ===========================================================

            this.btnGuardar.Text = "Guardar cambios";
            this.btnGuardar.Location = new System.Drawing.Point(150, 300);
            this.btnGuardar.Size = new System.Drawing.Size(150, 32);
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Location = new System.Drawing.Point(320, 300);
            this.btnCancelar.Size = new System.Drawing.Size(120, 32);
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // ===========================================================
            // Form
            // ===========================================================

            this.ClientSize = new System.Drawing.Size(660, 360);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblArtista);
            this.Controls.Add(this.lblAlbum);
            this.Controls.Add(this.lblGenero);
            this.Controls.Add(this.lblAnio);
            this.Controls.Add(this.lblDuracion);
            this.Controls.Add(this.lblRuta);

            this.Controls.Add(this.txtTitulo);
            this.Controls.Add(this.txtArtista);
            this.Controls.Add(this.txtAlbum);
            this.Controls.Add(this.txtGenero);
            this.Controls.Add(this.txtAnio);
            this.Controls.Add(this.txtDuracion);
            this.Controls.Add(this.txtRuta);

            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Text = "Editar Metadatos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            this.Load += new System.EventHandler(this.FrmEditarMetadatos_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
