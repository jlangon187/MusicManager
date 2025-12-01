using System;
using System.IO;
using System.Windows.Forms;

namespace MusicManager.Utils
{
    public static class Organizador
    {
        /// <summary>
        /// Crea una ruta física usando metadatos y mueve el archivo.
        /// </summary>
        public static void OrganizarArchivo(
            string rutaOriginal,
            string carpetaBase,
            string artista,
            string album,
            string genero,
            string año,
            int modo)
        {
            // Evitar caracteres no válidos
            artista = Sanitizar(artista);
            album = Sanitizar(album);
            genero = Sanitizar(genero);
            año = Sanitizar(año);

            string nuevaRuta = "";

            switch (modo)
            {
                case 1: // Artista / Álbum / archivo
                    nuevaRuta = Path.Combine(carpetaBase, artista, album);
                    break;

                case 2: // Género / Año / archivo
                    nuevaRuta = Path.Combine(carpetaBase, genero, año);
                    break;

                case 3: // Año / Artista / archivo
                    nuevaRuta = Path.Combine(carpetaBase, año, artista);
                    break;

                default:
                    MessageBox.Show("Modo de organización no reconocido.");
                    return;
            }

            // Crear carpetas si no existen
            Directory.CreateDirectory(nuevaRuta);

            // Nombre del archivo
            string nombreArchivo = Path.GetFileName(rutaOriginal);

            string destino = Path.Combine(nuevaRuta, nombreArchivo);

            // Mover el archivo
            try
            {
                File.Move(rutaOriginal, destino, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error moviendo archivo:\n" + ex.Message);
            }
        }

        private static string Sanitizar(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return "Desconocido";

            foreach (char c in Path.GetInvalidFileNameChars())
                valor = valor.Replace(c, '_');

            return valor.Trim();
        }
    }
}
