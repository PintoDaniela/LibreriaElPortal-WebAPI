using System.Text.RegularExpressions;

namespace LibreriaElPortal_WebAPI.Helper
{
    public static class LogMessages
    {
        //Método para armar el mensaje de error proveniente de una Excepción.
        public static string ExceptionLogMessage(Exception ex, string metodo)
        {
            string posicionError;

            //Obtengo el fragmento de texto que contiene la posición en la que se dio el error. Ej.: "line 107"
            Match match = Regex.Match(ex.StackTrace, @"line (\d+)");

            if (match.Success)
            {
                // La subcadena "line 107" se encuentra en match.Groups[0].
                posicionError = match.Groups[0].Value;
            }
            else
            {
                posicionError = "-";
            }

            string text = $" FAILED |  MÉTODO: {metodo} | POSICIÓN: {posicionError} | CÓDIGO DE ERROR: {ex.HResult}  | DESCRIPCIÓN: {ex.Message}";
            return text;
        }
    }
}
