using System;
using System.Windows.Forms;

namespace MusicManager
{
    internal static class Program
    {
        internal static object appMusic;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Lanzar formulario principal
            Application.Run(new FrmMain());
        }
    }
}
