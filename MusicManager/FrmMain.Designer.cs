namespace MusicManager
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelReproductor;

        private Button btnPlay;
        private Button btnPausa;
        private Label lblReproduciendo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelReproductor = new Panel();
            lblReproduciendo = new Label();
            btnPlay = new Button();
            btnPausa = new Button();
            panel1 = new Panel();
            menuStrip1 = new MenuStrip();
            archivoToolStripMenuItem = new ToolStripMenuItem();
            configuraciónToolStripMenuItem = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            consolaDeDepuraciónToolStripMenuItem = new ToolStripMenuItem();
            panelContenido = new Panel();
            dgvCanciones = new DataGridView();
            colTitulo = new DataGridViewTextBoxColumn();
            colArtista = new DataGridViewTextBoxColumn();
            colAlbum = new DataGridViewTextBoxColumn();
            colGenero = new DataGridViewTextBoxColumn();
            colAnno = new DataGridViewTextBoxColumn();
            colRuta = new DataGridViewTextBoxColumn();
            txtBuscar = new TextBox();
            statusStrip1 = new StatusStrip();
            lbConexionDB = new ToolStripStatusLabel();
            lblTotalArtistas = new ToolStripStatusLabel();
            lblTotalAlbumes = new ToolStripStatusLabel();
            lblTotalGeneros = new ToolStripStatusLabel();
            lblDuracionTotal = new ToolStripStatusLabel();
            lblTotalCanciones = new ToolStripStatusLabel();
            panelLateral = new Panel();
            btnOrganizar = new Button();
            btnDescargarMetadatos = new Button();
            btnSeleccionarCarpeta = new Button();
            btnSincronizar = new Button();
            panelReproductor.SuspendLayout();
            panel1.SuspendLayout();
            menuStrip1.SuspendLayout();
            panelContenido.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).BeginInit();
            statusStrip1.SuspendLayout();
            panelLateral.SuspendLayout();
            SuspendLayout();
            // 
            // panelReproductor
            // 
            panelReproductor.BackColor = Color.Silver;
            panelReproductor.Controls.Add(lblReproduciendo);
            panelReproductor.Controls.Add(btnPlay);
            panelReproductor.Controls.Add(btnPausa);
            panelReproductor.Dock = DockStyle.Bottom;
            panelReproductor.Location = new Point(0, 718);
            panelReproductor.Name = "panelReproductor";
            panelReproductor.Size = new Size(1222, 60);
            panelReproductor.TabIndex = 1;
            // 
            // lblReproduciendo
            // 
            lblReproduciendo.AutoSize = true;
            lblReproduciendo.Location = new Point(10, 10);
            lblReproduciendo.Name = "lblReproduciendo";
            lblReproduciendo.Size = new Size(91, 15);
            lblReproduciendo.TabIndex = 0;
            lblReproduciendo.Text = "Reproduciendo:";
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(10, 30);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(75, 23);
            btnPlay.TabIndex = 1;
            btnPlay.Text = "Play";
            btnPlay.Click += btnPlay_Click;
            // 
            // btnPausa
            // 
            btnPausa.Location = new Point(90, 30);
            btnPausa.Name = "btnPausa";
            btnPausa.Size = new Size(75, 23);
            btnPausa.TabIndex = 2;
            btnPausa.Text = "Pausa";
            btnPausa.Click += btnPausa_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(menuStrip1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(180, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1042, 30);
            panel1.TabIndex = 4;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { archivoToolStripMenuItem, ayudaToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1042, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { configuraciónToolStripMenuItem });
            archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            archivoToolStripMenuItem.Size = new Size(60, 20);
            archivoToolStripMenuItem.Text = "&Archivo";
            // 
            // configuraciónToolStripMenuItem
            // 
            configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            configuraciónToolStripMenuItem.Size = new Size(150, 22);
            configuraciónToolStripMenuItem.Text = "&Configuración";
            configuraciónToolStripMenuItem.Click += configuraciónToolStripMenuItem_Click;
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { consolaDeDepuraciónToolStripMenuItem });
            ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            ayudaToolStripMenuItem.Size = new Size(53, 20);
            ayudaToolStripMenuItem.Text = "A&yuda";
            // 
            // consolaDeDepuraciónToolStripMenuItem
            // 
            consolaDeDepuraciónToolStripMenuItem.Name = "consolaDeDepuraciónToolStripMenuItem";
            consolaDeDepuraciónToolStripMenuItem.Size = new Size(196, 22);
            consolaDeDepuraciónToolStripMenuItem.Text = "&Consola de depuración";
            consolaDeDepuraciónToolStripMenuItem.Click += consolaDeDepuraciónToolStripMenuItem_Click;
            // 
            // panelContenido
            // 
            panelContenido.Controls.Add(dgvCanciones);
            panelContenido.Controls.Add(txtBuscar);
            panelContenido.Controls.Add(statusStrip1);
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(180, 30);
            panelContenido.Name = "panelContenido";
            panelContenido.Padding = new Padding(10);
            panelContenido.Size = new Size(1042, 688);
            panelContenido.TabIndex = 5;
            // 
            // dgvCanciones
            // 
            dgvCanciones.AllowUserToAddRows = false;
            dgvCanciones.Columns.AddRange(new DataGridViewColumn[] { colTitulo, colArtista, colAlbum, colGenero, colAnno, colRuta });
            dgvCanciones.Dock = DockStyle.Fill;
            dgvCanciones.Location = new Point(10, 33);
            dgvCanciones.Name = "dgvCanciones";
            dgvCanciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCanciones.Size = new Size(1022, 623);
            dgvCanciones.TabIndex = 0;
            dgvCanciones.CellDoubleClick += dgvCanciones_CellDoubleClick;
            // 
            // colTitulo
            // 
            colTitulo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colTitulo.HeaderText = "Título";
            colTitulo.Name = "colTitulo";
            colTitulo.Width = 300;
            // 
            // colArtista
            // 
            colArtista.HeaderText = "Artista";
            colArtista.Name = "colArtista";
            colArtista.Width = 250;
            // 
            // colAlbum
            // 
            colAlbum.FillWeight = 200F;
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
            // 
            // txtBuscar
            // 
            txtBuscar.Dock = DockStyle.Top;
            txtBuscar.Location = new Point(10, 10);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(1022, 23);
            txtBuscar.TabIndex = 1;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lbConexionDB, lblTotalArtistas, lblTotalAlbumes, lblTotalGeneros, lblDuracionTotal, lblTotalCanciones });
            statusStrip1.Location = new Point(10, 656);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1022, 22);
            statusStrip1.TabIndex = 3;
            // 
            // lbConexionDB
            // 
            lbConexionDB.AutoSize = false;
            lbConexionDB.Margin = new Padding(0, 3, 50, 2);
            lbConexionDB.Name = "lbConexionDB";
            lbConexionDB.Size = new Size(300, 17);
            lbConexionDB.Text = "No está conectado a la base de datos";
            lbConexionDB.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalArtistas
            // 
            lblTotalArtistas.Name = "lblTotalArtistas";
            lblTotalArtistas.Size = new Size(58, 17);
            lblTotalArtistas.Text = "Artistas: 0";
            // 
            // lblTotalAlbumes
            // 
            lblTotalAlbumes.Name = "lblTotalAlbumes";
            lblTotalAlbumes.Size = new Size(66, 17);
            lblTotalAlbumes.Text = "Álbumes: 0";
            // 
            // lblTotalGeneros
            // 
            lblTotalGeneros.Name = "lblTotalGeneros";
            lblTotalGeneros.Size = new Size(62, 17);
            lblTotalGeneros.Text = "Géneros: 0";
            // 
            // lblDuracionTotal
            // 
            lblDuracionTotal.Name = "lblDuracionTotal";
            lblDuracionTotal.Size = new Size(118, 17);
            lblDuracionTotal.Text = "Duración total: 0 min";
            // 
            // lblTotalCanciones
            // 
            lblTotalCanciones.Name = "lblTotalCanciones";
            lblTotalCanciones.Size = new Size(74, 17);
            lblTotalCanciones.Text = "Canciones: 0";
            // 
            // panelLateral
            // 
            panelLateral.BackColor = Color.LightGray;
            panelLateral.Controls.Add(btnSincronizar);
            panelLateral.Controls.Add(btnOrganizar);
            panelLateral.Controls.Add(btnDescargarMetadatos);
            panelLateral.Controls.Add(btnSeleccionarCarpeta);
            panelLateral.Dock = DockStyle.Left;
            panelLateral.Location = new Point(0, 0);
            panelLateral.Name = "panelLateral";
            panelLateral.Padding = new Padding(10);
            panelLateral.Size = new Size(180, 718);
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
            // btnSincronizar
            // 
            btnSincronizar.Dock = DockStyle.Top;
            btnSincronizar.Location = new Point(10, 130);
            btnSincronizar.Name = "btnSincronizar";
            btnSincronizar.Size = new Size(160, 40);
            btnSincronizar.TabIndex = 3;
            btnSincronizar.Text = "Sincronizar BD";
            btnSincronizar.Click += btnSincronizar_Click;
            // 
            // FrmMain
            // 
            ClientSize = new Size(1222, 778);
            Controls.Add(panelContenido);
            Controls.Add(panel1);
            Controls.Add(panelLateral);
            Controls.Add(panelReproductor);
            MainMenuStrip = menuStrip1;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MusicManager";
            Load += FrmMain_Load;
            panelReproductor.ResumeLayout(false);
            panelReproductor.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panelContenido.ResumeLayout(false);
            panelContenido.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panelLateral.ResumeLayout(false);
            ResumeLayout(false);
        }
        private Panel panel1;
        private Panel panelContenido;
        private DataGridView dgvCanciones;
        private TextBox txtBuscar;
        private Panel panelLateral;
        private Button btnOrganizar;
        private Button btnDescargarMetadatos;
        private Button btnSeleccionarCarpeta;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblTotalArtistas;
        private ToolStripStatusLabel lblTotalAlbumes;
        private ToolStripStatusLabel lblTotalGeneros;
        private ToolStripStatusLabel lblDuracionTotal;
        private ToolStripStatusLabel lblTotalCanciones;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem ayudaToolStripMenuItem;
        private ToolStripStatusLabel lbConexionDB;
        private ToolStripMenuItem configuraciónToolStripMenuItem;
        private ToolStripMenuItem consolaDeDepuraciónToolStripMenuItem;
        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colArtista;
        private DataGridViewTextBoxColumn colAlbum;
        private DataGridViewTextBoxColumn colGenero;
        private DataGridViewTextBoxColumn colAnno;
        private DataGridViewTextBoxColumn colRuta;
        private Button btnSincronizar;
    }
}
