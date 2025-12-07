using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MusicManager.Utils
{
    public static class Organizador
    {
        // Limpiar nombres de carpetas y archivos
        public static string LimpiarNombre(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "Desconocido";

            texto = texto.Trim();
            texto = Regex.Replace(texto, @"[\\/:*?""<>|]", "");
            texto = Regex.Replace(texto, @"\s{2,}", " ");
            return texto;
        }

        // Obtener carpeta destino según el modo seleccionado
        public static string ObtenerCarpetaDestino(string basePath,
            string artista, string album, string genero, string anio, int modo)
        {
            artista = LimpiarNombre(artista);
            album = LimpiarNombre(album);
            genero = LimpiarNombre(genero);
            anio = LimpiarNombre(anio);

            return modo switch
            {
                1 => Path.Combine(basePath, artista, album),
                2 => Path.Combine(basePath, genero, anio),
                3 => Path.Combine(basePath, anio, artista),
                4 => Path.Combine(basePath, artista, anio, album),
                _ => basePath
            };
        }
    }
}
