using Shared.DTOs;

namespace LibreriaElPortal_WebAPI.Interfaces
{
    public interface IVentaRepository
    {
        Task<ICollection<VentaDto>?> GetVentasAsync();
        Task<VentaDto> CreateVentaAsync(AgregarVentaDto venta);
        Task<bool> UpdateVentaAsync(VentaDto venta);
        Task<ICollection<VentaDto>?> GetVentasByClienteAsync(int clienteId);
        Task<ICollection<VentaDto>?> GetVentasDelDiaAsync();
        Task<bool> ExisteVentaAsync(int ventaId);
    }
}
