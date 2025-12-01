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

        // Constructor estático → se ejecuta solo 1 vez
        static MetadatosAPI()
        {
            http.DefaultRequestHeaders.UserAgent.Clear();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("MusicManager/1.0 (contacto@ejemplo.com)");
        }

        /// <summary>
        /// Buscar una coincidencia rápida (primer resultado).
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
                // Construir búsqueda flexible (mucho más efectiva)
                string query = Uri.EscapeDataString($"{titulo} {artista}");

                string url = $"https://musicbrainz.org/ws/2/recording/?query={query}&fmt=json";

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

                    // TÍTULO
                    r.Titulo = rec.GetProperty("title").GetString() ?? "";

                    // ARTISTA
                    if (rec.TryGetProperty("artist-credit", out var ac) &&
                        ac.GetArrayLength() > 0)
                    {
                        r.Artista = ac[0]
                            .GetProperty("artist")
                            .GetProperty("name")
                            .GetString() ?? "";
                    }

                    // ALBUM + AÑO
                    if (rec.TryGetProperty("releases", out var rels) &&
                        rels.GetArrayLength() > 0)
                    {
                        r.Album = rels[0].GetProperty("title").GetString() ?? "";

                        if (rels[0].TryGetProperty("date", out var date))
                        {
                            var s = date.GetString();
                            if (!string.IsNullOrEmpty(s) && s.Length >= 4)
                                r.Anio = int.TryParse(s.Substring(0, 4), out int y) ? y : 0;
                        }
                    }

                    // GÉNERO (opcional)
                    if (rec.TryGetProperty("tags", out var tags) &&
                        tags.GetArrayLength() > 0)
                    {
                        r.Genero = tags[0].GetProperty("name").GetString() ?? "";
                    }

                    resultados.Add(r);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error MusicBrainz: " + ex.Message);
            }

            return resultados;
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
}
