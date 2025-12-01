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

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblTotalArtistas;
        private ToolStripStatusLabel lblTotalAlbumes;
        private ToolStripStatusLabel lblTotalGeneros;
        private ToolStripStatusLabel lblDuracionTotal;
        private ToolStripStatusLabel lblTotalCanciones;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            panelLateral = new Panel();
            btnOrganizar = new Button();
            btnDescargarMetadatos = new Button();
            btnSeleccionarCarpeta = new Button();

            panelContenido = new Panel();
            txtBuscar = new TextBox();
            panelEstadisticas = new Panel();
            dgvCanciones = new DataGridView();
            statusStrip1 = new StatusStrip();

            lblTotalArtistas = new ToolStripStatusLabel();
            lblTotalAlbumes = new ToolStripStatusLabel();
            lblTotalGeneros = new ToolStripStatusLabel();
            lblDuracionTotal = new ToolStripStatusLabel();
            lblTotalCanciones = new ToolStripStatusLabel();

            panelReproductor = new Panel();
            btnPlay = new Button();
            btnPausa = new Button();
            lblReproduciendo = new Label();

            // ─────────────────────────────────────────────────
            // COLUMNAS: CORRECCIÓN IMPORTANTE ⬇⬇⬇
            // ─────────────────────────────────────────────────
            colTitulo = new DataGridViewTextBoxColumn();
            colTitulo.HeaderText = "Título";
            colTitulo.Name = "colTitulo";

            colArtista = new DataGridViewTextBoxColumn();
            colArtista.HeaderText = "Artista";
            colArtista.Name = "colArtista";

            colAlbum = new DataGridViewTextBoxColumn();
            colAlbum.HeaderText = "Álbum";
            colAlbum.Name = "colAlbum";

            colGenero = new DataGridViewTextBoxColumn();
            colGenero.HeaderText = "Género";
            colGenero.Name = "colGenero";

            colAnno = new DataGridViewTextBoxColumn();
            colAnno.HeaderText = "Año";
            colAnno.Name = "colAnno";

            colRuta = new DataGridViewTextBoxColumn();
            colRuta.HeaderText = "Ruta";
            colRuta.Name = "colRuta";
            colRuta.Visible = false;

            SuspendLayout();

            // PANEL LATERAL
            panelLateral.BackColor = Color.LightGray;
            panelLateral.Dock = DockStyle.Left;
            panelLateral.Padding = new Padding(10);
            panelLateral.Width = 180;

            btnSeleccionarCarpeta.Text = "Seleccionar Carpeta";
            btnSeleccionarCarpeta.Dock = DockStyle.Top;
            btnSeleccionarCarpeta.Height = 40;
            btnSeleccionarCarpeta.Click += btnSeleccionarCarpeta_Click;

            btnDescargarMetadatos.Text = "Descargar Metadatos";
            btnDescargarMetadatos.Dock = DockStyle.Top;
            btnDescargarMetadatos.Height = 40;
            btnDescargarMetadatos.Click += btnDescargarMetadatos_Click;

            btnOrganizar.Text = "Organizar Música";
            btnOrganizar.Dock = DockStyle.Top;
            btnOrganizar.Height = 40;
            btnOrganizar.Click += btnOrganizar_Click;

            panelLateral.Controls.Add(btnOrganizar);
            panelLateral.Controls.Add(btnDescargarMetadatos);
            panelLateral.Controls.Add(btnSeleccionarCarpeta);

            // PANEL CONTENIDO
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Padding = new Padding(10);

            txtBuscar.Dock = DockStyle.Top;
            txtBuscar.Height = 25;
            txtBuscar.TextChanged += txtBuscar_TextChanged;

            panelEstadisticas.Dock = DockStyle.Top;
            panelEstadisticas.Height = 30;
            panelEstadisticas.BackColor = Color.WhiteSmoke;

            // DATA GRID VIEW
            dgvCanciones.Dock = DockStyle.Fill;
            dgvCanciones.AllowUserToAddRows = false;
            dgvCanciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCanciones.CellDoubleClick += dgvCanciones_CellDoubleClick;

            // AQUI AÑADIMOS LAS COLUMNAS CORRECTAMENTE
            dgvCanciones.Columns.AddRange(
                colTitulo, colArtista, colAlbum, colGenero, colAnno, colRuta
            );

            // STATUS STRIP
            statusStrip1.Dock = DockStyle.Bottom;
            statusStrip1.Items.AddRange(new ToolStripItem[]
            {
                lblTotalArtistas,
                lblTotalAlbumes,
                lblTotalGeneros,
                lblDuracionTotal,
                lblTotalCanciones
            });

            lblTotalArtistas.Text = "Artistas: 0";
            lblTotalAlbumes.Text = "Álbumes: 0";
            lblTotalGeneros.Text = "Géneros: 0";
            lblDuracionTotal.Text = "Duración total: 0 min";
            lblTotalCanciones.Text = "Canciones: 0";

            panelContenido.Controls.Add(dgvCanciones);
            panelContenido.Controls.Add(txtBuscar);
            panelContenido.Controls.Add(panelEstadisticas);
            panelContenido.Controls.Add(statusStrip1);

            // PANEL REPRODUCTOR
            panelReproductor.BackColor = Color.Silver;
            panelReproductor.Dock = DockStyle.Bottom;
            panelReproductor.Height = 60;

            lblReproduciendo.Text = "Reproduciendo:";
            lblReproduciendo.Location = new Point(10, 10);
            lblReproduciendo.AutoSize = true;

            btnPlay.Text = "Play";
            btnPlay.Location = new Point(10, 30);
            btnPlay.Click += btnPlay_Click;

            btnPausa.Text = "Pausa";
            btnPausa.Location = new Point(90, 30);
            btnPausa.Click += btnPausa_Click;

            panelReproductor.Controls.Add(lblReproduciendo);
            panelReproductor.Controls.Add(btnPlay);
            panelReproductor.Controls.Add(btnPausa);

            // FORM SETTINGS
            ClientSize = new Size(1100, 650);
            Controls.Add(panelContenido);
            Controls.Add(panelReproductor);
            Controls.Add(panelLateral);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MusicManager";
            Load += FrmMain_Load;

            ResumeLayout(false);
        }
    }
}
