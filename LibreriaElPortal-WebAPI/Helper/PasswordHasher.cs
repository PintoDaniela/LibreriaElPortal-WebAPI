using System.Security.Cryptography;
using System.Text;

namespace LibreriaElPortal_WebAPI.Helper
{
    public class PasswordHasher
    {
        private readonly ILogger<PasswordHasher> _logger;
        public PasswordHasher(ILogger<PasswordHasher> logger)
        {
            _logger = logger;
        }
        public (string? hashedPassword, byte[]? salt) HashPassword(string password)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] salt = new byte[16];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }

                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                    Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                    Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                    byte[] hashedPassword = sha256.ComputeHash(saltedPassword);

                    return (Convert.ToBase64String(hashedPassword), salt);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "HashPassword");
                return (null, null);
            }

        }

        public bool VerifyPassword(string hashedPassword, byte[] salt, string passwordToCheck)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(passwordToCheck);
                    byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                    Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                    Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                    byte[] hashedPasswordToCheck = sha256.ComputeHash(saltedPassword);
                    string hashedPasswordToCheckBase64 = Convert.ToBase64String(hashedPasswordToCheck);

                    return hashedPassword == hashedPasswordToCheckBase64;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "VerifyPassword");
                return false;
            }
        }

        //--------------------------------------
        //Métodos auxiliares
        //--------------------------------------

        ///// LOGS /////
        //Los métodos que graban Logs se definen a nivel local porque el objeto _logger es propio de cada clase ej:  ILogger<JwtTokenManager>       
        private void ExceptionLogs(Exception ex, string metodo)
        {
            string mensaje = LogMessages.ExceptionLogMessage(ex, metodo);
            _logger.LogError(mensaje);
        }
    }
}
