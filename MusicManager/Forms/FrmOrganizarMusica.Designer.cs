namespace MusicManager.Forms
{
    partial class FrmOrganizarMusica
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox cbModo;
        private TextBox txtRutaAntigua;
        private TextBox txtRutaNueva;
        private Label lblAntigua;
        private Label lblNueva;
        private Button btnAceptar;
        private Button btnCerrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOrganizarMusica));
            cbModo = new ComboBox();
            txtRutaAntigua = new TextBox();
            txtRutaNueva = new TextBox();
            lblAntigua = new Label();
            lblNueva = new Label();
            btnAceptar = new Button();
            btnCerrar = new Button();
            toolTip1 = new ToolTip(components);
            lbSeleccionador = new Label();
            SuspendLayout();
            // 
            // cbModo
            // 
            cbModo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbModo.Location = new Point(17, 35);
            cbModo.Name = "cbModo";
            cbModo.Size = new Size(260, 23);
            cbModo.TabIndex = 1;
            cbModo.SelectedIndexChanged += cbModo_SelectedIndexChanged;
            // 
            // txtRutaAntigua
            // 
            txtRutaAntigua.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRutaAntigua.Location = new Point(17, 96);
            txtRutaAntigua.Name = "txtRutaAntigua";
            txtRutaAntigua.ReadOnly = true;
            txtRutaAntigua.Size = new Size(645, 23);
            txtRutaAntigua.TabIndex = 3;
            // 
            // txtRutaNueva
            // 
            txtRutaNueva.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRutaNueva.Location = new Point(17, 160);
            txtRutaNueva.Name = "txtRutaNueva";
            txtRutaNueva.ReadOnly = true;
            txtRutaNueva.Size = new Size(645, 23);
            txtRutaNueva.TabIndex = 5;
            // 
            // lblAntigua
            // 
            lblAntigua.Location = new Point(17, 72);
            lblAntigua.Name = "lblAntigua";
            lblAntigua.Size = new Size(130, 23);
            lblAntigua.TabIndex = 2;
            lblAntigua.Text = "Ruta actual (ejemplo):";
            // 
            // lblNueva
            // 
            lblNueva.Location = new Point(17, 135);
            lblNueva.Name = "lblNueva";
            lblNueva.Size = new Size(150, 23);
            lblNueva.TabIndex = 4;
            lblNueva.Text = "Nueva ruta (ejemplo):";
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(17, 198);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(150, 32);
            btnAceptar.TabIndex = 6;
            btnAceptar.Text = "Organizar música";
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(172, 198);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 32);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;
            // 
            // lbSeleccionador
            // 
            lbSeleccionador.Location = new Point(17, 11);
            lbSeleccionador.Name = "lbSeleccionador";
            lbSeleccionador.Size = new Size(205, 23);
            lbSeleccionador.TabIndex = 0;
            lbSeleccionador.Text = "Seleccione el modo de organización:";
            // 
            // FrmOrganizarMusica
            // 
            ClientSize = new Size(674, 251);
            Controls.Add(lbSeleccionador);
            Controls.Add(cbModo);
            Controls.Add(lblAntigua);
            Controls.Add(lblNueva);
            Controls.Add(txtRutaAntigua);
            Controls.Add(txtRutaNueva);
            Controls.Add(btnAceptar);
            Controls.Add(btnCerrar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(1000, 290);
            MinimizeBox = false;
            MinimumSize = new Size(690, 290);
            Name = "FrmOrganizarMusica";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Organizar Música";
            ResumeLayout(false);
            PerformLayout();
        }
        private ToolTip toolTip1;
        private Label lbSeleccionador;
    }
}
