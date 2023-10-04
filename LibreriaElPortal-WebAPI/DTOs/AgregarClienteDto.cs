namespace LibreriaElPortal_WebAPI.DTOs
{
    public class AgregarClienteDto
    {
        public string Nombre { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}
