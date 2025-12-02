using MusicManager.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicManager.Forms
{
    public partial class FrmSincronizar : Form
    {
        private readonly List<string> nuevas;
        private readonly List<string> perdidas;
        private readonly GestorMusica gm;

        public FrmSincronizar(List<string> nuevas, List<string> perdidas, GestorMusica gm)
        {
            InitializeComponent();
            this.nuevas = nuevas;
            this.perdidas = perdidas;
            this.gm = gm;
        }

        private void FrmSincronizar_Load(object sender, EventArgs e)
        {
            dgvNuevas.Rows.Clear();
            dgvPerdidas.Rows.Clear();

            foreach (var n in nuevas)
                dgvNuevas.Rows.Add(n);

            foreach (var p in perdidas)
                dgvPerdidas.Rows.Add(p);

            lblNuevas.Text = $"Canciones nuevas encontradas: {nuevas.Count}";
            lblPerdidas.Text = $"Canciones perdidas (no existen en carpeta): {perdidas.Count}";
        }

        // ------------------------------------------------------------
        // IMPORTAR NUEVAS CANCIONES
        // ------------------------------------------------------------
        private void btnImportar_Click(object sender, EventArgs e)
        {
            if (nuevas.Count == 0)
            {
                MessageBox.Show("No hay canciones nuevas que importar.");
                return;
            }

            int importadas = 0;

            foreach (DataGridViewRow row in dgvNuevas.Rows)
            {
                string ruta = row.Cells[0].Value.ToString();

                try
                {
                    gm.InsertarCancionDesdeArchivo(ruta);
                    importadas++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error importando:\n" + ruta + "\n" + ex.Message);
                }
            }

            MessageBox.Show($"Importación completada. {importadas} canciones agregadas.");
        }

        // ------------------------------------------------------------
        // ELIMINAR PERDIDAS DE LA BD
        // ------------------------------------------------------------
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (perdidas.Count == 0)
            {
                MessageBox.Show("No hay canciones perdidas que eliminar.");
                return;
            }

            int eliminadas = 0;

            foreach (DataGridViewRow row in dgvPerdidas.Rows)
            {
                string ruta = row.Cells[0].Value.ToString();

                try
                {
                    gm.EliminarCancionPorRuta(ruta);
                    eliminadas++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando:\n" + ruta + "\n" + ex.Message);
                }
            }

            MessageBox.Show($"Eliminación completada. {eliminadas} canciones removidas de la BD.");
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
