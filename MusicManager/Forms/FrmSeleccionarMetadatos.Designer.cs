namespace MusicManager
{
    partial class FrmSeleccionarMetadatos
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvResultados;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblTitulo;
        private TextBox txtBusqueda;
        private Button btnBuscar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvResultados = new DataGridView();
            btnAceptar = new Button();
            btnCancelar = new Button();
            lblTitulo = new Label();
            txtBusqueda = new TextBox();
            btnBuscar = new Button();
            panel1 = new Panel();
            pnControles = new Panel();
            colTitulo = new DataGridViewTextBoxColumn();
            colArtista = new DataGridViewTextBoxColumn();
            colAlbum = new DataGridViewTextBoxColumn();
            colGenero = new DataGridViewTextBoxColumn();
            colAnio = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvResultados).BeginInit();
            panel1.SuspendLayout();
            pnControles.SuspendLayout();
            SuspendLayout();
            // 
            // dgvResultados
            // 
            dgvResultados.AllowUserToAddRows = false;
            dgvResultados.Columns.AddRange(new DataGridViewColumn[] { colTitulo, colArtista, colAlbum, colGenero, colAnio });
            dgvResultados.Dock = DockStyle.Fill;
            dgvResultados.Location = new Point(0, 0);
            dgvResultados.Name = "dgvResultados";
            dgvResultados.RowHeadersVisible = false;
            dgvResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResultados.Size = new Size(864, 394);
            dgvResultados.TabIndex = 1;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(285, 13);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(120, 23);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(479, 13);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 23);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitulo.Location = new Point(10, 10);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(251, 20);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Seleccione la coincidencia correcta:";
            // 
            // txtBusqueda
            // 
            txtBusqueda.Dock = DockStyle.Top;
            txtBusqueda.Location = new Point(0, 0);
            txtBusqueda.Name = "txtBusqueda";
            txtBusqueda.PlaceholderText = "Buscar manualmente...";
            txtBusqueda.Size = new Size(864, 23);
            txtBusqueda.TabIndex = 3;
            // 
            // btnBuscar
            // 
            btnBuscar.Dock = DockStyle.Top;
            btnBuscar.Location = new Point(0, 23);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(864, 32);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.Click += btnBuscar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(dgvResultados);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 55);
            panel1.Name = "panel1";
            panel1.Size = new Size(864, 394);
            panel1.TabIndex = 4;
            // 
            // pnControles
            // 
            pnControles.Controls.Add(btnAceptar);
            pnControles.Controls.Add(btnCancelar);
            pnControles.Dock = DockStyle.Bottom;
            pnControles.Location = new Point(0, 401);
            pnControles.Name = "pnControles";
            pnControles.Size = new Size(864, 48);
            pnControles.TabIndex = 5;
            // 
            // colTitulo
            // 
            colTitulo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTitulo.HeaderText = "Título";
            colTitulo.Name = "colTitulo";
            // 
            // colArtista
            // 
            colArtista.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colArtista.HeaderText = "Artista";
            colArtista.Name = "colArtista";
            colArtista.Width = 250;
            // 
            // colAlbum
            // 
            colAlbum.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colAlbum.HeaderText = "Álbum";
            colAlbum.Name = "colAlbum";
            colAlbum.Width = 200;
            // 
            // colGenero
            // 
            colGenero.HeaderText = "Género";
            colGenero.Name = "colGenero";
            colGenero.Width = 120;
            // 
            // colAnio
            // 
            colAnio.HeaderText = "Año";
            colAnio.Name = "colAnio";
            colAnio.Width = 80;
            // 
            // FrmSeleccionarMetadatos
            // 
            ClientSize = new Size(864, 449);
            Controls.Add(pnControles);
            Controls.Add(panel1);
            Controls.Add(btnBuscar);
            Controls.Add(txtBusqueda);
            Controls.Add(lblTitulo);
            Name = "FrmSeleccionarMetadatos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seleccionar metadatos";
            Load += FrmSeleccionarMetadatos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResultados).EndInit();
            panel1.ResumeLayout(false);
            pnControles.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        private Panel panel1;
        private Panel pnControles;
        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colArtista;
        private DataGridViewTextBoxColumn colAlbum;
        private DataGridViewTextBoxColumn colGenero;
        private DataGridViewTextBoxColumn colAnio;
    }
}
