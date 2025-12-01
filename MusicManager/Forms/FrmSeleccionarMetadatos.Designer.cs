namespace MusicManager
{
    partial class FrmSeleccionarMetadatos
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvResultados;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblTitulo;

        private DataGridViewTextBoxColumn colTitulo;
        private DataGridViewTextBoxColumn colArtista;
        private DataGridViewTextBoxColumn colAlbum;
        private DataGridViewTextBoxColumn colAnio;
        private DataGridViewTextBoxColumn colGenero;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            dgvResultados = new DataGridView();
            btnAceptar = new Button();
            btnCancelar = new Button();
            lblTitulo = new Label();

            colTitulo = new DataGridViewTextBoxColumn();
            colArtista = new DataGridViewTextBoxColumn();
            colAlbum = new DataGridViewTextBoxColumn();
            colAnio = new DataGridViewTextBoxColumn();
            colGenero = new DataGridViewTextBoxColumn();

            // FORM
            this.ClientSize = new Size(700, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Seleccionar metadatos";
            this.Load += FrmSeleccionarMetadatos_Load;

            // LABEL
            lblTitulo.Text = "Seleccione la coincidencia correcta:";
            lblTitulo.AutoSize = true;
            lblTitulo.Left = 10;
            lblTitulo.Top = 10;
            lblTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            // DATAGRIDVIEW
            dgvResultados.Left = 10;
            dgvResultados.Top = 40;
            dgvResultados.Width = 670;
            dgvResultados.Height = 280;
            dgvResultados.AllowUserToAddRows = false;
            dgvResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResultados.RowHeadersVisible = false;

            colTitulo.HeaderText = "Título";
            colArtista.HeaderText = "Artista";
            colAlbum.HeaderText = "Álbum";
            colAnio.HeaderText = "Año";
            colGenero.HeaderText = "Género";

            dgvResultados.Columns.AddRange(colTitulo, colArtista, colAlbum, colAnio, colGenero);

            // BOTONES
            btnAceptar.Text = "Aceptar";
            btnAceptar.Width = 120;
            btnAceptar.Left = 380;
            btnAceptar.Top = 330;
            btnAceptar.Click += btnAceptar_Click;

            btnCancelar.Text = "Cancelar";
            btnCancelar.Width = 120;
            btnCancelar.Left = 530;
            btnCancelar.Top = 330;
            btnCancelar.Click += btnCancelar_Click;

            // ADD ALL
            this.Controls.Add(lblTitulo);
            this.Controls.Add(dgvResultados);
            this.Controls.Add(btnAceptar);
            this.Controls.Add(btnCancelar);
        }
    }
}
