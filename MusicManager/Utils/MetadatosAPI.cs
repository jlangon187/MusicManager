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

        static MetadatosAPI()
        {
            http.DefaultRequestHeaders.UserAgent.Clear();
            http.DefaultRequestHeaders.UserAgent.ParseAdd(
                "MusicManager/1.0 (contacto@ejemplo.com)"
            );
        }

        public static async Task<MetadataResult> BuscarMetadatos(string titulo, string artista)
        {
            var lista = await BuscarLista(titulo, artista);
            if (lista.Count > 0)
                return lista[0];
            return null;
        }

        public static async Task<List<MetadataResult>> BuscarLista(string titulo, string artista)
        {
            List<MetadataResult> resultados = new();

            string exactQuery = Uri.EscapeDataString(
                $"artist:\"{artista}\" AND recording:\"{titulo}\""
            );

            string urlExact =
                $"https://musicbrainz.org/ws/2/recording/?query={exactQuery}&fmt=json";

            resultados.AddRange(await EjecutarConsulta(urlExact));

            if (resultados.Count > 0)
                return OrdenarPorRelevancia(resultados, titulo, artista);

            string fuzzyQuery = Uri.EscapeDataString($"{titulo} {artista}");
            string urlFuzzy =
                $"https://musicbrainz.org/ws/2/recording/?query={fuzzyQuery}&fmt=json";

            resultados.AddRange(await EjecutarConsulta(urlFuzzy));

            resultados = resultados
                .Where(r => Normalizar(r.Artista).Contains(Normalizar(artista)))
                .ToList();

            return OrdenarPorRelevancia(resultados, titulo, artista);
        }

        private static async Task<List<MetadataResult>> EjecutarConsulta(string url)
        {
            List<MetadataResult> lista = new();

            try
            {
                var response = await http.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return lista;

                string json = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(json);
                if (!doc.RootElement.TryGetProperty("recordings", out JsonElement recordings))
                    return lista;

                foreach (var rec in recordings.EnumerateArray())
                {
                    var r = new MetadataResult();

                    r.Titulo = rec.GetProperty("title").GetString() ?? "";

                    if (rec.TryGetProperty("artist-credit", out var ac) &&
                        ac.GetArrayLength() > 0)
                    {
                        r.Artista = ac[0]
                            .GetProperty("artist")
                            .GetProperty("name")
                            .GetString() ?? "";
                    }

                    if (rec.TryGetProperty("releases", out var rels) &&
                        rels.GetArrayLength() > 0)
                    {
                        r.Album = rels[0].GetProperty("title").GetString() ?? "";

                        if (rels[0].TryGetProperty("date", out var date))
                        {
                            string ds = date.GetString();
                            if (!string.IsNullOrEmpty(ds) && ds.Length >= 4)
                                r.Anio = int.TryParse(ds.Substring(0, 4), out int y) ? y : 0;
                        }
                    }

                    if (rec.TryGetProperty("tags", out var tags) &&
                        tags.GetArrayLength() > 0)
                    {
                        r.Genero = tags[0].GetProperty("name").GetString() ?? "";
                    }

                    lista.Add(r);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error MusicBrainz: " + ex.Message);
            }

            return lista;
        }

        public static (string titulo, string artista) ParsearBusqueda(string texto)
        {
            texto = texto.Trim();

            if (texto.Contains("-"))
            {
                var partes = texto.Split('-', 2);

                string izquierda = partes[0].Trim();
                string derecha = partes[1].Trim();

                // Si la derecha contiene palabras típicas de artista ("feat", "&", etc.)
                if (derecha.ToLower().Contains("feat") || derecha.Contains("&"))
                    return (izquierda, derecha);

                // Regla general: "Artista - Título"
                // Artista suele ser la primera palabra y empieza por mayúscula más a menudo
                if (char.IsUpper(izquierda[0]) && !char.IsUpper(derecha[0]))
                    return (derecha, izquierda);

                // Si la derecha ES un título típico (más de 1 palabra, o empieza por mayúscula)
                if (char.IsUpper(derecha[0]))
                    return (derecha, izquierda);

                return (derecha, izquierda);
            }

            // Si no hay "-", se trata como solo título
            return (texto, "");
        }


        private static string Normalizar(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            s = s.ToLower();
            s = s.Replace("á", "a").Replace("é", "e").Replace("í", "i")
                 .Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n")
                 .Replace("ö", "o").Replace("ä", "a").Replace("ë", "e");
            return s;
        }

        private static List<MetadataResult> OrdenarPorRelevancia(
            List<MetadataResult> lista, string titulo, string artista)
        {
            string t = Normalizar(titulo);
            string a = Normalizar(artista);

            return lista
                .OrderByDescending(r => Normalizar(r.Artista) == a)
                .ThenByDescending(r => Normalizar(r.Titulo).Contains(t))
                .ThenByDescending(r => Normalizar(r.Titulo) == t)
                .ThenBy(r => r.Anio == 0 ? 9999 : r.Anio)
                .ToList();
        }


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
