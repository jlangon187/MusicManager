namespace MusicManager.Forms
{
    partial class FrmConfigAPI
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTituloItunes;
        private Button btnTestItunes;
        private Label lblEstadoItunes;

        private Label lblTituloSpotify;
        private Button btnTestSpotify;
        private Label lblEstadoSpotify;

        private Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfigAPI));
            lblTituloItunes = new Label();
            btnTestItunes = new Button();
            lblEstadoItunes = new Label();
            lblTituloSpotify = new Label();
            btnTestSpotify = new Button();
            lblEstadoSpotify = new Label();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblTituloItunes
            // 
            lblTituloItunes.AutoSize = true;
            lblTituloItunes.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloItunes.Location = new Point(30, 20);
            lblTituloItunes.Name = "lblTituloItunes";
            lblTituloItunes.Size = new Size(202, 21);
            lblTituloItunes.TabIndex = 0;
            lblTituloItunes.Text = "Comprobar API de iTunes";
            // 
            // btnTestItunes
            // 
            btnTestItunes.Location = new Point(65, 75);
            btnTestItunes.Name = "btnTestItunes";
            btnTestItunes.Size = new Size(130, 30);
            btnTestItunes.TabIndex = 2;
            btnTestItunes.Text = "Probar iTunes";
            btnTestItunes.Click += btnTestItunes_Click;
            // 
            // lblEstadoItunes
            // 
            lblEstadoItunes.AutoSize = true;
            lblEstadoItunes.ForeColor = Color.DimGray;
            lblEstadoItunes.Location = new Point(30, 50);
            lblEstadoItunes.Name = "lblEstadoItunes";
            lblEstadoItunes.Size = new Size(230, 15);
            lblEstadoItunes.TabIndex = 1;
            lblEstadoItunes.Text = "Pulsa para comprobar el estado de iTunes.";
            // 
            // lblTituloSpotify
            // 
            lblTituloSpotify.AutoSize = true;
            lblTituloSpotify.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTituloSpotify.Location = new Point(30, 130);
            lblTituloSpotify.Name = "lblTituloSpotify";
            lblTituloSpotify.Size = new Size(208, 21);
            lblTituloSpotify.TabIndex = 3;
            lblTituloSpotify.Text = "Comprobar API de Spotify";
            // 
            // btnTestSpotify
            // 
            btnTestSpotify.Location = new Point(65, 185);
            btnTestSpotify.Name = "btnTestSpotify";
            btnTestSpotify.Size = new Size(130, 30);
            btnTestSpotify.TabIndex = 5;
            btnTestSpotify.Text = "Probar Spotify";
            btnTestSpotify.Click += btnTestSpotify_Click;
            // 
            // lblEstadoSpotify
            // 
            lblEstadoSpotify.AutoSize = true;
            lblEstadoSpotify.ForeColor = Color.DimGray;
            lblEstadoSpotify.Location = new Point(30, 160);
            lblEstadoSpotify.Name = "lblEstadoSpotify";
            lblEstadoSpotify.Size = new Size(232, 15);
            lblEstadoSpotify.TabIndex = 4;
            lblEstadoSpotify.Text = "Pulsa para comprobar el estado de Spotify.";
            // 
            // btnClose
            // 
            btnClose.Location = new Point(260, 230);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(130, 35);
            btnClose.TabIndex = 6;
            btnClose.Text = "Cerrar";
            btnClose.Click += btnClose_Click;
            // 
            // FrmConfigAPI
            // 
            ClientSize = new Size(430, 290);
            Controls.Add(lblTituloItunes);
            Controls.Add(lblEstadoItunes);
            Controls.Add(btnTestItunes);
            Controls.Add(lblTituloSpotify);
            Controls.Add(lblEstadoSpotify);
            Controls.Add(btnTestSpotify);
            Controls.Add(btnClose);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(446, 329);
            MinimizeBox = false;
            MinimumSize = new Size(446, 329);
            Name = "FrmConfigAPI";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Estado de las APIs";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
