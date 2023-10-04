using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LibroController : Controller
    {       
        public readonly elportalContext _context; 
        public LibroController(elportalContext context)
        {
            _context = context;            
        }

        [HttpGet]
        public async Task<IActionResult> GetLibros()
        {
            return Ok(await _context.Libros
                .ToListAsync());
        }
    }
}
