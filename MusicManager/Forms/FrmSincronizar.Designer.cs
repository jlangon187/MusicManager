namespace MusicManager.Forms
{
    partial class FrmSincronizar
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvNuevas;
        private System.Windows.Forms.DataGridView dgvPerdidas;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblNuevas;
        private System.Windows.Forms.Label lblPerdidas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvNuevas = new System.Windows.Forms.DataGridView();
            this.dgvPerdidas = new System.Windows.Forms.DataGridView();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblNuevas = new System.Windows.Forms.Label();
            this.lblPerdidas = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvNuevas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerdidas)).BeginInit();
            this.SuspendLayout();

            // 
            // dgvNuevas
            // 
            this.dgvNuevas.AllowUserToAddRows = false;
            this.dgvNuevas.AllowUserToDeleteRows = false;
            this.dgvNuevas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNuevas.Location = new System.Drawing.Point(20, 45);
            this.dgvNuevas.Name = "dgvNuevas";
            this.dgvNuevas.RowHeadersVisible = false;
            this.dgvNuevas.Size = new System.Drawing.Size(700, 180);

            this.dgvNuevas.Columns.Add("RutaNueva", "Ruta del archivo");

            // 
            // lblNuevas
            // 
            this.lblNuevas.AutoSize = true;
            this.lblNuevas.Location = new System.Drawing.Point(20, 20);
            this.lblNuevas.Name = "lblNuevas";
            this.lblNuevas.Size = new System.Drawing.Size(180, 15);
            this.lblNuevas.Text = "Canciones nuevas encontradas:";

            // 
            // dgvPerdidas
            // 
            this.dgvPerdidas.AllowUserToAddRows = false;
            this.dgvPerdidas.AllowUserToDeleteRows = false;
            this.dgvPerdidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPerdidas.Location = new System.Drawing.Point(20, 265);
            this.dgvPerdidas.Name = "dgvPerdidas";
            this.dgvPerdidas.RowHeadersVisible = false;
            this.dgvPerdidas.Size = new System.Drawing.Size(700, 180);

            this.dgvPerdidas.Columns.Add("RutaPerdida", "Ruta almacenada en BD");

            // 
            // lblPerdidas
            // 
            this.lblPerdidas.AutoSize = true;
            this.lblPerdidas.Location = new System.Drawing.Point(20, 240);
            this.lblPerdidas.Name = "lblPerdidas";
            this.lblPerdidas.Size = new System.Drawing.Size(250, 15);
            this.lblPerdidas.Text = "Canciones perdidas (no existen en carpeta):";

            // 
            // btnImportar
            // 
            this.btnImportar.Location = new System.Drawing.Point(740, 75);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(120, 40);
            this.btnImportar.Text = "Importar nuevas";
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);

            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(740, 295);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(120, 40);
            this.btnEliminar.Text = "Eliminar perdidas";
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(740, 405);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(120, 40);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);

            // 
            // FrmSincronizar
            // 
            this.ClientSize = new System.Drawing.Size(880, 470);
            this.Controls.Add(this.dgvNuevas);
            this.Controls.Add(this.dgvPerdidas);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.lblNuevas);
            this.Controls.Add(this.lblPerdidas);
            this.Name = "FrmSincronizar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sincronizar Biblioteca";
            this.Load += new System.EventHandler(this.FrmSincronizar_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvNuevas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerdidas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
