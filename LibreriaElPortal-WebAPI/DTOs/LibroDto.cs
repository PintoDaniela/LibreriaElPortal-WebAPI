namespace LibreriaElPortal_WebAPI.DTOs
{
    public class LibroDto
    {
        public string Isbn { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string? Autor { get; set; }
        public string? Genero { get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
    }
}
