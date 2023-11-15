using LibreriaElPortal_WebAPI.Models;

namespace LibreriaElPortal_WebAPI.DTOs
{
    public class VentaDto
    {
        public int VentaId { get; set; }
        public DateTime FechaVenta { get; set; }
        public int? ClienteId { get; set; }
        public ICollection<DetalleVentaDto> Detalles { get; set; } = new List<DetalleVentaDto>();

    }
}
