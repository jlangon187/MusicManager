using MusicManager.Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace MusicManager
{
    public class AppMusic
    {
        public ConfiguracionConexion configConexion;        // Objeto con la configuración de la conexión a la BD.
        public EstadoApp estadoApp;                         // Estado de la aplicación en el momento actual.
        public string rutaBase { get; private set; }        // Ruta base de la aplicación.
        public string rutaConfigDB;                         // Ruta al archivo de configuración de la base de datos.

        // Me indica si estoy conectado a la base de datos o no.
        public bool conectado => (_conexion != null) && (_conexion.State == System.Data.ConnectionState.Open);

        public string ultimoError { get; private set; }     // Ultimo error registrado.

        private MySqlConnection _conexion = null;           // Cliente MySQL para comunicarnos con la base de datos

        private DebugApp debug;                             // Objeto para gestionar el log de depuración.
        public bool Conectado => LaConexion != null && LaConexion.State == System.Data.ConnectionState.Open;

        public AppMusic()
        {
            // Estado inicial de la App.
            estadoApp = EstadoApp.Iniciando;

            // Instancio el cliente mysql.
            _conexion = new MySqlConnection();

            // Inicializo la aplicación
            InitApp();
        }

        public MySqlConnection LaConexion => _conexion;     // Propiedad para acceder a la conexión MySQL.


        /// <summary>
        /// Inicializa la aplicación (conexión a la base de datos, log de errores, etc.).
        /// </summary>
        private void InitApp()
        {
            // Ruta por defecto en Documentos
            rutaBase = AppDomain.CurrentDomain.BaseDirectory;

            // Ruta al archivo de configuración de la base de datos
            rutaConfigDB = Path.Combine(rutaBase, "configDB.json");

            // Inicializa el sistema de logs
            debug = new DebugApp(rutaBase);

            // Configuro y me conecto a la base de datos.
            ConfiguraYConectaDB(rutaConfigDB);
        }

        /// <summary>
        /// Carga la configuración de la base de datos desde el archivo de configuración
        /// </summary>
        /// <param name="aRutaConfig"></param>
        public void ConfiguraYConectaDB(string aRutaConfig)
        {
            // Inicializo el último error
            ultimoError = "";

            // Cargo la configuración
            configConexion = CargarConfiguracionDB(aRutaConfig);


            if (configConexion != null)
            {
                if (ConectarDB())
                {
                    estadoApp = EstadoApp.Conectado;
                }
                else
                {
                    estadoApp = (ultimoError != "") ? EstadoApp.Error : EstadoApp.SinConexion;

                    // Solo logueamos si hay error
                    if (!string.IsNullOrEmpty(ultimoError))
                        RegistrarLog("ConfiguraYConectaDB", ultimoError);
                }
            }
            else
            {
                estadoApp = (ultimoError != "") ? EstadoApp.Error : EstadoApp.SinConexion;

                // Solo logueamos si hay error
                if (!string.IsNullOrEmpty(ultimoError))
                    RegistrarLog("ConfiguraYConectaDB", ultimoError);
            }
        }

        /// <summary>
        /// Carga la configuración de la base de datos en un objeto de la clase "ConfiguracionConexion",
        /// retornando dicho objeto. La configuración la intentará cargar de un archivo llamado
        /// "configDB.json" en el directorio base de la aplicación.
        /// </summary>
        /// <returns>Retorna el objeto de tipo "ConfiguracionConexion" con la configuración de la base
        /// de datos, null si no lo ha conseguido.</returns>
        private ConfiguracionConexion CargarConfiguracionDB(string aRuta)
        {
            ConfiguracionConexion resultado = null;

            if (File.Exists(aRuta))
            {
                try
                {
                    string jsonText = File.ReadAllText(aRuta);
                    resultado = JsonSerializer.Deserialize<ConfiguracionConexion>(jsonText);
                }
                catch (Exception ex)
                {
                    ultimoError = "Error al cargar archivo de configuración. " + ex.Message;
                    RegistrarLog("CargarConfiguracionDB", ultimoError);
                }
            }
            else
            {
                // Establecer mensaje de error si no existe el archivo
                ultimoError = $"No se encontró el archivo de configuración: {aRuta}";
                RegistrarLog("CargarConfiguracionDB", ultimoError);
            }

            return resultado;
        }

        /// <summary>
        /// Intenta conectarse a la base de datos con la configuración de las propiedades de la clase.
        /// Si durante el intenta de conexión se produce alguna excepción, almacena el mensaje de error
        /// en el campo "UltimoError".
        /// </summary>
        /// <returns>True si se ha conectado correctamente, false sino.</returns>
        public bool ConectarDB()
        {
            if (conectado)
                _conexion.Close();

            _conexion.ConnectionString = configConexion.CadenaDeConexion();

            try
            {
                _conexion.Open();
            }
            catch (Exception ex)
            {
                ultimoError = "Error al intentar la conexión a la base de datos. " + ex.Message;
                RegistrarLog("ConectarDB", ultimoError);
            }

            if (conectado)
            {
                estadoApp = EstadoApp.Conectado;
            }
            else
            {
                estadoApp = EstadoApp.SinConexion;
            }

            return conectado;
        }


        /// <summary>
        /// Cierra la conexión a la base de datos.
        /// </summary>
        public void DesconectarDB()
        {
            if (conectado)
            {
                try
                {
                    _conexion.Close();
                }
                catch (Exception ex)
                {
                    ultimoError = "Error al intentar cerrar conexión a la base de datos. " + ex.Message;
                    RegistrarLog("DesconectarDB", ultimoError);
                }
            }

            estadoApp = EstadoApp.SinConexion;
        }

        /// <summary>
        /// Registra una línea en el log de depuración.
        /// </summary>
        /// <param name="proceso"></param>
        /// <param name="mensaje"></param>
        public void RegistrarLog(string proceso, string mensaje)
        {
            string linea = $"{DateTime.Now:dd-MM-yyyy} | {DateTime.Now:HH:mm:ss} | {proceso} | {mensaje}";
            debug.GuardarLog(linea);
        }
    }
}
