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


        // =====================================================
        //  LOAD DEL FORMULARIO
        // =====================================================
        private void FrmSeleccionarMetadatos_Load(object sender, EventArgs e)
        {
            // Sincronizar con configuración actual de la API
            chkSpotify.Checked = MetadatosAPI.UsarSpotify;
            chkITunes.Checked = MetadatosAPI.UsarITunes;

            CargarResultadosEnGrid(resultados);
        }


        // =====================================================
        //  CARGAR RESULTADOS EN GRID
        // =====================================================
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
                    r.Anio,
                    r.Fuente
                );
            }
            tsStatusLabel.Text = $"Mostrando {lista.Count} resultados.";
        }


        // =====================================================
        //  BUSCAR MANUALMENTE
        // =====================================================
        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtBusqueda.Text.Trim();
            if (texto.Length == 0)
            {
                MessageBox.Show("Escribe algo para buscar.");
                return;
            }

            // Aplicar fuentes seleccionadas
            MetadatosAPI.UsarSpotify = chkSpotify.Checked;
            MetadatosAPI.UsarITunes = chkITunes.Checked;

            if (!MetadatosAPI.UsarSpotify && !MetadatosAPI.UsarITunes)
            {
                MessageBox.Show("Debes seleccionar al menos una fuente (Spotify o iTunes).");
                return;
            }

            // Desactivar los controles mientras se busca
            tsStatusLabel.Text = "Buscando...";
            DesactivarControles();

            string titulo = texto;
            string artista = "";

            if (texto.Contains("-"))
            {
                var partes = texto.Split('-', 2);
                titulo = partes[0].Trim();
                artista = partes[1].Trim();
            }

            // Buscar usando el sistema híbrido
            var nuevosResultados = await MetadatosAPI.BuscarLista(titulo, artista);

            if (nuevosResultados.Count == 0)
            {
                MessageBox.Show("No se encontraron resultados.");
                tsStatusLabel.Text = "No se encontraron resultados.";
                return;
            }

            // Reactivar los controles
            ActivarControles();

            resultados = nuevosResultados;
            tsStatusLabel.Text = $"Mostrando {resultados.Count} resultados.";
            CargarResultadosEnGrid(resultados);
        }


        // =====================================================
        //  ACEPTAR SELECCIÓN
        // =====================================================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvResultados.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila primero.");
                return;
            }

            int index = dgvResultados.CurrentRow.Index;

            if (index >= 0 && index < resultados.Count)
            {
                Seleccionado = resultados[index];
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }


        // =====================================================
        //  CANCELAR
        // =====================================================
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DesactivarControles()
        {
            pnControles.Enabled = false;
            pnGrid.Enabled = false;
            pnOpciones.Enabled = false;
        }

        private void ActivarControles()
        {
            pnControles.Enabled = true;
            pnGrid.Enabled = true;
            pnOpciones.Enabled = true;
        }
    }
}
