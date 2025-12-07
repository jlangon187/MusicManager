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
            lblTitulo = new Label();
            lblArtista = new Label();
            lblAlbum = new Label();
            lblGenero = new Label();
            lblAnio = new Label();
            lblDuracion = new Label();
            lblRuta = new Label();
            txtTitulo = new TextBox();
            txtArtista = new TextBox();
            txtAlbum = new TextBox();
            txtAnio = new TextBox();
            txtDuracion = new TextBox();
            txtRuta = new TextBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            pbPortada = new PictureBox();
            btnAdd = new Button();
            cbGenero = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pbPortada).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(41, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Título:";
            // 
            // lblArtista
            // 
            lblArtista.AutoSize = true;
            lblArtista.Location = new Point(20, 60);
            lblArtista.Name = "lblArtista";
            lblArtista.Size = new Size(44, 15);
            lblArtista.TabIndex = 1;
            lblArtista.Text = "Artista:";
            // 
            // lblAlbum
            // 
            lblAlbum.AutoSize = true;
            lblAlbum.Location = new Point(20, 100);
            lblAlbum.Name = "lblAlbum";
            lblAlbum.Size = new Size(46, 15);
            lblAlbum.TabIndex = 2;
            lblAlbum.Text = "Álbum:";
            // 
            // lblGenero
            // 
            lblGenero.AutoSize = true;
            lblGenero.Location = new Point(20, 140);
            lblGenero.Name = "lblGenero";
            lblGenero.Size = new Size(48, 15);
            lblGenero.TabIndex = 3;
            lblGenero.Text = "Género:";
            // 
            // lblAnio
            // 
            lblAnio.AutoSize = true;
            lblAnio.Location = new Point(20, 180);
            lblAnio.Name = "lblAnio";
            lblAnio.Size = new Size(32, 15);
            lblAnio.TabIndex = 4;
            lblAnio.Text = "Año:";
            // 
            // lblDuracion
            // 
            lblDuracion.AutoSize = true;
            lblDuracion.Location = new Point(20, 220);
            lblDuracion.Name = "lblDuracion";
            lblDuracion.Size = new Size(58, 15);
            lblDuracion.TabIndex = 5;
            lblDuracion.Text = "Duración:";
            // 
            // lblRuta
            // 
            lblRuta.AutoSize = true;
            lblRuta.Location = new Point(20, 260);
            lblRuta.Name = "lblRuta";
            lblRuta.Size = new Size(95, 15);
            lblRuta.TabIndex = 6;
            lblRuta.Text = "Ruta del archivo:";
            // 
            // txtTitulo
            // 
            txtTitulo.Location = new Point(150, 18);
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(300, 23);
            txtTitulo.TabIndex = 7;
            // 
            // txtArtista
            // 
            txtArtista.Location = new Point(150, 58);
            txtArtista.Name = "txtArtista";
            txtArtista.Size = new Size(300, 23);
            txtArtista.TabIndex = 8;
            // 
            // txtAlbum
            // 
            txtAlbum.Location = new Point(150, 98);
            txtAlbum.Name = "txtAlbum";
            txtAlbum.Size = new Size(300, 23);
            txtAlbum.TabIndex = 9;
            // 
            // txtAnio
            // 
            txtAnio.Location = new Point(150, 178);
            txtAnio.Name = "txtAnio";
            txtAnio.Size = new Size(100, 23);
            txtAnio.TabIndex = 11;
            txtAnio.KeyPress += txtAnio_KeyPress;
            // 
            // txtDuracion
            // 
            txtDuracion.Location = new Point(150, 218);
            txtDuracion.Name = "txtDuracion";
            txtDuracion.ReadOnly = true;
            txtDuracion.Size = new Size(100, 23);
            txtDuracion.TabIndex = 12;
            // 
            // txtRuta
            // 
            txtRuta.Location = new Point(150, 258);
            txtRuta.Name = "txtRuta";
            txtRuta.ReadOnly = true;
            txtRuta.Size = new Size(480, 23);
            txtRuta.TabIndex = 13;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(150, 300);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(150, 32);
            btnGuardar.TabIndex = 14;
            btnGuardar.Text = "Guardar cambios";
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(320, 300);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 32);
            btnCancelar.TabIndex = 15;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;
            // 
            // pbPortada
            // 
            pbPortada.Location = new Point(462, 17);
            pbPortada.Name = "pbPortada";
            pbPortada.Size = new Size(185, 185);
            pbPortada.SizeMode = PictureBoxSizeMode.StretchImage;
            pbPortada.TabIndex = 16;
            pbPortada.TabStop = false;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(320, 139);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(60, 23);
            btnAdd.TabIndex = 17;
            btnAdd.Text = "Nuevo";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // cbGenero
            // 
            cbGenero.FormattingEnabled = true;
            cbGenero.Location = new Point(150, 140);
            cbGenero.Name = "cbGenero";
            cbGenero.Size = new Size(164, 23);
            cbGenero.TabIndex = 18;
            // 
            // FrmEditarMetadatos
            // 
            ClientSize = new Size(660, 360);
            Controls.Add(cbGenero);
            Controls.Add(btnAdd);
            Controls.Add(pbPortada);
            Controls.Add(lblTitulo);
            Controls.Add(lblArtista);
            Controls.Add(lblAlbum);
            Controls.Add(lblGenero);
            Controls.Add(lblAnio);
            Controls.Add(lblDuracion);
            Controls.Add(lblRuta);
            Controls.Add(txtTitulo);
            Controls.Add(txtArtista);
            Controls.Add(txtAlbum);
            Controls.Add(txtAnio);
            Controls.Add(txtDuracion);
            Controls.Add(txtRuta);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MaximumSize = new Size(676, 399);
            MinimizeBox = false;
            MinimumSize = new Size(676, 399);
            Name = "FrmEditarMetadatos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar Metadatos";
            Load += FrmEditarMetadatos_Load;
            ((System.ComponentModel.ISupportInitialize)pbPortada).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private PictureBox pbPortada;
        private Button btnAdd;
        private ComboBox cbGenero;
    }
}
