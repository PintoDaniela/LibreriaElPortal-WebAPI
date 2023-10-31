using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Models;

namespace LibreriaElPortal_WebAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> CreateUserAsync(UserDto newUser);
        Task<(bool, User?)> CheckUserCredentialsAsync(UserDto loginUser);
        Task<bool> ExisteUsuarioByNameAsync(String userame);
    }
}
