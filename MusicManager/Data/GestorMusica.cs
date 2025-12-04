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
        private readonly MySqlConnection conexion;
        private readonly Tabla tabla;

        public GestorMusica(MySqlConnection conexion)
        {
            this.conexion = conexion;
            this.tabla = new Tabla(conexion);
        }

        // =====================================================================
        // NORMALIZADORES
        // =====================================================================

        public static string NormalizarTexto(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return "desconocido";

            return s.Trim()
                    .ToLowerInvariant();
        }

        public string NormalizarRuta(string ruta)
        {
            return Path.GetFullPath(ruta)
                .Replace('/', '\\')
                .Trim()
                .ToLowerInvariant();
        }

        // =====================================================================
        // ARTISTA
        // =====================================================================

        public int GetOrCreateArtista(string nombre)
        {
            string normal = NormalizarTexto(nombre);

            int id = ObtenerIdCaseInsensitive("artista", "nombre", normal);
            if (id != -1) return id;

            // Guardar el nombre REAL, NO normalizado
            return tabla.Insertar("artista", ("nombre", nombre.Trim()));
        }

        // =====================================================================
        // GÉNERO
        // =====================================================================

        public int GetOrCreateGenero(string nombre)
        {
            nombre = NormalizarTexto(nombre);

            int id = ObtenerIdCaseInsensitive("genero", "nombre", nombre);
            if (id != -1)
                return id;

            return tabla.Insertar("genero", ("nombre", nombre));
        }

        // =====================================================================
        // ÁLBUM
        // =====================================================================

        public int GetOrCreateAlbum(string titulo, int idArtista, int anio)
        {
            titulo = NormalizarTexto(titulo);

            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                @"SELECT id_album 
                  FROM album 
                  WHERE LOWER(titulo)=LOWER(@t) AND id_artista=@a
                  LIMIT 1";

            cmd.Parameters.AddWithValue("@t", titulo);
            cmd.Parameters.AddWithValue("@a", idArtista);

            object res = cmd.ExecuteScalar();
            if (res != null)
                return Convert.ToInt32(res);

            return tabla.Insertar("album",
                ("titulo", titulo),
                ("anio_lanzamiento", anio),
                ("id_artista", idArtista));
        }

        // =====================================================================
        // CANCIONES
        // =====================================================================

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

        // =====================================================================
        // SINCRONIZACIÓN
        // =====================================================================



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

        public void EliminarCancionPorRuta(string ruta)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText = "DELETE FROM cancion WHERE ruta_archivo=@ruta";

            cmd.Parameters.AddWithValue("@ruta", NormalizarRuta(ruta));
            cmd.ExecuteNonQuery();
        }

        public void InsertarCancionDesdeArchivo(string ruta)
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
            int idAlbum = GetOrCreateAlbum(album, idArtista, anio);

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

        // =====================================================================
        // UTILIDADES
        // =====================================================================

        private int ObtenerIdCaseInsensitive(string tablaNombre, string campo, string valor)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                $"SELECT id_{tablaNombre} FROM {tablaNombre} WHERE LOWER({campo})=LOWER(@v) LIMIT 1";

            cmd.Parameters.AddWithValue("@v", valor);

            object res = cmd.ExecuteScalar();
            return (res == null) ? -1 : Convert.ToInt32(res);
        }

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

        // =====================================================================
        // METADATOS EN ARCHIVO
        // =====================================================================

        public void ActualizarMetadatosEnArchivo(string ruta, string titulo, string artista, string album, string genero, int anio)
        {
            try
            {
                if (!File.Exists(ruta))
                    return;

                var file = TagLib.File.Create(ruta);

                if (file.Tag == null)
                    return;

                // --- Actualizar etiquetas ---
                file.Tag.Title = titulo ?? "";
                file.Tag.Performers = new[] { artista ?? "" };
                file.Tag.Album = album ?? "";
                file.Tag.Genres = new[] { genero ?? "" };

                if (anio > 0)
                    file.Tag.Year = (uint)anio;
                else
                    file.Tag.Year = 0;

                // --- Guardar cambios ---
                file.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error actualizando metadatos del archivo: " + ex.Message);
            }
        }


        // =====================================================================
        // LIMPIEZA DE HUÉRFANOS
        // =====================================================================

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
    }
}
