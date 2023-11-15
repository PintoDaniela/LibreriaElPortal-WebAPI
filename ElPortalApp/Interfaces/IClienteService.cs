using Shared.DTOs;

namespace ElPortalApp.Interfaces
{
    public interface IClienteService
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
