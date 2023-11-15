using Shared.DTOs;
using LibreriaElPortal_WebAPI.Models;

namespace LibreriaElPortal_WebAPI.Interfaces
{
    public interface IClienteRepository
    {
        Task<ICollection<ClienteDto>?> GetClientesAsync();
        Task<ClienteDto?> GetClienteAsync(int id);
        Task<bool> ExisteClienteByEmailAsync(string email);
        Task<ClienteDto> CreateClienteAsync(AgregarClienteDto cliente); 
        Task<bool> UpdateClienteAsync(ClienteDto cliente); 
        Task<bool> DeleteClienteAsync(int id);
        Task<bool> ExisteClienteAsync(int id);
    }
}
