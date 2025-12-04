namespace MusicManager.Forms
{
    partial class FrmConfigAPI
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private GroupBox gpItunes;
        private CheckBox chkItunes;
        private Button btnTestItunes;
        private Button btnGuardar;
        private Label lblEstado;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            gpItunes = new GroupBox();
            chkItunes = new CheckBox();
            btnTestItunes = new Button();
            btnGuardar = new Button();
            lblEstado = new Label();
            gpItunes.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(337, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Configuración de APIs de Metadatos";
            // 
            // gpItunes
            // 
            gpItunes.Controls.Add(chkItunes);
            gpItunes.Location = new Point(20, 60);
            gpItunes.Name = "gpItunes";
            gpItunes.Size = new Size(360, 70);
            gpItunes.TabIndex = 1;
            gpItunes.TabStop = false;
            gpItunes.Text = "iTunes Search API";
            // 
            // chkItunes
            // 
            chkItunes.AutoSize = true;
            chkItunes.Location = new Point(20, 30);
            chkItunes.Name = "chkItunes";
            chkItunes.Size = new Size(108, 19);
            chkItunes.TabIndex = 0;
            chkItunes.Text = "Usar iTunes API";
            // 
            // btnTestItunes
            // 
            btnTestItunes.Location = new Point(400, 75);
            btnTestItunes.Name = "btnTestItunes";
            btnTestItunes.Size = new Size(130, 35);
            btnTestItunes.TabIndex = 2;
            btnTestItunes.Text = "Probar iTunes";
            btnTestItunes.Click += btnTestItunes_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(227, 177);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(130, 40);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = Color.DimGray;
            lblEstado.Location = new Point(24, 140);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(0, 15);
            lblEstado.TabIndex = 4;
            // 
            // FrmConfigAPI
            // 
            ClientSize = new Size(580, 229);
            Controls.Add(lblTitulo);
            Controls.Add(gpItunes);
            Controls.Add(btnTestItunes);
            Controls.Add(btnGuardar);
            Controls.Add(lblEstado);
            Name = "FrmConfigAPI";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Configuración de APIs";
            gpItunes.ResumeLayout(false);
            gpItunes.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
