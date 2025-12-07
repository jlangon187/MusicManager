using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicManager.Utils
{
    public class Validacion
    {
        // Validar si el año es un número válido entre el 1900 y el 2100
        public static bool ValidarAnio(string anioStr, out int anio)
        {
            if (int.TryParse(anioStr, out anio))
            {
                return anio >= 1900 && anio <= 2100;
            }
            return false;
        }

        // Validar si los campos obligatorios no están vacíos pasandole el texto y el nombre del campo
        public static bool ValidarCampoObligatorio(string texto, string nombreCampo, out string mensajeError)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                mensajeError = $"El campo {nombreCampo} es obligatorio.";
                return false;
            }
            mensajeError = string.Empty;
            return true;
        }
    }
}
