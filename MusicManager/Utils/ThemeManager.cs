using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusicManager.Utils
{
    public static class ThemeManager
    {
        // ========== PALETA MODERNA (NO OSCURA) ==========
        public static readonly Color FondoPrincipal = Color.FromArgb(243, 247, 242);
        public static readonly Color FondoPanel = Color.FromArgb(230, 242, 228);
        public static readonly Color FondoSecundario = Color.FromArgb(220, 239, 216);
        public static readonly Color VerdePastel = Color.FromArgb(139, 209, 124);
        public static readonly Color VerdeHover = Color.FromArgb(168, 230, 163);
        public static readonly Color VerdeTextoOscuro = Color.FromArgb(30, 76, 45);
        public static readonly Color TextoPrincipal = Color.FromArgb(30, 30, 30);
        public static readonly Color TextoSecundario = Color.FromArgb(80, 80, 80);

        // ========== MÉTODO PRINCIPAL ==========
        public static void ApplyTheme(Control root)
        {
            if (root == null) return;

            root.BackColor = FondoPrincipal;
            root.ForeColor = TextoPrincipal;

            foreach (Control ctrl in root.Controls)
                ApplyControlTheme(ctrl);
        }

        // ========== APLICAR ESTILO A CONTROLES ==========
        private static void ApplyControlTheme(Control ctrl)
        {
            // ========== Panel =============
            if (ctrl is Panel)
            {
                ctrl.BackColor = FondoPanel;
                ctrl.ForeColor = TextoPrincipal;
            }

            // ========== Botones con transición ==========
            if (ctrl is Button btn)
                ApplyButtonTheme(btn);

            // ========== TextBox ==========
            if (ctrl is TextBox txt)
            {
                txt.BackColor = FondoSecundario;
                txt.ForeColor = TextoPrincipal;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // ========== MenuStrip ==========
            if (ctrl is MenuStrip menu)
            {
                menu.BackColor = FondoPanel;
                menu.ForeColor = TextoPrincipal;

                foreach (ToolStripMenuItem item in menu.Items)
                    ThemeMenuItem(item);
            }

            // ========== StatusStrip ==========
            if (ctrl is StatusStrip strip)
            {
                strip.BackColor = FondoPanel;
                strip.ForeColor = TextoSecundario;
            }

            // ========== DataGridView moderno ==========
            if (ctrl is DataGridView dgv)
                ApplyDataGridTheme(dgv);

            // Recursivo → hijos del control
            foreach (Control child in ctrl.Controls)
                ApplyControlTheme(child);
        }

        private static void ThemeMenuItem(ToolStripMenuItem item)
        {
            item.BackColor = FondoPanel;
            item.ForeColor = TextoPrincipal;

            foreach (ToolStripItem sub in item.DropDownItems)
            {
                sub.BackColor = FondoPanel;
                sub.ForeColor = TextoPrincipal;
            }
        }

        // ========== ESTILO DGV ==========
        private static void ApplyDataGridTheme(DataGridView dgv)
        {
            dgv.BackgroundColor = FondoPrincipal;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = VerdePastel;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = VerdePastel;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            dgv.DefaultCellStyle.BackColor = FondoSecundario;
            dgv.DefaultCellStyle.ForeColor = TextoPrincipal;
            dgv.DefaultCellStyle.SelectionBackColor = VerdeHover;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.RowHeadersVisible = false;
        }

        // ========== BOTONES CON ANIMACIÓN ==========
        private static void ApplyButtonTheme(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = VerdePastel;
            btn.ForeColor = Color.Black;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            // Eventos de animación
            btn.MouseEnter -= Button_MouseEnter;
            btn.MouseLeave -= Button_MouseLeave;

            btn.MouseEnter += Button_MouseEnter;
            btn.MouseLeave += Button_MouseLeave;

            btn.Paint -= Button_PaintBorder;
            btn.Paint += Button_PaintBorder;
        }

        // ========== Animación hover ==========
        private static void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
                AnimateButtonColor(btn, VerdeHover);
        }

        private static void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
                AnimateButtonColor(btn, VerdePastel);
        }

        // Animación suave de color
        private static void AnimateButtonColor(Button btn, Color target)
        {
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 15;

            t.Tick += (s, e) =>
            {
                btn.BackColor = ColorBlend(btn.BackColor, target, 0.15);

                if (Math.Abs(btn.BackColor.R - target.R) < 5)
                {
                    btn.BackColor = target;
                    t.Stop();
                    t.Dispose();
                }
            };

            t.Start();
        }

        // Mezcla progresiva de colores (transición)
        private static Color ColorBlend(Color a, Color b, double amount)
        {
            int r = (int)(a.R + (b.R - a.R) * amount);
            int g = (int)(a.G + (b.G - a.G) * amount);
            int b2 = (int)(a.B + (b.B - a.B) * amount);
            return Color.FromArgb(r, g, b2);
        }

        // Borde personalizado (opcional)
        private static void Button_PaintBorder(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;

            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);

            // Borde exterior oscuro
            using (Pen p1 = new Pen(Color.FromArgb(90, 120, 90), 2))
                e.Graphics.DrawRectangle(p1, rect);

            // Borde interior claro
            Rectangle inner = new Rectangle(2, 2, rect.Width - 4, rect.Height - 4);
            using (Pen p2 = new Pen(Color.FromArgb(210, 255, 210), 1))
                e.Graphics.DrawRectangle(p2, inner);
        }
    }
}
