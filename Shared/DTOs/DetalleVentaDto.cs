namespace Shared.DTOs
{
    public class DetalleVentaDto
    {
        public int VentaId { get; set; }
        public string Isbn { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
