using LibreriaElPortal_WebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibreriaElPortal_WebAPI.Helper
{
    public class JwtTokenManager
    {
        private readonly ILogger<JwtTokenManager> _logger;

        public JwtTokenManager(ILogger<JwtTokenManager> logger)
        {
            _logger = logger;
        }

        public string GenerateJwtToken(User user, IConfiguration configuration)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username)
            };

                int TokenExpiration = Convert.ToInt32(configuration["JwtSettings:TokenExpirationMinutes"]);

                var token = new JwtSecurityToken(
                    issuer: configuration["JwtSettings:Issuer"],
                    audience: configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(TokenExpiration),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "GenerateJwtToken");
                return "";
            }

        }


        public bool ValidateToken(string token, IConfiguration configuration)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
                        var tokenHandler = new JwtSecurityTokenHandler();
                        //verificar validez del token, excepto fecha de expiración
                        //Mientras no tenga registro de logs lo hago por separado para ver en qué falla. Después se pondría esta validación en true y se borraría la verificación posterior de la fecha
                        tokenHandler.ValidateToken(token, new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidIssuer = configuration["JwtSettings:Issuer"],
                            ValidAudience = configuration["JwtSettings:Audience"],
                            IssuerSigningKey = securityKey,
                            ValidateLifetime = false
                        }, out SecurityToken validatedToken);

                        // Verificar la fecha de expiración
                        if (validatedToken is JwtSecurityToken jwtToken && jwtToken.ValidTo > DateTime.Now)
                        {
                            return true;
                        }
                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "ValidateToken");
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
