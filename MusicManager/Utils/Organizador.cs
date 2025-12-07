using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MusicManager.Utils
{
    public static class Organizador
    {
        public static string LimpiarNombre(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "Desconocido";

            texto = texto.Trim();
            texto = Regex.Replace(texto, @"[\\/:*?""<>|]", "");
            texto = Regex.Replace(texto, @"\s{2,}", " ");
            return texto;
        }

        // Mantener nombre original
        public static string ObtenerNombreArchivoOriginal(string ruta)
        {
            return Path.GetFileName(ruta);
        }

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

        public class ResultadoOrganizacion
        {
            public string RutaActual { get; set; }
            public string RutaNueva { get; set; }
            public string Archivo { get; set; }
            public string Estado { get; set; } // Movido, Sin cambios, Conflicto, Error
        }

        public static ResultadoOrganizacion Previsualizar(
            string ruta,
            string carpetaBase,
            string titulo,
            string artista,
            string album,
            string genero,
            string anio,
            int modo)
        {
            var r = new ResultadoOrganizacion();

            try
            {
                r.RutaActual = ruta;
                r.Archivo = ObtenerNombreArchivoOriginal(ruta);

                string carpetaDestino = ObtenerCarpetaDestino(
                    carpetaBase, artista, album, genero, anio, modo);

                r.RutaNueva = Path.Combine(carpetaDestino, r.Archivo);

                if (r.RutaActual.Equals(r.RutaNueva, StringComparison.InvariantCultureIgnoreCase))
                {
                    r.Estado = "Sin cambios";
                    return r;
                }

                if (File.Exists(r.RutaNueva))
                {
                    r.Estado = "Conflicto";
                    return r;
                }

                r.Estado = "Movido";
            }
            catch
            {
                r.Estado = "Error";
            }

            return r;
        }

        public static bool Aplicar(ResultadoOrganizacion r)
        {
            if (r.Estado != "Movido")
                return false;

            try
            {
                string carpeta = Path.GetDirectoryName(r.RutaNueva);
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                File.Move(r.RutaActual, r.RutaNueva);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
