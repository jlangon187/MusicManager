namespace MusicManager
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelLateral;
        private Panel panelContenido;
        private Panel panelEstadisticas;
        private Panel panelReproductor;

        private Button btnSeleccionarCarpeta;
        private Button btnDescargarMetadatos;
        private Button btnOrganizar;

        private TextBox txtBuscar;
        private DataGridView dgvCanciones;

        private Button btnPlay;
        private Button btnPausa;
        private Label lblReproduciendo;

        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colArtista;
        private DataGridViewTextBoxColumn colAlbum;
        private DataGridViewTextBoxColumn colGenero;
        private DataGridViewTextBoxColumn colAnno;
        private DataGridViewTextBoxColumn colRuta;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelLateral = new Panel();
            btnOrganizar = new Button();
            btnDescargarMetadatos = new Button();
            btnSeleccionarCarpeta = new Button();
            panelContenido = new Panel();
            dgvCanciones = new DataGridView();
            colTitulo = new DataGridViewTextBoxColumn();
            colArtista = new DataGridViewTextBoxColumn();
            colAlbum = new DataGridViewTextBoxColumn();
            colGenero = new DataGridViewTextBoxColumn();
            colAnno = new DataGridViewTextBoxColumn();
            colRuta = new DataGridViewTextBoxColumn();
            txtBuscar = new TextBox();
            panelEstadisticas = new Panel();
            panelReproductor = new Panel();
            btnPlay = new Button();
            btnPausa = new Button();
            lblReproduciendo = new Label();
            statusStrip1 = new StatusStrip();
            lblTotalArtistas = new ToolStripStatusLabel();
            lblTotalAlbumes = new ToolStripStatusLabel();
            lblTotalGeneros = new ToolStripStatusLabel();
            lblDuracionTotal = new ToolStripStatusLabel();
            lblTotalCanciones = new ToolStripStatusLabel();
            panelLateral.SuspendLayout();
            panelContenido.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).BeginInit();
            panelReproductor.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panelLateral
            // 
            panelLateral.BackColor = Color.LightGray;
            panelLateral.Controls.Add(btnOrganizar);
            panelLateral.Controls.Add(btnDescargarMetadatos);
            panelLateral.Controls.Add(btnSeleccionarCarpeta);
            panelLateral.Dock = DockStyle.Left;
            panelLateral.Location = new Point(0, 0);
            panelLateral.Name = "panelLateral";
            panelLateral.Padding = new Padding(10);
            panelLateral.Size = new Size(180, 655);
            panelLateral.TabIndex = 2;
            // 
            // btnOrganizar
            // 
            btnOrganizar.Dock = DockStyle.Top;
            btnOrganizar.Location = new Point(10, 90);
            btnOrganizar.Name = "btnOrganizar";
            btnOrganizar.Size = new Size(160, 40);
            btnOrganizar.TabIndex = 0;
            btnOrganizar.Text = "Organizar Música";
            btnOrganizar.Click += btnOrganizar_Click;
            // 
            // btnDescargarMetadatos
            // 
            btnDescargarMetadatos.Dock = DockStyle.Top;
            btnDescargarMetadatos.Location = new Point(10, 50);
            btnDescargarMetadatos.Name = "btnDescargarMetadatos";
            btnDescargarMetadatos.Size = new Size(160, 40);
            btnDescargarMetadatos.TabIndex = 1;
            btnDescargarMetadatos.Text = "Descargar Metadatos";
            btnDescargarMetadatos.Click += btnDescargarMetadatos_Click;
            // 
            // btnSeleccionarCarpeta
            // 
            btnSeleccionarCarpeta.Dock = DockStyle.Top;
            btnSeleccionarCarpeta.Location = new Point(10, 10);
            btnSeleccionarCarpeta.Name = "btnSeleccionarCarpeta";
            btnSeleccionarCarpeta.Size = new Size(160, 40);
            btnSeleccionarCarpeta.TabIndex = 2;
            btnSeleccionarCarpeta.Text = "Seleccionar Carpeta";
            btnSeleccionarCarpeta.Click += btnSeleccionarCarpeta_Click;
            // 
            // panelContenido
            // 
            panelContenido.Controls.Add(statusStrip1);
            panelContenido.Controls.Add(dgvCanciones);
            panelContenido.Controls.Add(txtBuscar);
            panelContenido.Controls.Add(panelEstadisticas);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(180, 0);
            panelContenido.Name = "panelContenido";
            panelContenido.Padding = new Padding(10);
            panelContenido.Size = new Size(912, 595);
            panelContenido.TabIndex = 0;
            // 
            // dgvCanciones
            // 
            dgvCanciones.AllowUserToAddRows = false;
            dgvCanciones.Columns.AddRange(new DataGridViewColumn[] { colTitulo, colArtista, colAlbum, colGenero, colAnno, colRuta });
            dgvCanciones.Dock = DockStyle.Fill;
            dgvCanciones.Location = new Point(10, 61);
            dgvCanciones.Name = "dgvCanciones";
            dgvCanciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCanciones.Size = new Size(892, 524);
            dgvCanciones.TabIndex = 0;
            dgvCanciones.CellDoubleClick += dgvCanciones_CellDoubleClick;
            // 
            // colTitulo
            // 
            colTitulo.HeaderText = "Título";
            colTitulo.Name = "colTitulo";
            // 
            // colArtista
            // 
            colArtista.HeaderText = "Artista";
            colArtista.Name = "colArtista";
            // 
            // colAlbum
            // 
            colAlbum.HeaderText = "Álbum";
            colAlbum.Name = "colAlbum";
            // 
            // colGenero
            // 
            colGenero.HeaderText = "Género";
            colGenero.Name = "colGenero";
            // 
            // colAnno
            // 
            colAnno.HeaderText = "Año";
            colAnno.Name = "colAnno";
            // 
            // colRuta
            // 
            colRuta.HeaderText = "Ruta";
            colRuta.Name = "colRuta";
            colRuta.Visible = false;
            // 
            // txtBuscar
            // 
            txtBuscar.Dock = DockStyle.Top;
            txtBuscar.Location = new Point(10, 38);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(892, 23);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // panelEstadisticas
            // 
            panelEstadisticas.BackColor = Color.WhiteSmoke;
            panelEstadisticas.Dock = DockStyle.Top;
            panelEstadisticas.Location = new Point(10, 10);
            panelEstadisticas.Name = "panelEstadisticas";
            panelEstadisticas.Size = new Size(892, 28);
            panelEstadisticas.TabIndex = 2;
            // 
            // panelReproductor
            // 
            panelReproductor.BackColor = Color.Silver;
            panelReproductor.Controls.Add(btnPlay);
            panelReproductor.Controls.Add(btnPausa);
            panelReproductor.Controls.Add(lblReproduciendo);
            panelReproductor.Dock = DockStyle.Bottom;
            panelReproductor.Location = new Point(180, 595);
            panelReproductor.Name = "panelReproductor";
            panelReproductor.Size = new Size(912, 60);
            panelReproductor.TabIndex = 1;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(6, 33);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(75, 23);
            btnPlay.TabIndex = 0;
            btnPlay.Text = "Play";
            btnPlay.Click += btnPlay_Click;
            // 
            // btnPausa
            // 
            btnPausa.Location = new Point(87, 33);
            btnPausa.Name = "btnPausa";
            btnPausa.Size = new Size(75, 23);
            btnPausa.TabIndex = 1;
            btnPausa.Text = "Pausa";
            btnPausa.Click += btnPausa_Click;
            // 
            // lblReproduciendo
            // 
            lblReproduciendo.Location = new Point(9, 11);
            lblReproduciendo.Name = "lblReproduciendo";
            lblReproduciendo.Size = new Size(100, 23);
            lblReproduciendo.TabIndex = 2;
            lblReproduciendo.Text = "Reproduciendo:";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblTotalArtistas, lblTotalAlbumes, lblTotalGeneros, lblDuracionTotal, lblTotalCanciones });
            statusStrip1.Location = new Point(10, 563);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(892, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblTotalArtistas
            // 
            lblTotalArtistas.AutoSize = false;
            lblTotalArtistas.Margin = new Padding(0, 3, 20, 2);
            lblTotalArtistas.Name = "lblTotalArtistas";
            lblTotalArtistas.Size = new Size(90, 17);
            lblTotalArtistas.Text = "Generos:";
            lblTotalArtistas.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalAlbumes
            // 
            lblTotalAlbumes.AutoSize = false;
            lblTotalAlbumes.Margin = new Padding(0, 3, 20, 2);
            lblTotalAlbumes.Name = "lblTotalAlbumes";
            lblTotalAlbumes.Size = new Size(90, 17);
            lblTotalAlbumes.Text = "Albumes:";
            lblTotalAlbumes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalGeneros
            // 
            lblTotalGeneros.AutoSize = false;
            lblTotalGeneros.Margin = new Padding(0, 3, 20, 2);
            lblTotalGeneros.Name = "lblTotalGeneros";
            lblTotalGeneros.Size = new Size(90, 17);
            lblTotalGeneros.Text = "Generos:";
            lblTotalGeneros.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDuracionTotal
            // 
            lblDuracionTotal.AutoSize = false;
            lblDuracionTotal.Margin = new Padding(0, 3, 20, 2);
            lblDuracionTotal.Name = "lblDuracionTotal";
            lblDuracionTotal.Size = new Size(150, 17);
            lblDuracionTotal.Text = "Duración total:";
            lblDuracionTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalCanciones
            // 
            lblTotalCanciones.AutoSize = false;
            lblTotalCanciones.Margin = new Padding(0, 3, 20, 2);
            lblTotalCanciones.Name = "lblTotalCanciones";
            lblTotalCanciones.Size = new Size(150, 17);
            lblTotalCanciones.Text = "Total canciones:";
            lblTotalCanciones.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FrmMain
            // 
            ClientSize = new Size(1092, 655);
            Controls.Add(panelContenido);
            Controls.Add(panelReproductor);
            Controls.Add(panelLateral);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MusicManager";
            Load += FrmMain_Load;
            panelLateral.ResumeLayout(false);
            panelContenido.ResumeLayout(false);
            panelContenido.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).EndInit();
            panelReproductor.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
        }
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblTotalArtistas;
        private ToolStripStatusLabel lblTotalAlbumes;
        private ToolStripStatusLabel lblTotalGeneros;
        private ToolStripStatusLabel lblDuracionTotal;
        private ToolStripStatusLabel lblTotalCanciones;
    }
}