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
        public MetadataResult Seleccionado { get; private set; }

        public FrmSeleccionarMetadatos(List<MetadataResult> lista)
        {
            InitializeComponent();
            resultados = lista;
        }

        private void FrmSeleccionarMetadatos_Load(object sender, EventArgs e)
        {
            CargarResultadosEnGrid(resultados);
        }

        private void CargarResultadosEnGrid(List<MetadataResult> lista)
        {
            dgvResultados.Rows.Clear();

            foreach (var r in lista)
            {
                dgvResultados.Rows.Add(
                    r.Titulo,
                    r.Artista,
                    r.Album,
                    r.Genero,
                    r.Anio
                );
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtBusqueda.Text.Trim();
            if (texto.Length == 0)
            {
                MessageBox.Show("Escribe algo para buscar.");
                return;
            }

            string titulo = texto;
            string artista = "";

            if (texto.Contains("-"))
            {
                var partes = texto.Split('-', 2);
                titulo = partes[0].Trim();
                artista = partes[1].Trim();
            }

            var nuevosResultados = await MetadatosAPI.BuscarLista(titulo, artista);

            if (nuevosResultados.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                return;
            }

            resultados = nuevosResultados;
            CargarResultadosEnGrid(resultados);
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
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
