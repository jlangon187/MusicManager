using MusicManager.Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagLib;
using File = System.IO.File;

namespace MusicManager.Data
{
    public class GestorMusica
    {
        private readonly MySqlConnection conexion;          // Conexión a la base de datos MySQL
        private readonly Tabla tabla;                       // Instancia de la clase Tabla para operaciones comunes

        /// <summary>
        /// Constructor de la clase GestorMusica.
        /// </summary>
        /// <param name="conexion"></param>
        public GestorMusica(MySqlConnection conexion)
        {
            this.conexion = conexion;
            this.tabla = new Tabla(conexion);
        }

        /// <summary>
        /// Metodo que normaliza un texto para comparaciones.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Retorna el texto normalizado.</returns>
        public static string NormalizarTexto(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return "desconocido";

            return s.Trim()
                    .ToLowerInvariant();
        }

        /// <summary>
        /// Metodo que normaliza una ruta de archivo para comparaciones.
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns>Retorna la ruta normalizada.</returns>
        public string NormalizarRuta(string ruta)
        {
            return Path.GetFullPath(ruta)
                .Replace('/', '\\')
                .Trim()
                .ToLowerInvariant();
        }

        /// <summary>
        /// Metodo que obtiene o crea un artista por su nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>Retorna el ID del artista.</returns>
        public int GetOrCreateArtista(string nombre)
        {
            string normal = NormalizarTexto(nombre);

            int id = ObtenerIdCaseInsensitive("artista", "nombre", normal);
            if (id != -1) return id;

            // Guardar el nombre REAL, NO normalizado
            return tabla.Insertar("artista", ("nombre", nombre.Trim()));
        }

        /// <summary>
        /// Metodo que obtiene o crea un género por su nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>Retorna el ID del género.</returns>
        public int GetOrCreateGenero(string nombre)
        {
            nombre = NormalizarTexto(nombre);

            int id = ObtenerIdCaseInsensitive("genero", "nombre", nombre);
            if (id != -1)
                return id;

            return tabla.Insertar("genero", ("nombre", nombre));
        }

        /// <summary>
        /// Metodo que obtiene o crea un álbum por su título.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="idArtista"></param>
        /// <param name="anio"></param>
        /// <param name="portadaUrl"></param>
        /// <returns>Retorna el ID del álbum.</returns>
        public int GetOrCreateAlbum(string titulo, int idArtista, int anio, string portadaUrl = null)
        {
            titulo = NormalizarTexto(titulo);

            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                @"SELECT id_album
          FROM album
          WHERE LOWER(titulo)=LOWER(@t)
          LIMIT 1";

            cmd.Parameters.AddWithValue("@t", titulo);

            object res = cmd.ExecuteScalar();

            if (res != null)
            {
                int idAlbum = Convert.ToInt32(res);

                // Actualizar portada solo si viene una URL válida y el álbum no la tiene
                if (!string.IsNullOrWhiteSpace(portadaUrl))
                {
                    var update = conexion.CreateCommand();
                    update.CommandText =
                        @"UPDATE album
                  SET portada=@p
                  WHERE id_album=@id AND (portada IS NULL OR portada = '')";

                    update.Parameters.AddWithValue("@p", portadaUrl);
                    update.Parameters.AddWithValue("@id", idAlbum);
                    update.ExecuteNonQuery();
                }

                return idAlbum;
            }

            // Insertar nuevo álbum
            return tabla.Insertar("album",
                ("titulo", titulo),
                ("anio_lanzamiento", anio),
                ("id_artista", idArtista),
                ("portada", portadaUrl ?? "")
            );
        }

        /// <summary>
        /// Metodo que obtiene el ID de una canción por su ruta de archivo.
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns>Retorna el ID de la canción o null si no existe.</returns>
        public int? GetCancionPorRuta(string ruta)
        {
            string rutaNorm = NormalizarRuta(ruta);

            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                @"SELECT id_cancion 
                  FROM cancion
                  WHERE ruta_archivo=@ruta
                  LIMIT 1";

            cmd.Parameters.AddWithValue("@ruta", rutaNorm);

            object res = cmd.ExecuteScalar();
            if (res == null)
                return null;

            return Convert.ToInt32(res);
        }

        /// <summary>
        /// Metodo que obtiene una canción completa por su ruta de archivo.
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns>Retorna la canción o null si no existe.</returns>
        public Cancion ObtenerCancionPorRuta(string ruta)
        {
            ruta = NormalizarRuta(ruta);

            var cmd = conexion.CreateCommand();
            cmd.CommandText = @"
        SELECT id_cancion, titulo, duracion, id_artista, id_album, id_genero, anio, ruta_archivo
        FROM cancion
        WHERE ruta_archivo=@ruta
        LIMIT 1";

            cmd.Parameters.AddWithValue("@ruta", ruta);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Cancion
                {
                    id_cancion = reader.GetInt32("id_cancion"),
                    titulo = reader.GetString("titulo"),
                    duracion = reader.GetString("duracion"),
                    id_artista = reader.GetInt32("id_artista"),
                    id_album = reader.GetInt32("id_album"),
                    id_genero = reader.GetInt32("id_genero"),
                    anio = reader.GetInt32("anio"),
                    ruta_archivo = reader.GetString("ruta_archivo")
                };
            }

            return null;
        }

        /// <summary>
        /// Metodo que obtiene la portada de un álbum por su ID.
        /// </summary>
        /// <param name="idAlbum"></param>
        /// <returns>Retorna la URL de la portada del álbum.</returns>
        public string ObtenerPortadaAlbum(int idAlbum)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT portada FROM album WHERE id_album=@id LIMIT 1";
            cmd.Parameters.AddWithValue("@id", idAlbum);

            object res = cmd.ExecuteScalar();
            return res?.ToString();
        }

        /// <summary>
        /// Metodo que inserta una nueva canción en la base de datos.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Retorna el ID de la canción insertada.</returns>
        public int InsertarCancion(Cancion c)
        {
            return tabla.Insertar("cancion",
                ("titulo", c.titulo.Trim()),
                ("duracion", c.duracion),
                ("id_artista", c.id_artista),
                ("id_album", c.id_album),
                ("id_genero", c.id_genero),
                ("anio", c.anio),
                ("ruta_archivo", NormalizarRuta(c.ruta_archivo))
            );
        }

        /// <summary>
        /// Metodo que actualiza los datos de una canción existente.
        /// </summary>
        /// <param name="c"></param>
        public void ActualizarCancion(Cancion c)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                @"UPDATE cancion SET
                    titulo=@titulo,
                    duracion=@duracion,
                    id_artista=@id_artista,
                    id_album=@id_album,
                    id_genero=@id_genero,
                    anio=@anio,
                    ultima_actualizacion=NOW()
                  WHERE id_cancion=@id";

            cmd.Parameters.AddWithValue("@titulo", NormalizarTexto(c.titulo));
            cmd.Parameters.AddWithValue("@duracion", c.duracion);
            cmd.Parameters.AddWithValue("@id_artista", c.id_artista);
            cmd.Parameters.AddWithValue("@id_album", c.id_album);
            cmd.Parameters.AddWithValue("@id_genero", c.id_genero);
            cmd.Parameters.AddWithValue("@anio", c.anio);
            cmd.Parameters.AddWithValue("@id", c.id_cancion);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Metodo que actualiza la ruta de archivo de una canción.
        /// </summary>
        /// <param name="rutaAntigua"></param>
        /// <param name="rutaNueva"></param>
        public void ActualizarRutaCancion(string rutaAntigua, string rutaNueva)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                @"UPDATE cancion 
                  SET ruta_archivo=@nueva, ultima_actualizacion=NOW()
                  WHERE ruta_archivo=@antigua";

            cmd.Parameters.AddWithValue("@nueva", NormalizarRuta(rutaNueva));
            cmd.Parameters.AddWithValue("@antigua", NormalizarRuta(rutaAntigua));

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Metodo que obtiene todas las rutas de archivo de las canciones.
        /// </summary>
        /// <returns>Retorna una lista de rutas de archivo.</returns>
        public List<string> GetTodasLasRutas()
        {
            List<string> rutas = new();

            var cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT ruta_archivo FROM cancion";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rutas.Add(reader.GetString("ruta_archivo"));
            }

            return rutas;
        }

        /// <summary>
        /// Metodo que elimina una canción por su ruta de archivo.
        /// </summary>
        /// <param name="ruta"></param>
        public void EliminarCancionPorRuta(string ruta)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText = "DELETE FROM cancion WHERE ruta_archivo=@ruta";

            cmd.Parameters.AddWithValue("@ruta", NormalizarRuta(ruta));
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Metodo que inserta una canción en la base de datos leyendo sus metadatos desde el archivo.
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="portadaUrl"></param>
        public void InsertarCancionDesdeArchivo(string ruta, string portadaUrl = "")
        {
            if (!File.Exists(ruta))
                return;

            ruta = NormalizarRuta(ruta);

            TagLib.File tagFile = null;

            try { tagFile = TagLib.File.Create(ruta); } catch { }

            string titulo = tagFile?.Tag.Title?.Trim() ?? Path.GetFileNameWithoutExtension(ruta);
            string artista = tagFile?.Tag.FirstPerformer?.Trim() ?? "Desconocido";
            string album = tagFile?.Tag.Album?.Trim() ?? "Desconocido";
            string genero = tagFile?.Tag.FirstGenre?.Trim() ?? "Desconocido";

            int anio = GetYearSeguro(tagFile);

            string duracion = "00:00";
            try
            {
                if (tagFile != null)
                {
                    var dur = tagFile.Properties.Duration;
                    duracion = $"{(int)dur.TotalMinutes:00}:{dur.Seconds:00}";
                }
            }
            catch { }

            int idArtista = GetOrCreateArtista(artista);
            int idGenero = GetOrCreateGenero(genero);

            int idAlbum = GetOrCreateAlbum(album, idArtista, anio, portadaUrl);

            InsertarCancion(new Cancion
            {
                titulo = titulo,
                duracion = duracion,
                id_artista = idArtista,
                id_album = idAlbum,
                id_genero = idGenero,
                anio = anio,
                ruta_archivo = ruta
            });
        }

        /// <summary>
        /// Metodo que obtiene el ID de un registro en una tabla de forma case insensitive.
        /// </summary>
        /// <param name="tablaNombre"></param>
        /// <param name="campo"></param>
        /// <param name="valor"></param>
        /// <returns>Retorna el ID del registro o -1 si no existe.</returns>
        private int ObtenerIdCaseInsensitive(string tablaNombre, string campo, string valor)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                $"SELECT id_{tablaNombre} FROM {tablaNombre} WHERE LOWER({campo})=LOWER(@v) LIMIT 1";

            cmd.Parameters.AddWithValue("@v", valor);

            object res = cmd.ExecuteScalar();
            return (res == null) ? -1 : Convert.ToInt32(res);
        }

        /// <summary>
        /// Metodo que obtiene el año de una canción de forma segura.
        /// </summary>
        /// <param name="tagFile"></param>
        /// <returns>Retorna el año o 0 si no se encuentra.</returns>
        private int GetYearSeguro(TagLib.File tagFile)
        {
            if (tagFile == null)
                return 0;

            if (tagFile.Tag.Year > 0)
                return (int)tagFile.Tag.Year;

            string[] posibles = {
                tagFile.Tag.Comment,
                tagFile.Tag.Title,
                tagFile.Tag.Album
            };

            foreach (var txt in posibles)
            {
                if (string.IsNullOrWhiteSpace(txt)) continue;

                var match = Regex.Match(txt, @"\b(19|20)\d{2}\b");
                if (match.Success)
                    return int.Parse(match.Value);
            }

            return 0;
        }

        /// <summary>
        /// Metodo que actualiza los metadatos de un archivo de música.
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="titulo"></param>
        /// <param name="artista"></param>
        /// <param name="album"></param>
        /// <param name="genero"></param>
        /// <param name="anio"></param>
        public void ActualizarMetadatosEnArchivo(string ruta, string titulo, string artista, string album, string genero, int anio)
        {
            try
            {
                if (!File.Exists(ruta))
                    return;

                var file = TagLib.File.Create(ruta);

                if (file.Tag == null)
                    return;

                file.Tag.Title = titulo ?? "";
                file.Tag.Performers = new[] { artista ?? "" };
                file.Tag.Album = album ?? "";
                file.Tag.Genres = new[] { genero ?? "" };

                if (anio > 0)
                    file.Tag.Year = (uint)anio;
                else
                    file.Tag.Year = 0;

                file.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error actualizando metadatos del archivo: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo que elimina los registros huérfanos de álbumes, artistas y géneros.
        /// </summary>
        public void LimpiarHuerfanos()
        {
            var cmd = conexion.CreateCommand();

            // Álbumes
            cmd.CommandText = "DELETE FROM album WHERE id_album NOT IN (SELECT id_album FROM cancion)";
            cmd.ExecuteNonQuery();

            // Artistas
            cmd.CommandText = "DELETE FROM artista WHERE id_artista NOT IN (SELECT id_artista FROM cancion)";
            cmd.ExecuteNonQuery();

            // Géneros
            cmd.CommandText = "DELETE FROM genero WHERE id_genero NOT IN (SELECT id_genero FROM cancion)";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Metodo que obtiene todos los géneros disponibles.
        /// </summary>
        /// <returns>Retorna una lista de géneros.</returns>
        internal IEnumerable<object> ObtenerTodosGeneros()
        {
            var generos = new List<string>();
            var cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT nombre FROM genero ORDER BY nombre ASC";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                generos.Add(reader.GetString("nombre"));
            }
            return generos;
        }
    }
}
