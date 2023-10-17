using LibreriaElPortal_WebAPI.DTOs;

namespace LibreriaElPortal_WebAPI.Interfaces
{
    public interface IVentaRepository
    {
        Task<ICollection<VentaDto>?> GetVentasAsync();
        Task<VentaDto> CreateVentaAsync(AgregarVentaDto venta);
        Task<ICollection<VentaDto>?> GetVentasByClienteAsync(int clienteId);
        Task<ICollection<VentaDto>?> GetVentasDelDiaAsync();
    }
}
