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
        private static readonly HttpClient http = new HttpClient();                         // Cliente HTTP compartido

        public static string SpotifyClientId = "1c9d3a11c5284bb997ab40efbe4d4f0c";          // Credenciales de la aplicación Spotify  
        public static string SpotifyClientSecret = "ad0e63504a794a97939566b0fdf5b860";      // (usar las propias en producción)

        // Activar/desactivar fuentes
        public static bool UsarSpotify = true;
        public static bool UsarITunes = true;

        // Caching token Spotify
        private static string SpotifyToken = "";
        private static DateTime SpotifyTokenExpira = DateTime.MinValue;

        /// <summary>
        /// Constructor estático para inicializar el cliente HTTP.
        /// </summary>
        static MetadatosAPI()
        {
            http.DefaultRequestHeaders.UserAgent.Clear();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("MusicManager/1.0 (contacto@ejemplo.com)");
        }

        /// <summary>
        /// Método para probar la conexión a iTunes.
        /// </summary>
        /// <returns>Retorna true si la conexión es exitosa, false en caso contrario.</returns>
        public static async Task<bool> ProbarConexionITunes()
        {
            try
            {
                var r = await http.GetAsync("https://itunes.apple.com/search?term=test&limit=1");
                return r.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        /// <summary>
        /// Metodo para probar la conexión a Spotify.
        /// </summary>
        /// <returns>Retorna true si la conexión es exitosa, false en caso contrario.</returns>
        public static async Task<bool> ProbarConexionSpotify()
        {
            try
            {
                // Obtener token (si falla, Spotify está offline o credenciales malas)
                string token = await ObtenerTokenSpotify();

                // Hacemos una petición real
                var req = new HttpRequestMessage(
                    HttpMethod.Get,
                    "https://api.spotify.com/v1/search?q=test&type=track&limit=1"
                );

                req.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var resp = await http.SendAsync(req);
                return resp.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Método para obtener un token de acceso de Spotify.
        /// </summary>
        /// <returns>Retorna el token de acceso como una cadena.</returns>
        private static async Task<string> ObtenerTokenSpotify()
        {
            if (!string.IsNullOrEmpty(SpotifyToken) && DateTime.Now < SpotifyTokenExpira)
                return SpotifyToken;

            string auth = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{SpotifyClientId}:{SpotifyClientSecret}")
            );

            var req = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);
            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            });

            var resp = await http.SendAsync(req);
            string json = await resp.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            SpotifyToken = doc.RootElement.GetProperty("access_token").GetString();
            int expires = doc.RootElement.GetProperty("expires_in").GetInt32();

            SpotifyTokenExpira = DateTime.Now.AddSeconds(expires - 30);
            return SpotifyToken;
        }

        /// <summary>
        /// Metodo principal para buscar metadatos en las fuentes activadas.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="artista"></param>
        /// <returns>Retorna una lista de resultados de metadatos.</returns>
        public static async Task<List<MetadataResult>> BuscarLista(string titulo, string artista)
        {
            List<MetadataResult> resultados = new();

            // Spotify
            if (UsarSpotify)
            {
                var spot = await BuscarSpotify(titulo, artista);
                resultados.AddRange(spot);
            }

            // iTunes
            if (UsarITunes)
            {
                var itunes = await BuscarITunes(titulo, artista);
                resultados.AddRange(itunes);
            }

            // Quitar duplicados (título + artista)
            resultados = resultados
                .GroupBy(r => (r.Titulo.ToLower(), r.Artista.ToLower()))
                .Select(g => g.First())
                .ToList();

            return resultados;
        }

        /// <summary>
        /// Metodo para buscar metadatos en Spotify.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="artista"></param>
        /// <returns>Retorna una lista de resultados de metadatos.</returns>
        private static async Task<List<MetadataResult>> BuscarSpotify(string titulo, string artista)
        {
            List<MetadataResult> lista = new();

            try
            {
                string token = await ObtenerTokenSpotify();
                string q = Uri.EscapeDataString($"{titulo} {artista}");
                string url = $"https://api.spotify.com/v1/search?q={q}&type=track&limit=20";

                var req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var resp = await http.SendAsync(req);
                string json = await resp.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var tracks = doc.RootElement
                    .GetProperty("tracks")
                    .GetProperty("items");

                foreach (var item in tracks.EnumerateArray())
                {
                    string tituloReal = item.GetProperty("name").GetString();
                    string artistaReal = item.GetProperty("artists")[0].GetProperty("name").GetString();
                    string artistId = item.GetProperty("artists")[0].GetProperty("id").GetString();
                    string albumReal = item.GetProperty("album").GetProperty("name").GetString();

                    // portada
                    var imgs = item.GetProperty("album").GetProperty("images");
                    string portada = imgs.GetArrayLength() > 0
                        ? imgs[0].GetProperty("url").GetString()
                        : "";

                    // año
                    int anio = 0;
                    if (item.GetProperty("album").TryGetProperty("release_date", out var fecha))
                    {
                        string f = fecha.GetString();
                        if (f.Length >= 4) anio = int.Parse(f.Substring(0, 4));
                    }

                    // género
                    var generos = await ObtenerGenerosSpotify(artistId, token);
                    string generoFinal = generos.FirstOrDefault() ?? "";

                    lista.Add(new MetadataResult
                    {
                        Titulo = tituloReal,
                        Artista = artistaReal,
                        Album = albumReal,
                        Anio = anio,
                        Genero = generoFinal,
                        PortadaUrl = portada,
                        Fuente = "Spotify"
                    });
                }
            }
            catch { }

            return lista;
        }

        /// <summary>
        /// Metodo para obtener los géneros de un artista en Spotify ya que no vienen en la búsqueda de pistas.
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="token"></param>
        /// <returns>Retorna una lista de géneros.</returns>
        private static async Task<List<string>> ObtenerGenerosSpotify(string artistId, string token)
        {
            try
            {
                string url = $"https://api.spotify.com/v1/artists/{artistId}";

                var req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var resp = await http.SendAsync(req);
                string json = await resp.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var genres = doc.RootElement.GetProperty("genres");

                return genres.EnumerateArray().Select(g => g.GetString()).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Metodo para buscar metadatos en iTunes.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="artista"></param>
        /// <returns>Retorna una lista de resultados de metadatos.</returns>
        private static async Task<List<MetadataResult>> BuscarITunes(string titulo, string artista)
        {
            List<MetadataResult> lista = new();

            try
            {
                string q = Uri.EscapeDataString($"{titulo} {artista}");
                string url = $"https://itunes.apple.com/search?term={q}&entity=song&limit=20";

                var resp = await http.GetStringAsync(url);
                using var doc = JsonDocument.Parse(resp);

                var results = doc.RootElement.GetProperty("results");

                foreach (var item in results.EnumerateArray())
                {
                    string tituloReal = item.GetProperty("trackName").GetString();
                    string artistaReal = item.GetProperty("artistName").GetString();
                    string albumReal = item.TryGetProperty("collectionName", out var alb)
                        ? alb.GetString()
                        : "";

                    int anio = 0;
                    if (item.TryGetProperty("releaseDate", out var fecha))
                        anio = fecha.GetDateTime().Year;

                    // portada → convertir 100x100 a 600x600
                    string portada = item.TryGetProperty("artworkUrl100", out var art)
                        ? ConvertirCaratula(art.GetString(), 600)
                        : "";

                    string genero = item.TryGetProperty("primaryGenreName", out var gen)
                        ? gen.GetString()
                        : "";

                    lista.Add(new MetadataResult
                    {
                        Titulo = tituloReal,
                        Artista = artistaReal,
                        Album = albumReal,
                        Anio = anio,
                        Genero = genero,
                        PortadaUrl = portada,
                        Fuente = "iTunes"
                    });
                }
            }
            catch { }

            return lista;
        }

        /// <summary>
        /// Método para parsear una cadena de búsqueda en título y artista.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>Retorna una tupla con (título, artista).</returns>
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

        /// <summary>
        /// Metodo para convertir la URL de la carátula de iTunes a un tamaño específico.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="size"></param>
        /// <returns>Retorna la URL modificada.</returns>
        private static string ConvertirCaratula(string url, int size)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;
            return url.Replace("100x100", $"{size}x{size}");
        }

        /// <summary>
        /// Modelo para representar un resultado de metadatos.
        /// </summary>
        public class MetadataResult
        {
            public string Titulo { get; set; }
            public string Artista { get; set; }
            public string Album { get; set; }
            public int Anio { get; set; }
            public string Genero { get; set; }
            public string PortadaUrl { get; set; }
            public string Fuente { get; set; }

            public override string ToString()
            {
                return $"{Artista} - {Titulo} ({Album}, {Anio}) [{Genero}] ({Fuente})";
            }
        }
    }
}
