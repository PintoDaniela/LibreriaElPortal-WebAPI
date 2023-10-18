using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        public readonly IVentaRepository _ventaRepository;

        public VentaController(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetVentas() 
        {
            var ventas = await _ventaRepository.GetVentasAsync();
            return Ok(ventas);
        }

        [HttpGet ("{clienteId}")]
        public async Task<IActionResult> GetVentas(int clienteId)
        {
            var ventas = await _ventaRepository.GetVentasByClienteAsync(clienteId);
            return Ok(ventas);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenta([FromBody] AgregarVentaDto venta)
        {
            var nuevaVenta = await _ventaRepository.CreateVentaAsync(venta);

            return CreatedAtAction(nameof(CreateVenta), new { id = nuevaVenta.VentaId }, nuevaVenta);
        }
    }
}
