using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MusicManager.Data
{
    public class Tabla
    {
        private MySqlConnection conexion;

        public DataTable LaTabla { get; private set; }

        public Tabla(MySqlConnection conexion)
        {
            this.conexion = conexion;
            LaTabla = new DataTable();
        }

        /// <summary>
        /// Carga datos desde una sentencia SELECT.
        /// </summary>
        public void InicializarDatos(string sql)
        {
            LaTabla.Clear();

            using var cmd = new MySqlCommand(sql, conexion);
            using var da = new MySqlDataAdapter(cmd);
            da.Fill(LaTabla);
        }

        /// <summary>
        /// Filtrado sobre los datos ya cargados.
        /// </summary>
        public DataView Filtrar(string condicion)
        {
            DataView vista = new DataView(LaTabla);
            vista.RowFilter = condicion;
            return vista;
        }

        /// <summary>
        /// Inserta un registro en una tabla (INSERT simple).
        /// diccionario: { "nombre", "Queen" }, { "pais", "UK" }
        /// </summary>
        public int Insertar(string tabla, params (string columna, object valor)[] datos)
        {
            string columnas = "";
            string valores = "";

            var cmd = new MySqlCommand();
            cmd.Connection = conexion;

            for (int i = 0; i < datos.Length; i++)
            {
                columnas += datos[i].columna;
                valores += "@" + datos[i].columna;

                cmd.Parameters.AddWithValue("@" + datos[i].columna, datos[i].valor);

                if (i < datos.Length - 1)
                {
                    columnas += ", ";
                    valores += ", ";
                }
            }

            cmd.CommandText = $"INSERT INTO {tabla} ({columnas}) VALUES ({valores})";
            cmd.ExecuteNonQuery();

            return (int)cmd.LastInsertedId;
        }

        /// <summary>
        /// Actualiza un registro por ID.
        /// </summary>
        public void Actualizar(string tabla, string idCampo, int id, params (string columna, object valor)[] datos)
        {
            string set = "";

            var cmd = new MySqlCommand();
            cmd.Connection = conexion;

            for (int i = 0; i < datos.Length; i++)
            {
                set += $"{datos[i].columna} = @{datos[i].columna}";
                cmd.Parameters.AddWithValue("@" + datos[i].columna, datos[i].valor);

                if (i < datos.Length - 1)
                    set += ", ";
            }

            cmd.Parameters.AddWithValue("@id", id);

            cmd.CommandText = $"UPDATE {tabla} SET {set} WHERE {idCampo}=@id";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Elimina un registro por ID.
        /// </summary>
        public void Eliminar(string tabla, string idCampo, int id)
        {
            var cmd = new MySqlCommand(
                $"DELETE FROM {tabla} WHERE {idCampo}=@id",
                conexion
            );
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
