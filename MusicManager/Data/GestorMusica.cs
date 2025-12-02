using MusicManager.Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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

        // =============================================================
        //  ARTISTA
        // =============================================================
        public int GetOrCreateArtista(string nombre)
        {
            nombre = string.IsNullOrWhiteSpace(nombre) ? "Desconocido" : nombre.Trim();

            int id = ObtenerId("artista", "nombre", nombre);
            if (id != -1)
                return id;

            return tabla.Insertar("artista", ("nombre", nombre));
        }

        // =============================================================
        //  GENERO
        // =============================================================
        public int GetOrCreateGenero(string nombre)
        {
            nombre = string.IsNullOrWhiteSpace(nombre) ? "Desconocido" : nombre.Trim();

            int id = ObtenerId("genero", "nombre", nombre);
            if (id != -1)
                return id;

            return tabla.Insertar("genero", ("nombre", nombre));
        }

        // =============================================================
        //  ALBUM
        // =============================================================
        public int GetOrCreateAlbum(string titulo, int idArtista, int anio)
        {
            titulo = string.IsNullOrWhiteSpace(titulo) ? "Desconocido" : titulo.Trim();

            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                "SELECT id_album FROM album WHERE titulo=@t AND id_artista=@a LIMIT 1";

            cmd.Parameters.AddWithValue("@t", titulo);
            cmd.Parameters.AddWithValue("@a", idArtista);

            object res = cmd.ExecuteScalar();

            if (res != null)
                return Convert.ToInt32(res);

            return tabla.Insertar("album",
                ("titulo", titulo),
                ("anio_lanzamiento", anio),
                ("id_artista", idArtista)
            );
        }

        // =============================================================
        //  CANCIONES
        // =============================================================

        public int? GetCancionPorRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return null;

            string rutaNorm = NormalizarRuta(ruta);

            var cmd = conexion.CreateCommand();
            cmd.CommandText = "SELECT id_cancion, ruta_archivo FROM cancion";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string rutaBD = reader.GetString("ruta_archivo");

                if (NormalizarRuta(rutaBD) == rutaNorm)
                    return reader.GetInt32("id_cancion");
            }

            return null;
        }

        public int InsertarCancion(Cancion c)
        {

            return tabla.Insertar("cancion",
                ("titulo", c.titulo),
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

            cmd.Parameters.AddWithValue("@titulo", c.titulo);
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

        // =============================================================
        //  SINCRONIZACIÓN
        // =============================================================

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
            cmd.Parameters.AddWithValue("@ruta", ruta);
            cmd.ExecuteNonQuery();
        }

        public void InsertarCancionDesdeArchivo(string ruta)
        {
            if (!File.Exists(ruta))
                return;

            ruta = Path.GetFullPath(ruta);

            TagLib.File tagFile = null;
            try
            {
                tagFile = TagLib.File.Create(ruta);
            }
            catch
            {
                tagFile = null; // TagLib falló
            }

            // --- CAMPOS BASE ---
            string titulo = tagFile?.Tag.Title ?? Path.GetFileNameWithoutExtension(ruta);
            string artista = tagFile?.Tag.FirstPerformer ?? "Desconocido";
            string album = tagFile?.Tag.Album ?? "Desconocido";
            string genero = tagFile?.Tag.FirstGenre ?? "Desconocido";

            // --- AÑO SEGURO ---
            int anio = 0;

            try
            {
                if (tagFile != null && tagFile.Tag.Year > 0)
                {
                    anio = (int)tagFile.Tag.Year;
                }
                else
                {
                    // Intentar extraer año de texto usando Regex
                    string[] posibles = {
                tagFile?.Tag.Comment,
                tagFile?.Tag.Title,
                tagFile?.Tag.Album
            };

                    foreach (var txt in posibles)
                    {
                        if (string.IsNullOrWhiteSpace(txt)) continue;

                        var m = System.Text.RegularExpressions.Regex.Match(txt, @"\b(19|20)\d{2}\b");
                        if (m.Success)
                        {
                            anio = int.Parse(m.Value);
                            break;
                        }
                    }
                }
            }
            catch
            {
                anio = 0;
            }

            // --- DURACIÓN SEGURA ---
            string duracion = "00:00";
            try
            {
                if (tagFile != null)
                {
                    var dur = tagFile.Properties.Duration;
                    duracion = $"{(int)dur.TotalMinutes:00}:{dur.Seconds:00}";
                }
            }
            catch
            {
                duracion = "00:00";
            }

            // --- RELACIONES ---
            int idArtista = GetOrCreateArtista(artista);
            int idGenero = GetOrCreateGenero(genero);

            // Álbum seguro (evita duplicar si año = 0)
            int idAlbum = GetOrCreateAlbum(album, idArtista, anio);

            // --- INSERTAR CANCIÓN ---
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


        // =============================================================
        //  UTILIDADES
        // =============================================================

        private int ObtenerId(string tablaNombre, string campo, string valor)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText = $"SELECT id_{tablaNombre} FROM {tablaNombre} WHERE {campo}=@v LIMIT 1";
            cmd.Parameters.AddWithValue("@v", valor);

            object res = cmd.ExecuteScalar();
            return (res == null) ? -1 : Convert.ToInt32(res);
        }

        private int GetYearSeguro(TagLib.File tagFile)
        {
            // 1. Si TagLib ya proporciona año válido, usarlo
            if (tagFile.Tag.Year > 0)
                return (int)tagFile.Tag.Year;

            // 2. Intentar leer "date" o "year" desde Taglib como texto
            // Algunas versiones almacenan fechas como string dentro de Tag.Comment
            string[] posiblesFechas = {
                tagFile.Tag.Comment,
                tagFile.Tag.Title,
                tagFile.Tag.Album
            };

            foreach (var texto in posiblesFechas)
            {
                if (string.IsNullOrWhiteSpace(texto))
                    continue;

                // Buscar cualquier grupo de 4 números seguidos → año
                var match = System.Text.RegularExpressions.Regex.Match(texto, @"\b(19|20)\d{2}\b");
                if (match.Success)
                    return int.Parse(match.Value);
            }

            // 3. Si no se encuentra año, devolver 0
            return 0;
        }
        public string NormalizarRuta(string ruta)
        {
            return Path.GetFullPath(ruta)
                .Replace('/', '\\')
                .Trim()
                .ToLowerInvariant();
        }
    }
}
