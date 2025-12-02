public static class Organizador
{
    public static string OrganizarArchivo(
        string rutaOriginal,
        string carpetaBase,
        string artista,
        string album,
        string genero,
        string anno,
        int modo)
    {
        try
        {
            if (!File.Exists(rutaOriginal))
                return null;

            string destino = carpetaBase;

            switch (modo)
            {
                case 1: // Artista / Álbum
                    destino = Path.Combine(carpetaBase, artista, album);
                    break;

                case 2: // Género / Año
                    destino = Path.Combine(carpetaBase, genero, anno);
                    break;

                case 3: // Año / Artista
                    destino = Path.Combine(carpetaBase, anno, artista);
                    break;
            }

            // Crear carpetas si no existen
            Directory.CreateDirectory(destino);

            // Construir nueva ruta
            string nombreArchivo = Path.GetFileName(rutaOriginal);
            string nuevaRuta = Path.Combine(destino, nombreArchivo);

            // Evitar colisiones
            if (File.Exists(nuevaRuta))
            {
                string sinExt = Path.GetFileNameWithoutExtension(nombreArchivo);
                string ext = Path.GetExtension(nombreArchivo);
                nuevaRuta = Path.Combine(destino,
                    $"{sinExt}_{DateTime.Now.Ticks}{ext}");
            }

            // Mover archivo
            File.Move(rutaOriginal, nuevaRuta);

            return nuevaRuta;
        }
        catch
        {
            return null;
        }
    }
}