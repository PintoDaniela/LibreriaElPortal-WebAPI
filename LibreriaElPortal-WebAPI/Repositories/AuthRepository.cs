using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Helper;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly elportalContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(elportalContext appDbContext, PasswordHasher passwordHasher, ILogger<AuthRepository> logger)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<User?> CreateUserAsync(UserDto newUser)
        {
            try
            {
                var (hashedPassword, passwordSalt) = _passwordHasher.HashPassword(newUser.Password);

                var user = new User
                {
                    Username = newUser.Username,
                    PasswordHash = hashedPassword,
                    PasswordSalt = passwordSalt,
                    FechaAlta = DateTime.UtcNow,
                    Status = true,
                    TipoUsuario = 1
                };

                await _appDbContext.AddAsync(user);
                var usuarioCreado = await _appDbContext.SaveChangesAsync();

                if (usuarioCreado >= 0)
                {
                    return (user);
                }

                return null;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "CreateUserAsync");
                return null;
            }
        }

        public async Task<(bool, User?)> CheckUserCredentialsAsync(UserDto loginUser)
        {
            try
            {
                bool validPass = false;
                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(loginUser.Username));
                if (user != null)
                {
                    validPass = _passwordHasher.VerifyPassword(user.PasswordHash, user.PasswordSalt, loginUser.Password);
                }
                if (user == null || !validPass)
                {
                    return (false, null);
                }
                return (true, user);
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "CheckUserCredentialsAsync");
                return (false, null);
            }
        }

        public async Task<bool> ExisteUsuarioByNameAsync(string userame)
        {            
            try
            {
                bool existe = await _appDbContext.Users.AnyAsync(u => u.Username.Equals(userame.Trim()));
                return existe;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "ExisteUsuarioByNameAsync");
                return false;
            }
        }

        //--------------------------------------
        //Métodos auxiliares
        //--------------------------------------

        ///// LOGS /////
        //Los métodos que graban Logs se definen a nivel local porque el objeto _logger es propio de cada clase ej:  ILogger<AuthRepository>       
        private void ExceptionLogs(Exception ex, string metodo)
        {
            string mensaje = LogMessages.ExceptionLogMessage(ex, metodo);
            _logger.LogError(mensaje);
        }

       
    }
}
