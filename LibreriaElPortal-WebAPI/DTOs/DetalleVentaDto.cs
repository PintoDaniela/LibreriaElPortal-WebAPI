namespace LibreriaElPortal_WebAPI.DTOs
{
    public class DetalleVentaDto
    {
        public int DetalleVentaId { get; set; }
        public int VentaId { get; set; }
        public string Isbn { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
