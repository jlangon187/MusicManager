using System;
using System.Windows.Forms;

namespace MusicManager
{
    internal static class Program
    {
        public static AppMusic appMusic;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            appMusic = new AppMusic();

            // Lanzar formulario principal
            Application.Run(new FrmMain());
        }
    }
}
