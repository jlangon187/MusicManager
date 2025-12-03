using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicManager.Utils
{
    public static class MetadatosAPI
    {
        private static readonly HttpClient http = new HttpClient();

        // =============================================
        // CONFIGURACIÓN GLOBAL DE API
        // =============================================

        private const string CONFIG_API_FILE = "config_api.json";

        public static bool UsarITunes { get; private set; } = true;


        // =====================================================
        //      Constructor estático → carga configuración
        // =====================================================
        static MetadatosAPI()
        {
            http.DefaultRequestHeaders.UserAgent.Clear();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("MusicManager/1.0 (contacto@ejemplo.com)");
            CargarConfiguracionAPIs();
        }

        // =====================================================
        //      CARGAR / GUARDAR CONFIGURACIÓN JSON
        // =====================================================
        private class ApiConfigJson
        {
            public bool usar_itunes { get; set; }
        }

        public static void CargarConfiguracionAPIs()
        {
            if (!File.Exists(CONFIG_API_FILE))
            {
                GuardarConfiguracionAPIs(); // crear archivo inicial
                return;
            }

            try
            {
                var json = File.ReadAllText(CONFIG_API_FILE);
                var cfg = JsonSerializer.Deserialize<ApiConfigJson>(json);

                if (cfg != null)
                {
                    UsarITunes = cfg.usar_itunes;
                }
            }
            catch
            {
                UsarITunes = true;
            }
        }

        public static void GuardarConfiguracionAPIs()
        {
            var cfg = new ApiConfigJson
            {
                usar_itunes = UsarITunes
            };

            File.WriteAllText(CONFIG_API_FILE,
                JsonSerializer.Serialize(cfg, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void ConfigurarUsoAPIs(bool itunes)
        {
            UsarITunes = itunes;
            GuardarConfiguracionAPIs();
        }

        // =====================================================
        //       PRUEBA DE CONEXIÓN ITUNES
        // =====================================================
        public static async Task<bool> ProbarConexionITunes()
        {
            try
            {
                var r = await http.GetAsync("https://itunes.apple.com/search?term=test&limit=1");
                return r.IsSuccessStatusCode;
            }
            catch { return false; }
        }


        // =====================================================
        //                SISTEMA HÍBRIDO
        // =====================================================
        public static async Task<MetadataResult> BuscarMetadatos(string titulo, string artista)
        {
            var lista = await BuscarLista(titulo, artista);
            return lista.FirstOrDefault();
        }

        public static async Task<List<MetadataResult>> BuscarLista(string titulo, string artista)
        {
            List<MetadataResult> total = new();

            if (UsarITunes)
            {
                var apple = await BuscarITunes(titulo, artista);
                if (apple.Count > 0)
                    total.AddRange(apple);
            }

            return OrdenarPorRelevancia(total, titulo, artista);
        }


        // =====================================================
        //                 API: ITUNES
        // =====================================================
        private static async Task<List<MetadataResult>> BuscarITunes(string titulo, string artista)
        {
            List<MetadataResult> lista = new();

            try
            {
                string q = $"{titulo} {artista}";
                string url =
                    $"https://itunes.apple.com/search?entity=song&limit=25&term={Uri.EscapeDataString(q)}";

                string json = await http.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("results", out var results))
                    return lista;

                foreach (var r in results.EnumerateArray())
                {
                    lista.Add(new MetadataResult
                    {
                        Titulo = r.TryGetProperty("trackName", out var tn) ? tn.GetString() : "",
                        Artista = r.TryGetProperty("artistName", out var ar) ? ar.GetString() : "",
                        Album = r.TryGetProperty("collectionName", out var al) ? al.GetString() : "",
                        Anio = r.TryGetProperty("releaseDate", out var d)
                            ? int.Parse(d.GetString().Substring(0, 4))
                            : 0,
                        Genero = r.TryGetProperty("primaryGenreName", out var g)
                            ? g.GetString()
                            : ""
                    });
                }
            }
            catch { }

            return lista;
        }


        // =====================================================
        //            PARSEAR BUSQUEDA
        // =====================================================
        public static (string titulo, string artista) ParsearBusqueda(string texto)
        {
            texto = texto.Trim();

            if (texto.Contains("-"))
            {
                var partes = texto.Split('-', 2);
                string izquierda = partes[0].Trim();
                string derecha = partes[1].Trim();

                if (derecha.ToLower().Contains("feat") || derecha.Contains("&"))
                    return (izquierda, derecha);

                if (char.IsUpper(izquierda[0]) && !char.IsUpper(derecha[0]))
                    return (derecha, izquierda);

                if (char.IsUpper(derecha[0]))
                    return (derecha, izquierda);

                return (derecha, izquierda);
            }

            return (texto, "");
        }


        // =====================================================
        //            ORDENACIÓN POR RELEVANCIA
        // =====================================================
        private static string Normalizar(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            return s.ToLower()
                .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                .Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n");
        }

        private static List<MetadataResult> OrdenarPorRelevancia(
            List<MetadataResult> lista, string titulo, string artista)
        {
            string t = Normalizar(titulo);
            string a = Normalizar(artista);

            return lista
                .OrderByDescending(r => Normalizar(r.Artista) == a)
                .ThenByDescending(r => Normalizar(r.Titulo) == t)
                .ThenByDescending(r => Normalizar(r.Titulo).Contains(t))
                .ToList();
        }


        // =====================================================
        //                MODELO RESULTADO
        // =====================================================
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
