using Modelos;
using MySql.Data.MySqlClient;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MusicManager.Data
{
    public class GestorMusica
    {
        private MySqlConnection conexion;
        private Tabla tabla;

        public GestorMusica(MySqlConnection conexion)
        {
            this.conexion = conexion;
            tabla = new Tabla(conexion);
        }

        public int GetOrCreateArtista(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                nombre = "Desconocido";

            int id = ObtenerId("artista", "nombre", nombre);
            if (id != -1) return id;

            return tabla.Insertar("artista",
                ("nombre", nombre)
            );
        }

        public int GetOrCreateGenero(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                nombre = "Desconocido";

            int id = ObtenerId("genero", "nombre", nombre);
            if (id != -1) return id;

            return tabla.Insertar("genero",
                ("nombre", nombre)
            );
        }

        public int GetOrCreateAlbum(string titulo, int idArtista, int anio)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                titulo = "Desconocido";

            // Buscar si existe ya el álbum del mismo artista
            var cmd = conexion.CreateCommand();
            cmd.CommandText =
                "SELECT id_album FROM album WHERE titulo=@t AND id_artista=@a LIMIT 1";
            cmd.Parameters.AddWithValue("@t", titulo);
            cmd.Parameters.AddWithValue("@a", idArtista);

            object res = cmd.ExecuteScalar();
            if (res != null)
                return Convert.ToInt32(res);

            // Insertar si no existe
            return tabla.Insertar("album",
                ("titulo", titulo),
                ("anio_lanzamiento", anio),
                ("id_artista", idArtista)
            );
        }

        public int InsertarCancion(Cancion c)
        {
            return tabla.Insertar("cancion",
                ("titulo", c.titulo),
                ("ruta", c.ruta),
                ("id_album", c.id_album),
                ("id_genero", c.id_genero),
                ("anio", c.anio)
            );
        }

        private int ObtenerId(string tablaNombre, string campo, string valor)
        {
            var cmd = conexion.CreateCommand();
            cmd.CommandText = $"SELECT id_{tablaNombre} FROM {tablaNombre} WHERE {campo}=@v LIMIT 1";
            cmd.Parameters.AddWithValue("@v", valor);

            object res = cmd.ExecuteScalar();
            return res == null ? -1 : Convert.ToInt32(res);
        }
    }
}
