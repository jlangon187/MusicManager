namespace MusicManager
{
    partial class FrmSeleccionarMetadatos
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvResultados;
        private Button btnAceptar;
        private Button btnCancelar;
        private TextBox txtBusqueda;
        private Button btnBuscar;

        private CheckBox chkSpotify;
        private CheckBox chkITunes;

        private Panel pnGrid;
        private Panel pnControles;
        private Panel pnOpciones;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSeleccionarMetadatos));
            dgvResultados = new DataGridView();
            btnAceptar = new Button();
            btnCancelar = new Button();
            txtBusqueda = new TextBox();
            btnBuscar = new Button();
            chkSpotify = new CheckBox();
            chkITunes = new CheckBox();
            pnOpciones = new Panel();
            pnGrid = new Panel();
            pnControles = new Panel();
            pnStatus = new Panel();
            StatusStrip = new StatusStrip();
            tsStatusLabel = new ToolStripStatusLabel();
            colTitulo = new DataGridViewTextBoxColumn();
            colArtista = new DataGridViewTextBoxColumn();
            colAlbum = new DataGridViewTextBoxColumn();
            colGenero = new DataGridViewTextBoxColumn();
            colAnio = new DataGridViewTextBoxColumn();
            colFuente = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvResultados).BeginInit();
            pnOpciones.SuspendLayout();
            pnGrid.SuspendLayout();
            pnControles.SuspendLayout();
            pnStatus.SuspendLayout();
            StatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // dgvResultados
            // 
            dgvResultados.AllowUserToAddRows = false;
            dgvResultados.AllowUserToDeleteRows = false;
            dgvResultados.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvResultados.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvResultados.ColumnHeadersHeight = 30;
            dgvResultados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvResultados.Columns.AddRange(new DataGridViewColumn[] { colTitulo, colArtista, colAlbum, colGenero, colAnio, colFuente });
            dgvResultados.Dock = DockStyle.Fill;
            dgvResultados.Location = new Point(0, 0);
            dgvResultados.MultiSelect = false;
            dgvResultados.Name = "dgvResultados";
            dgvResultados.ReadOnly = true;
            dgvResultados.RowHeadersVisible = false;
            dgvResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResultados.Size = new Size(864, 419);
            dgvResultados.TabIndex = 0;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(285, 13);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(120, 23);
            btnAceptar.TabIndex = 0;
            btnAceptar.Text = "Aceptar";
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(479, 13);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 23);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;
            // 
            // txtBusqueda
            // 
            txtBusqueda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBusqueda.Location = new Point(5, 5);
            txtBusqueda.Name = "txtBusqueda";
            txtBusqueda.PlaceholderText = "Buscar manualmente...";
            txtBusqueda.Size = new Size(565, 23);
            txtBusqueda.TabIndex = 0;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(576, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(70, 27);
            btnBuscar.TabIndex = 1;
            btnBuscar.Text = "Buscar";
            btnBuscar.Click += btnBuscar_Click;
            // 
            // chkSpotify
            // 
            chkSpotify.Checked = true;
            chkSpotify.CheckState = CheckState.Checked;
            chkSpotify.Location = new Point(659, 6);
            chkSpotify.Name = "chkSpotify";
            chkSpotify.Size = new Size(104, 24);
            chkSpotify.TabIndex = 2;
            chkSpotify.Text = "Usar Spotify";
            // 
            // chkITunes
            // 
            chkITunes.Checked = true;
            chkITunes.CheckState = CheckState.Checked;
            chkITunes.Location = new Point(769, 6);
            chkITunes.Name = "chkITunes";
            chkITunes.Size = new Size(92, 24);
            chkITunes.TabIndex = 3;
            chkITunes.Text = "Usar iTunes";
            // 
            // pnOpciones
            // 
            pnOpciones.Controls.Add(chkSpotify);
            pnOpciones.Controls.Add(btnBuscar);
            pnOpciones.Controls.Add(chkITunes);
            pnOpciones.Controls.Add(txtBusqueda);
            pnOpciones.Dock = DockStyle.Top;
            pnOpciones.Location = new Point(0, 0);
            pnOpciones.Name = "pnOpciones";
            pnOpciones.Size = new Size(864, 32);
            pnOpciones.TabIndex = 0;
            // 
            // pnGrid
            // 
            pnGrid.Controls.Add(dgvResultados);
            pnGrid.Dock = DockStyle.Fill;
            pnGrid.Location = new Point(0, 32);
            pnGrid.Name = "pnGrid";
            pnGrid.Size = new Size(864, 419);
            pnGrid.TabIndex = 0;
            // 
            // pnControles
            // 
            pnControles.Controls.Add(btnAceptar);
            pnControles.Controls.Add(btnCancelar);
            pnControles.Dock = DockStyle.Bottom;
            pnControles.Location = new Point(0, 473);
            pnControles.Name = "pnControles";
            pnControles.Size = new Size(864, 48);
            pnControles.TabIndex = 1;
            // 
            // pnStatus
            // 
            pnStatus.Controls.Add(StatusStrip);
            pnStatus.Dock = DockStyle.Bottom;
            pnStatus.Location = new Point(0, 451);
            pnStatus.Name = "pnStatus";
            pnStatus.Size = new Size(864, 22);
            pnStatus.TabIndex = 6;
            // 
            // StatusStrip
            // 
            StatusStrip.AutoSize = false;
            StatusStrip.Items.AddRange(new ToolStripItem[] { tsStatusLabel });
            StatusStrip.Location = new Point(0, 0);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new Size(864, 22);
            StatusStrip.SizingGrip = false;
            StatusStrip.TabIndex = 0;
            StatusStrip.Text = "statusStrip1";
            // 
            // tsStatusLabel
            // 
            tsStatusLabel.Name = "tsStatusLabel";
            tsStatusLabel.Size = new Size(100, 17);
            tsStatusLabel.Text = "Nº de Resultados:";
            // 
            // colTitulo
            // 
            colTitulo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTitulo.FillWeight = 200F;
            colTitulo.HeaderText = "Título";
            colTitulo.Name = "colTitulo";
            colTitulo.ReadOnly = true;
            // 
            // colArtista
            // 
            colArtista.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colArtista.HeaderText = "Artista";
            colArtista.Name = "colArtista";
            colArtista.ReadOnly = true;
            // 
            // colAlbum
            // 
            colAlbum.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colAlbum.HeaderText = "Álbum";
            colAlbum.Name = "colAlbum";
            colAlbum.ReadOnly = true;
            // 
            // colGenero
            // 
            colGenero.HeaderText = "Género";
            colGenero.Name = "colGenero";
            colGenero.ReadOnly = true;
            colGenero.Width = 120;
            // 
            // colAnio
            // 
            colAnio.HeaderText = "Año";
            colAnio.Name = "colAnio";
            colAnio.ReadOnly = true;
            colAnio.Width = 80;
            // 
            // colFuente
            // 
            colFuente.HeaderText = "Fuente";
            colFuente.Name = "colFuente";
            colFuente.ReadOnly = true;
            colFuente.Width = 120;
            // 
            // FrmSeleccionarMetadatos
            // 
            ClientSize = new Size(864, 521);
            Controls.Add(pnGrid);
            Controls.Add(pnStatus);
            Controls.Add(pnOpciones);
            Controls.Add(pnControles);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimizeBox = false;
            MinimumSize = new Size(880, 560);
            Name = "FrmSeleccionarMetadatos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seleccionar metadatos";
            Load += FrmSeleccionarMetadatos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResultados).EndInit();
            pnOpciones.ResumeLayout(false);
            pnOpciones.PerformLayout();
            pnGrid.ResumeLayout(false);
            pnControles.ResumeLayout(false);
            pnStatus.ResumeLayout(false);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ResumeLayout(false);
        }
        private Panel pnStatus;
        private StatusStrip StatusStrip;
        private ToolStripStatusLabel tsStatusLabel;
        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colArtista;
        private DataGridViewTextBoxColumn colAlbum;
        private DataGridViewTextBoxColumn colGenero;
        private DataGridViewTextBoxColumn colAnio;
        private DataGridViewTextBoxColumn colFuente;
    }
}