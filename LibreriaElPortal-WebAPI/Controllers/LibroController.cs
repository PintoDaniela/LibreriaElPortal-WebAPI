using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LibroController : Controller
    {       
        public readonly ILibroRepository _libroRepository; 
        public LibroController(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLibros()
        {
            var listaLibros = await _libroRepository.GetLibrosAsync();

            if (listaLibros == null)
            {
                return NotFound("No se encontraron registros.");
            }

            return Ok(listaLibros);
        }
    }
}
