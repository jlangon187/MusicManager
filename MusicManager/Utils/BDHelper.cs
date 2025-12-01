using MySql.Data.MySqlClient;

namespace MusicManager.Utils
{
    public static class BDHelper
    {
        public static int ObtenerId(string tabla, string campo, string valor)
        {
            var cmd = Program.appMusic.LaConexion.CreateCommand();
            cmd.CommandText = $"SELECT id_{tabla} FROM {tabla} WHERE {campo}=@valor LIMIT 1";
            cmd.Parameters.AddWithValue("@valor", valor);

            object res = cmd.ExecuteScalar();
            return res == null ? -1 : (int)res;
        }

        public static int InsertarYObtenerId(string tabla, string campo, string valor)
        {
            var cmd = Program.appMusic.LaConexion.CreateCommand();
            cmd.CommandText = $"INSERT INTO {tabla} ({campo}) VALUES (@valor)";
            cmd.Parameters.AddWithValue("@valor", valor);
            cmd.ExecuteNonQuery();

            return (int)cmd.LastInsertedId;
        }

        public static int GetOrCreate(string tabla, string campo, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                valor = "Desconocido";

            int id = ObtenerId(tabla, campo, valor);
            if (id != -1) return id;

            return InsertarYObtenerId(tabla, campo, valor);
        }
    }
}
