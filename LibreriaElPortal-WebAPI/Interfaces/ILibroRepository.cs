using LibreriaElPortal_WebAPI.DTOs;

namespace LibreriaElPortal_WebAPI.Interfaces
{
    public interface ILibroRepository
    {
        Task<ICollection<LibroDto>?> GetLibrosAsync();
        Task<LibroDto?> GetLibroAsync(string Isbn);
        Task<LibroDto> CreateLibroAsync(LibroDto libro);
        Task<bool> UpdateLibroAsync(LibroDto libro);
        Task<bool> DeleteLibroAsync(string isbn);
        Task<bool> ExisteLibro(string isbn);//Sólo si no hay ventas asociadas
    }
}
