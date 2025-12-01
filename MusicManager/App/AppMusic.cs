using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace MusicManager
{
    public class AppMusic
    {
        public MySqlConnection LaConexion { get; private set; }
        public bool Conectado => LaConexion != null && LaConexion.State == System.Data.ConnectionState.Open;

        private string servidor;
        private int puerto;
        private string usuario;
        private string password;
        private string baseDatos;

        private readonly string rutaLog = "musicmanager_log.txt";

        public AppMusic(string servidor, int puerto, string usuario, string password, string baseDatos)
        {
            this.servidor = servidor;
            this.puerto = puerto;
            this.usuario = usuario;
            this.password = password;
            this.baseDatos = baseDatos;
        }

        /// <summary>
        /// Construye la cadena de conexión y conecta a MySQL.
        /// </summary>
        public bool Conectar()
        {
            try
            {
                string cadena =
                    $"server={servidor};" +
                    $"port={puerto};" +
                    $"user={usuario};" +
                    $"password={password};" +
                    $"database={baseDatos};" +
                    $"Allow User Variables=True;";

                LaConexion = new MySqlConnection(cadena);
                LaConexion.Open();

                RegistrarLog("Conectar", "Conexión establecida correctamente.");

                return true;
            }
            catch (Exception ex)
            {
                RegistrarLog("Conectar", "ERROR: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Desconectar de manera segura.
        /// </summary>
        public void Desconectar()
        {
            if (LaConexion != null && LaConexion.State != System.Data.ConnectionState.Closed)
            {
                LaConexion.Close();
                RegistrarLog("Desconectar", "Conexión cerrada.");
            }
        }

        /// <summary>
        /// Registrar mensajes de log en un archivo externo.
        /// </summary>
        public void RegistrarLog(string origen, string mensaje)
        {
            try
            {
                string linea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{origen}] {mensaje}";
                File.AppendAllLines(rutaLog, new[] { linea });
            }
            catch
            {
                // ignoramos errores de log
            }
        }
    }
}
