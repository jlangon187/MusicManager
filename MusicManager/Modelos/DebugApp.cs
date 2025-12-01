using System;
using System.IO;

namespace MusicManager.Modelos
{
    public class DebugApp
    {
        private readonly string rutaLog;

        /// <summary>
        /// Constructor: inicializa la ruta al archivo de log.
        /// </summary>
        /// <param name="rutaBase">Ruta base de la aplicación</param>
        public DebugApp(string rutaBase)
        {
            // El archivo estará en la ruta base, dentro de "logs/app.log"
            string carpetaLogs = Path.Combine(rutaBase, "logs");
            if (!Directory.Exists(carpetaLogs))
                Directory.CreateDirectory(carpetaLogs);

            rutaLog = Path.Combine(carpetaLogs, "app.log");
        }

        /// <summary>
        /// Guarda un mensaje en el archivo de log, agregándolo al final.
        /// </summary>
        /// <param name="mensaje">Texto a registrar</param>
        public void GuardarLog(string mensaje)
        {
            try
            {
                // Añade el mensaje en una sola línea
                File.AppendAllText(rutaLog, mensaje + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Si falla el log, no interrumpimos la aplicación, solo ignoramos el error.
                Console.WriteLine("Error al escribir log: " + ex.Message);
            }
        }
    }
}
