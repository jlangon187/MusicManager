namespace MusicManager.Forms
{
    partial class frmAcerca
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAcerca));
            pictureBox1 = new PictureBox();
            gbAcerca = new GroupBox();
            lbAcerca = new Label();
            linkLabel1 = new LinkLabel();
            pictureBox2 = new PictureBox();
            lbVersion = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            gbAcerca.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(18, 51);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(160, 166);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // gbAcerca
            // 
            gbAcerca.Controls.Add(lbAcerca);
            gbAcerca.Controls.Add(linkLabel1);
            gbAcerca.Controls.Add(pictureBox2);
            gbAcerca.Location = new Point(198, 51);
            gbAcerca.Name = "gbAcerca";
            gbAcerca.Size = new Size(239, 166);
            gbAcerca.TabIndex = 1;
            gbAcerca.TabStop = false;
            gbAcerca.Text = "Aplicación realizada por";
            // 
            // lbAcerca
            // 
            lbAcerca.AutoSize = true;
            lbAcerca.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lbAcerca.Location = new Point(26, 29);
            lbAcerca.Name = "lbAcerca";
            lbAcerca.Size = new Size(188, 22);
            lbAcerca.TabIndex = 2;
            lbAcerca.Text = "Javier Lanzas González";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(44, 135);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(157, 15);
            linkLabel1.TabIndex = 1;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Ayúdame con una donación";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(87, 70);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(60, 62);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // lbVersion
            // 
            lbVersion.AutoSize = true;
            lbVersion.Location = new Point(18, 232);
            lbVersion.Name = "lbVersion";
            lbVersion.Size = new Size(126, 15);
            lbVersion.TabIndex = 2;
            lbVersion.Text = "Version v1.0 build 1000";
            // 
            // frmAcerca
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(466, 258);
            Controls.Add(lbVersion);
            Controls.Add(gbAcerca);
            Controls.Add(pictureBox1);
            MaximizeBox = false;
            MaximumSize = new Size(482, 297);
            MinimizeBox = false;
            MinimumSize = new Size(482, 297);
            Name = "frmAcerca";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Acerca de";
            Load += frmAcerca_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            gbAcerca.ResumeLayout(false);
            gbAcerca.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private GroupBox gbAcerca;
        private PictureBox pictureBox2;
        private LinkLabel linkLabel1;
        private Label lbAcerca;
        private Label lbVersion;
    }
}