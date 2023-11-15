using Shared.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class LibroController : Controller
    {       
        public readonly ILibroRepository _libroRepository;
        public readonly IDetalleVentaRepository _detalleVentaRepository;
        public LibroController(ILibroRepository libroRepository, IDetalleVentaRepository detalleVentaRepository)
        {
            _libroRepository = libroRepository;
            _detalleVentaRepository = detalleVentaRepository;
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

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetLibro(string isbn)
        {
            var libro = await _libroRepository.GetLibroAsync(isbn);

            if (libro == null)
            {
                return NotFound("No se encontró el libro.");
            }

            return Ok(libro);
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
        
        [HttpPut]
        public async Task<IActionResult> UpdateLibro(LibroDto libro)
        {
            var existe = await _libroRepository.ExisteLibroAsync(libro.Isbn);
            if (!existe)
            {
                return BadRequest("No se encuentra el libro que intenta actualizar.");
            }
           
            var libroActualizado = await _libroRepository.UpdateLibroAsync(libro);

            if (!libroActualizado)
            {
                return StatusCode(500, $"Hubo un error al actualizar el libro {libro.Titulo}.");
            }

            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLibro(LibroDto libro)
        {
            var existe = await _libroRepository.ExisteLibroAsync(libro.Isbn);
            if (!existe)
            {
                return BadRequest("No se encuentra el libro que intenta eliminar.");
            }

            var hayVentasAsociadas = await _detalleVentaRepository.ExistenDetallesVentaByLibroAsync(libro.Isbn);
            if (hayVentasAsociadas)
            {
                return BadRequest($"No se puede eliminar el libro \"{libro.Titulo}\" ({libro.Isbn}) porque tiene ventas asociadas.");
            }

            var libroEliminado = await _libroRepository.DeleteLibroAsync(libro.Isbn);

            if (!libroEliminado)
            {
                return StatusCode(500, $"Hubo un error al remover el libro {libro.Titulo}.");
            }

            return NoContent();
        }
    }
}
