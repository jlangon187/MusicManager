namespace MusicManager.Forms
{
    partial class FrmPortada
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
            components = new System.ComponentModel.Container();
            pbPortada = new PictureBox();
            toolTip1 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pbPortada).BeginInit();
            SuspendLayout();
            // 
            // pbPortada
            // 
            pbPortada.Dock = DockStyle.Fill;
            pbPortada.Location = new Point(0, 0);
            pbPortada.Name = "pbPortada";
            pbPortada.Size = new Size(600, 600);
            pbPortada.SizeMode = PictureBoxSizeMode.StretchImage;
            pbPortada.TabIndex = 0;
            pbPortada.TabStop = false;
            toolTip1.SetToolTip(pbPortada, "Haga clic en la imagen para cerrar la ventana.");
            pbPortada.Click += pbPortada_Click;
            // 
            // FrmPortada
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 600);
            ControlBox = false;
            Controls.Add(pbPortada);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MaximumSize = new Size(600, 600);
            MinimizeBox = false;
            MinimumSize = new Size(600, 600);
            Name = "FrmPortada";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Portada del Álbum";
            ((System.ComponentModel.ISupportInitialize)pbPortada).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbPortada;
        private ToolTip toolTip1;
    }
}