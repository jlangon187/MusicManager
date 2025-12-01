using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MusicManager.Utils;
using static MusicManager.Utils.MetadatosAPI;

namespace MusicManager
{
    public partial class FrmSeleccionarMetadatos : Form
    {
        public List<MetadataResult> resultados = new List<MetadataResult>();
        private List<MetadataResult> lista;

        public MetadataResult Seleccionado { get; private set; }

        public FrmSeleccionarMetadatos(List<MetadataResult> lista)
        {
            InitializeComponent();
            this.lista = lista;
        }

        private void FrmSeleccionarMetadatos_Load(object sender, EventArgs e)
        {
            dgvResultados.Rows.Clear();

            foreach (var r in resultados)
            {
                dgvResultados.Rows.Add(
                    r.Titulo,
                    r.Artista,
                    r.Album,
                    r.Anio,
                    r.Genero
                );
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvResultados.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila primero.");
                return;
            }

            int index = dgvResultados.CurrentRow.Index;
            Seleccionado = resultados[index];

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
