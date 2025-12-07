using MusicManager.Modelos;

namespace MusicManager
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnReproductor;
        private Button btnPlay;
        private Button btnPausa;
        private Label lblReproduciendo;
        private Button btnStop;
        private TrackBar trackProgreso;
        private TrackBar trackVolumen;
        private Label lblTiempo;
        private Panel pnMenuPrincipal;
        private Panel pnContenido;
        private DataGridView dgvCanciones;
        private Panel pnLateral;
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
        private Button btnEditarMetadatos;
        private ToolStripMenuItem configuraciónAPIToolStripMenuItem;
        private ToolStripMenuItem salirDelProgramaToolStripMenuItem;
        private ToolStripMenuItem acercaDeToolStripMenuItem;
        private Label lblVolumen;
        private Panel pnHerramientas;
        private TextBox txtBuscarCancion;
        private Button btnLimpiarBusqueda;
        private Panel pnStatusBar;
        private PictureBox pictureBox1;
        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colArtista;
        private DataGridViewTextBoxColumn colAlbum;
        private DataGridViewTextBoxColumn colGenero;
        private DataGridViewTextBoxColumn colAnno;
        private DataGridViewTextBoxColumn colRuta;
        private Button btnSincronizaTodo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            pnReproductor = new Panel();
            lblVolumen = new Label();
            lblTiempo = new Label();
            trackProgreso = new TrackBar();
            trackVolumen = new TrackBar();
            btnStop = new Button();
            lblReproduciendo = new Label();
            btnPlay = new Button();
            btnPausa = new Button();
            pnMenuPrincipal = new Panel();
            menuStrip1 = new MenuStrip();
            archivoToolStripMenuItem = new ToolStripMenuItem();
            configuraciónToolStripMenuItem = new ToolStripMenuItem();
            configuraciónAPIToolStripMenuItem = new ToolStripMenuItem();
            salirDelProgramaToolStripMenuItem = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            consolaDeDepuraciónToolStripMenuItem = new ToolStripMenuItem();
            acercaDeToolStripMenuItem = new ToolStripMenuItem();
            pnContenido = new Panel();
            dgvCanciones = new DataGridView();
            colTitulo = new DataGridViewTextBoxColumn();
            colArtista = new DataGridViewTextBoxColumn();
            colAlbum = new DataGridViewTextBoxColumn();
            colGenero = new DataGridViewTextBoxColumn();
            colAnno = new DataGridViewTextBoxColumn();
            colRuta = new DataGridViewTextBoxColumn();
            statusStrip1 = new StatusStrip();
            lbConexionDB = new ToolStripStatusLabel();
            lblTotalArtistas = new ToolStripStatusLabel();
            lblTotalAlbumes = new ToolStripStatusLabel();
            lblTotalGeneros = new ToolStripStatusLabel();
            lblDuracionTotal = new ToolStripStatusLabel();
            lblTotalCanciones = new ToolStripStatusLabel();
            pnHerramientas = new Panel();
            progressBarSync = new ProgressBar();
            txtBuscarCancion = new TextBox();
            btnLimpiarBusqueda = new Button();
            pnLateral = new Panel();
            pbPortada = new PictureBox();
            btnSincronizaTodo = new Button();
            pictureBox1 = new PictureBox();
            btnOrganizar = new Button();
            btnDescargarMetadatos = new Button();
            btnEditarMetadatos = new Button();
            btnSeleccionarCarpeta = new Button();
            pnStatusBar = new Panel();
            toolTip1 = new ToolTip(components);
            pnReproductor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackProgreso).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackVolumen).BeginInit();
            pnMenuPrincipal.SuspendLayout();
            menuStrip1.SuspendLayout();
            pnContenido.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).BeginInit();
            statusStrip1.SuspendLayout();
            pnHerramientas.SuspendLayout();
            pnLateral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbPortada).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnStatusBar.SuspendLayout();
            SuspendLayout();
            // 
            // pnReproductor
            // 
            pnReproductor.BackColor = SystemColors.Control;
            pnReproductor.Controls.Add(lblVolumen);
            pnReproductor.Controls.Add(lblTiempo);
            pnReproductor.Controls.Add(trackProgreso);
            pnReproductor.Controls.Add(trackVolumen);
            pnReproductor.Controls.Add(btnStop);
            pnReproductor.Controls.Add(lblReproduciendo);
            pnReproductor.Controls.Add(btnPlay);
            pnReproductor.Controls.Add(btnPausa);
            pnReproductor.Dock = DockStyle.Bottom;
            pnReproductor.Location = new Point(0, 621);
            pnReproductor.Name = "pnReproductor";
            pnReproductor.Size = new Size(1114, 60);
            pnReproductor.TabIndex = 1;
            // 
            // lblVolumen
            // 
            lblVolumen.Anchor = AnchorStyles.Right;
            lblVolumen.AutoSize = true;
            lblVolumen.Location = new Point(947, 32);
            lblVolumen.Name = "lblVolumen";
            lblVolumen.Size = new Size(57, 15);
            lblVolumen.TabIndex = 7;
            lblVolumen.Text = "Volumen:";
            // 
            // lblTiempo
            // 
            lblTiempo.Anchor = AnchorStyles.Right;
            lblTiempo.Location = new Point(829, 32);
            lblTiempo.Name = "lblTiempo";
            lblTiempo.Size = new Size(93, 19);
            lblTiempo.TabIndex = 0;
            lblTiempo.Text = "00:00 / 00:00";
            // 
            // trackProgreso
            // 
            trackProgreso.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackProgreso.AutoSize = false;
            trackProgreso.Cursor = Cursors.Hand;
            trackProgreso.Location = new Point(206, 29);
            trackProgreso.Name = "trackProgreso";
            trackProgreso.Size = new Size(617, 25);
            trackProgreso.TabIndex = 1;
            trackProgreso.TickStyle = TickStyle.None;
            trackProgreso.MouseDown += trackProgreso_MouseDown;
            trackProgreso.MouseUp += trackProgreso_MouseUp;
            // 
            // trackVolumen
            // 
            trackVolumen.Anchor = AnchorStyles.Right;
            trackVolumen.AutoSize = false;
            trackVolumen.Cursor = Cursors.Hand;
            trackVolumen.Location = new Point(1001, 29);
            trackVolumen.Maximum = 100;
            trackVolumen.Name = "trackVolumen";
            trackVolumen.Size = new Size(101, 25);
            trackVolumen.TabIndex = 2;
            trackVolumen.TickStyle = TickStyle.None;
            trackVolumen.Value = 100;
            trackVolumen.ValueChanged += trackVolumen_ValueChanged;
            // 
            // btnStop
            // 
            btnStop.Anchor = AnchorStyles.Left;
            btnStop.Location = new Point(140, 30);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(60, 23);
            btnStop.TabIndex = 3;
            btnStop.Text = "Stop";
            btnStop.Click += btnStop_Click;
            // 
            // lblReproduciendo
            // 
            lblReproduciendo.Anchor = AnchorStyles.Left;
            lblReproduciendo.AutoSize = true;
            lblReproduciendo.Location = new Point(10, 10);
            lblReproduciendo.Name = "lblReproduciendo";
            lblReproduciendo.Size = new Size(146, 15);
            lblReproduciendo.TabIndex = 4;
            lblReproduciendo.Text = "Reproduciendo: (ninguna)";
            // 
            // btnPlay
            // 
            btnPlay.Anchor = AnchorStyles.Left;
            btnPlay.Location = new Point(10, 30);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(60, 23);
            btnPlay.TabIndex = 5;
            btnPlay.Text = "Play";
            btnPlay.Click += btnPlay_Click;
            // 
            // btnPausa
            // 
            btnPausa.Anchor = AnchorStyles.Left;
            btnPausa.Location = new Point(75, 30);
            btnPausa.Name = "btnPausa";
            btnPausa.Size = new Size(60, 23);
            btnPausa.TabIndex = 6;
            btnPausa.Text = "Pausa";
            btnPausa.Click += btnPausa_Click;
            // 
            // pnMenuPrincipal
            // 
            pnMenuPrincipal.Controls.Add(menuStrip1);
            pnMenuPrincipal.Dock = DockStyle.Top;
            pnMenuPrincipal.Location = new Point(0, 0);
            pnMenuPrincipal.Name = "pnMenuPrincipal";
            pnMenuPrincipal.Size = new Size(1114, 30);
            pnMenuPrincipal.TabIndex = 4;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { archivoToolStripMenuItem, ayudaToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1114, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { configuraciónToolStripMenuItem, configuraciónAPIToolStripMenuItem, salirDelProgramaToolStripMenuItem });
            archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            archivoToolStripMenuItem.Size = new Size(60, 20);
            archivoToolStripMenuItem.Text = "&Archivo";
            // 
            // configuraciónToolStripMenuItem
            // 
            configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            configuraciónToolStripMenuItem.Size = new Size(170, 22);
            configuraciónToolStripMenuItem.Text = "&Conexión BD";
            configuraciónToolStripMenuItem.Click += configuraciónToolStripMenuItem_Click;
            // 
            // configuraciónAPIToolStripMenuItem
            // 
            configuraciónAPIToolStripMenuItem.Name = "configuraciónAPIToolStripMenuItem";
            configuraciónAPIToolStripMenuItem.Size = new Size(170, 22);
            configuraciónAPIToolStripMenuItem.Text = "&Estado de la API";
            configuraciónAPIToolStripMenuItem.Click += configuraciónAPIToolStripMenuItem_Click;
            // 
            // salirDelProgramaToolStripMenuItem
            // 
            salirDelProgramaToolStripMenuItem.Name = "salirDelProgramaToolStripMenuItem";
            salirDelProgramaToolStripMenuItem.Size = new Size(170, 22);
            salirDelProgramaToolStripMenuItem.Text = "&Salir del programa";
            salirDelProgramaToolStripMenuItem.Click += salirDelProgramaToolStripMenuItem_Click;
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { consolaDeDepuraciónToolStripMenuItem, acercaDeToolStripMenuItem });
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
            // acercaDeToolStripMenuItem
            // 
            acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            acercaDeToolStripMenuItem.Size = new Size(196, 22);
            acercaDeToolStripMenuItem.Text = "&Acerca de...";
            acercaDeToolStripMenuItem.Click += acercaDeToolStripMenuItem_Click;
            // 
            // pnContenido
            // 
            pnContenido.Controls.Add(dgvCanciones);
            pnContenido.Dock = DockStyle.Fill;
            pnContenido.Location = new Point(180, 60);
            pnContenido.Name = "pnContenido";
            pnContenido.Padding = new Padding(10);
            pnContenido.Size = new Size(934, 531);
            pnContenido.TabIndex = 5;
            // 
            // dgvCanciones
            // 
            dgvCanciones.AllowUserToAddRows = false;
            dgvCanciones.AllowUserToDeleteRows = false;
            dgvCanciones.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(192, 251, 180);
            dgvCanciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCanciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCanciones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCanciones.ColumnHeadersHeight = 40;
            dgvCanciones.Columns.AddRange(new DataGridViewColumn[] { colTitulo, colArtista, colAlbum, colGenero, colAnno, colRuta });
            dgvCanciones.Dock = DockStyle.Fill;
            dgvCanciones.Location = new Point(10, 10);
            dgvCanciones.MultiSelect = false;
            dgvCanciones.Name = "dgvCanciones";
            dgvCanciones.ReadOnly = true;
            dgvCanciones.RowHeadersVisible = false;
            dgvCanciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCanciones.Size = new Size(914, 511);
            dgvCanciones.TabIndex = 0;
            dgvCanciones.CellDoubleClick += dgvCanciones_CellDoubleClick;
            dgvCanciones.SelectionChanged += dgvCanciones_SelectionChanged;
            // 
            // colTitulo
            // 
            colTitulo.FillWeight = 58.3416634F;
            colTitulo.HeaderText = "Título";
            colTitulo.Name = "colTitulo";
            colTitulo.ReadOnly = true;
            // 
            // colArtista
            // 
            colArtista.FillWeight = 58.3416634F;
            colArtista.HeaderText = "Artista";
            colArtista.Name = "colArtista";
            colArtista.ReadOnly = true;
            // 
            // colAlbum
            // 
            colAlbum.FillWeight = 43.75625F;
            colAlbum.HeaderText = "Álbum";
            colAlbum.Name = "colAlbum";
            colAlbum.ReadOnly = true;
            // 
            // colGenero
            // 
            colGenero.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colGenero.HeaderText = "Género";
            colGenero.Name = "colGenero";
            colGenero.ReadOnly = true;
            colGenero.Width = 120;
            // 
            // colAnno
            // 
            colAnno.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colAnno.HeaderText = "Año";
            colAnno.Name = "colAnno";
            colAnno.ReadOnly = true;
            colAnno.Width = 80;
            // 
            // colRuta
            // 
            colRuta.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRuta.HeaderText = "Ruta";
            colRuta.Name = "colRuta";
            colRuta.ReadOnly = true;
            // 
            // statusStrip1
            // 
            statusStrip1.Dock = DockStyle.Fill;
            statusStrip1.Items.AddRange(new ToolStripItem[] { lbConexionDB, lblTotalArtistas, lblTotalAlbumes, lblTotalGeneros, lblDuracionTotal, lblTotalCanciones });
            statusStrip1.Location = new Point(0, 0);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(934, 30);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 3;
            // 
            // lbConexionDB
            // 
            lbConexionDB.AutoSize = false;
            lbConexionDB.Margin = new Padding(0, 3, 50, 2);
            lbConexionDB.Name = "lbConexionDB";
            lbConexionDB.Size = new Size(220, 25);
            lbConexionDB.Text = "No está conectado a la base de datos";
            lbConexionDB.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalArtistas
            // 
            lblTotalArtistas.Name = "lblTotalArtistas";
            lblTotalArtistas.Size = new Size(329, 25);
            lblTotalArtistas.Spring = true;
            lblTotalArtistas.Text = "Artistas: 0";
            lblTotalArtistas.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalAlbumes
            // 
            lblTotalAlbumes.Name = "lblTotalAlbumes";
            lblTotalAlbumes.Size = new Size(66, 25);
            lblTotalAlbumes.Text = "Álbumes: 0";
            // 
            // lblTotalGeneros
            // 
            lblTotalGeneros.Name = "lblTotalGeneros";
            lblTotalGeneros.Size = new Size(62, 25);
            lblTotalGeneros.Text = "Géneros: 0";
            // 
            // lblDuracionTotal
            // 
            lblDuracionTotal.Name = "lblDuracionTotal";
            lblDuracionTotal.Size = new Size(118, 25);
            lblDuracionTotal.Text = "Duración total: 0 min";
            // 
            // lblTotalCanciones
            // 
            lblTotalCanciones.Name = "lblTotalCanciones";
            lblTotalCanciones.Size = new Size(74, 25);
            lblTotalCanciones.Text = "Canciones: 0";
            // 
            // pnHerramientas
            // 
            pnHerramientas.BackColor = SystemColors.Control;
            pnHerramientas.Controls.Add(progressBarSync);
            pnHerramientas.Controls.Add(txtBuscarCancion);
            pnHerramientas.Controls.Add(btnLimpiarBusqueda);
            pnHerramientas.Dock = DockStyle.Top;
            pnHerramientas.Location = new Point(180, 30);
            pnHerramientas.Name = "pnHerramientas";
            pnHerramientas.Padding = new Padding(10, 5, 10, 5);
            pnHerramientas.Size = new Size(934, 30);
            pnHerramientas.TabIndex = 5;
            // 
            // progressBarSync
            // 
            progressBarSync.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            progressBarSync.Location = new Point(672, 5);
            progressBarSync.Name = "progressBarSync";
            progressBarSync.Size = new Size(250, 20);
            progressBarSync.TabIndex = 3;
            // 
            // txtBuscarCancion
            // 
            txtBuscarCancion.Location = new Point(10, 4);
            txtBuscarCancion.Name = "txtBuscarCancion";
            txtBuscarCancion.PlaceholderText = "Buscar canción, artista, álbum, género, año...";
            txtBuscarCancion.Size = new Size(488, 23);
            txtBuscarCancion.TabIndex = 1;
            txtBuscarCancion.TextChanged += txtBuscarCancion_TextChanged;
            // 
            // btnLimpiarBusqueda
            // 
            btnLimpiarBusqueda.Location = new Point(504, 4);
            btnLimpiarBusqueda.Name = "btnLimpiarBusqueda";
            btnLimpiarBusqueda.Size = new Size(30, 23);
            btnLimpiarBusqueda.TabIndex = 2;
            btnLimpiarBusqueda.Text = "✖";
            btnLimpiarBusqueda.Click += btnLimpiarBusqueda_Click;
            // 
            // pnLateral
            // 
            pnLateral.BackColor = SystemColors.Control;
            pnLateral.Controls.Add(pbPortada);
            pnLateral.Controls.Add(btnSincronizaTodo);
            pnLateral.Controls.Add(pictureBox1);
            pnLateral.Controls.Add(btnOrganizar);
            pnLateral.Controls.Add(btnDescargarMetadatos);
            pnLateral.Controls.Add(btnEditarMetadatos);
            pnLateral.Controls.Add(btnSeleccionarCarpeta);
            pnLateral.Dock = DockStyle.Left;
            pnLateral.Location = new Point(0, 30);
            pnLateral.Name = "pnLateral";
            pnLateral.Padding = new Padding(10);
            pnLateral.Size = new Size(180, 591);
            pnLateral.TabIndex = 2;
            // 
            // pbPortada
            // 
            pbPortada.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pbPortada.Cursor = Cursors.Hand;
            pbPortada.Location = new Point(10, 224);
            pbPortada.Name = "pbPortada";
            pbPortada.Size = new Size(170, 170);
            pbPortada.SizeMode = PictureBoxSizeMode.StretchImage;
            pbPortada.TabIndex = 8;
            pbPortada.TabStop = false;
            toolTip1.SetToolTip(pbPortada, "Haga clic para ver la portada mas grande.");
            pbPortada.Click += pbPortada_Click;
            // 
            // btnSincronizaTodo
            // 
            btnSincronizaTodo.Dock = DockStyle.Top;
            btnSincronizaTodo.Location = new Point(10, 170);
            btnSincronizaTodo.Name = "btnSincronizaTodo";
            btnSincronizaTodo.Size = new Size(160, 40);
            btnSincronizaTodo.TabIndex = 7;
            btnSincronizaTodo.Text = "Sincronizar Todo";
            btnSincronizaTodo.Click += btnSincronizaTodo_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(10, 412);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(170, 170);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // btnOrganizar
            // 
            btnOrganizar.Dock = DockStyle.Top;
            btnOrganizar.Location = new Point(10, 130);
            btnOrganizar.Name = "btnOrganizar";
            btnOrganizar.Size = new Size(160, 40);
            btnOrganizar.TabIndex = 0;
            btnOrganizar.Text = "Organizar Música";
            btnOrganizar.Click += btnOrganizar_Click;
            // 
            // btnDescargarMetadatos
            // 
            btnDescargarMetadatos.Dock = DockStyle.Top;
            btnDescargarMetadatos.Location = new Point(10, 90);
            btnDescargarMetadatos.Name = "btnDescargarMetadatos";
            btnDescargarMetadatos.Size = new Size(160, 40);
            btnDescargarMetadatos.TabIndex = 1;
            btnDescargarMetadatos.Text = "Descargar Metadatos";
            btnDescargarMetadatos.Click += btnDescargarMetadatos_Click;
            // 
            // btnEditarMetadatos
            // 
            btnEditarMetadatos.Dock = DockStyle.Top;
            btnEditarMetadatos.Location = new Point(10, 50);
            btnEditarMetadatos.Name = "btnEditarMetadatos";
            btnEditarMetadatos.Size = new Size(160, 40);
            btnEditarMetadatos.TabIndex = 4;
            btnEditarMetadatos.Text = "Editar Metadatos";
            btnEditarMetadatos.Click += btnEditarMetadatos_Click;
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
            // pnStatusBar
            // 
            pnStatusBar.Controls.Add(statusStrip1);
            pnStatusBar.Dock = DockStyle.Bottom;
            pnStatusBar.Location = new Point(180, 591);
            pnStatusBar.Name = "pnStatusBar";
            pnStatusBar.Size = new Size(934, 30);
            pnStatusBar.TabIndex = 6;
            // 
            // FrmMain
            // 
            ClientSize = new Size(1114, 681);
            Controls.Add(pnContenido);
            Controls.Add(pnStatusBar);
            Controls.Add(pnHerramientas);
            Controls.Add(pnLateral);
            Controls.Add(pnReproductor);
            Controls.Add(pnMenuPrincipal);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(1130, 720);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MusicManager";
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            pnReproductor.ResumeLayout(false);
            pnReproductor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackProgreso).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackVolumen).EndInit();
            pnMenuPrincipal.ResumeLayout(false);
            pnMenuPrincipal.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            pnContenido.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCanciones).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            pnHerramientas.ResumeLayout(false);
            pnHerramientas.PerformLayout();
            pnLateral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbPortada).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnStatusBar.ResumeLayout(false);
            pnStatusBar.PerformLayout();
            ResumeLayout(false);
        }
        private ProgressBar progressBarSync;
        private PictureBox pbPortada;
        private ToolTip toolTip1;
    }
}
