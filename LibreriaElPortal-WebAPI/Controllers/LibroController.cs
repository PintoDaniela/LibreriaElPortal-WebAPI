using LibreriaElPortal_WebAPI.DTOs;
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
        [HttpPost]
        public async Task<IActionResult> CreateLibro(LibroDto libro)
        {
            var existe = await _libroRepository.ExisteLibroAsync(libro.Isbn);
            if (existe)
            {
                return BadRequest("El libro que intenta crear ya existe.");
            }
            var libroNuevo = await _libroRepository.CreateLibroAsync(libro);

            if (libroNuevo == null)
            {
                return StatusCode(500, $"Hubo un error al registrar el libro {libro.Titulo}.");
            }

            return CreatedAtAction(nameof(CreateLibro), new { id = libroNuevo.Isbn }, libroNuevo);
        }
    }
}
