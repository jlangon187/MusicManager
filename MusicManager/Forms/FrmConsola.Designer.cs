#if DEBUG
namespace MusicManager.Forms
{
    partial class FrmConsola
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtConsola;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.CheckBox chkAutoActualizar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtConsola = new TextBox();
            btnActualizar = new Button();
            chkAutoActualizar = new CheckBox();
            SuspendLayout();
            // 
            // txtConsola
            // 
            txtConsola.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtConsola.BackColor = Color.Black;
            txtConsola.ForeColor = Color.White;
            txtConsola.Location = new Point(12, 41);
            txtConsola.Multiline = true;
            txtConsola.Name = "txtConsola";
            txtConsola.Size = new Size(760, 407);
            txtConsola.TabIndex = 2;
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(12, 12);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(100, 23);
            btnActualizar.TabIndex = 0;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // chkAutoActualizar
            // 
            chkAutoActualizar.AutoSize = true;
            chkAutoActualizar.Location = new Point(130, 15);
            chkAutoActualizar.Name = "chkAutoActualizar";
            chkAutoActualizar.Size = new Size(175, 19);
            chkAutoActualizar.TabIndex = 1;
            chkAutoActualizar.Text = "Actualizar automáticamente";
            chkAutoActualizar.UseVisualStyleBackColor = true;
            chkAutoActualizar.CheckedChanged += chkAutoActualizar_CheckedChanged;
            // 
            // FrmConsola
            // 
            ClientSize = new Size(784, 461);
            Controls.Add(chkAutoActualizar);
            Controls.Add(btnActualizar);
            Controls.Add(txtConsola);
            Name = "FrmConsola";
            Text = "Consola de depuración";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
#endif
