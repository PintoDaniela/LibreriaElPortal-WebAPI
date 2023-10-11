namespace LibreriaElPortal_WebAPI.DTOs
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime? FechaAlta { get; set; }
    }
}
