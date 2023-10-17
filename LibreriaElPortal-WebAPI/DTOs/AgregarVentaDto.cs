namespace LibreriaElPortal_WebAPI.DTOs
{
    public class AgregarVentaDto
    {
        public string Isbn { get; set; } = null!;
        public int? ClienteId { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}
