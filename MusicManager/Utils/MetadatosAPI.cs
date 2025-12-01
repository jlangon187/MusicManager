using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicManager.Utils
{
    public static class MetadatosAPI
    {
        private static readonly HttpClient http = new HttpClient();

        /// <summary>
        /// Buscar una única coincidencia rápida (primer resultado).
        /// </summary>
        public static async Task<MetadataResult> BuscarMetadatos(string titulo, string artista)
        {
            var lista = await BuscarLista(titulo, artista);

            if (lista.Count > 0)
                return lista[0];

            return null;
        }

        /// <summary>
        /// Buscar una lista completa de coincidencias en MusicBrainz.
        /// </summary>
        public static async Task<List<MetadataResult>> BuscarLista(string titulo, string artista)
        {
            List<MetadataResult> resultados = new List<MetadataResult>();

            try
            {
                string qTitulo = Uri.EscapeDataString(titulo);
                string qArtista = Uri.EscapeDataString(artista);

                string url =
                    $"https://musicbrainz.org/ws/2/recording/?query=recording:{qTitulo}%20AND%20artist:{qArtista}&fmt=json";

                var response = await http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return resultados;

                string json = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("recordings", out JsonElement recordings))
                    return resultados;

                foreach (var rec in recordings.EnumerateArray())
                {
                    var r = new MetadataResult();

                    // Título
                    r.Titulo = rec.GetProperty("title").GetString() ?? "";

                    // Artista
                    if (rec.TryGetProperty("artist-credit", out var ac))
                    {
                        if (ac.GetArrayLength() > 0)
                        {
                            r.Artista = ac[0]
                                .GetProperty("artist")
                                .GetProperty("name")
                                .GetString() ?? "";
                        }
                    }

                    // Álbum
                    if (rec.TryGetProperty("releases", out var rels) && rels.GetArrayLength() > 0)
                    {
                        r.Album = rels[0].GetProperty("title").GetString() ?? "";

                        // Año
                        if (rels[0].TryGetProperty("date", out var date))
                        {
                            string d = date.GetString();
                            if (!string.IsNullOrEmpty(d) && d.Length >= 4)
                                r.Anio = int.TryParse(d.Substring(0, 4), out int y) ? y : 0;
                        }
                    }

                    // Género / etiquetas
                    if (rec.TryGetProperty("tags", out var tags) && tags.GetArrayLength() > 0)
                    {
                        r.Genero = tags[0].GetProperty("name").GetString();
                    }
                    else
                    {
                        r.Genero = "";
                    }

                    resultados.Add(r);
                }

                return resultados;
            }
            catch
            {
                return resultados;
            }
        }
    }

    /// <summary>
    /// Estructura con metadatos completos de MusicBrainz.
    /// </summary>
    public class MetadataResult
    {
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }
        public int Anio { get; set; }
        public string Genero { get; set; }

        public override string ToString()
        {
            return $"{Artista} - {Titulo} ({Album}, {Anio}) [{Genero}]";
        }
    }
}
