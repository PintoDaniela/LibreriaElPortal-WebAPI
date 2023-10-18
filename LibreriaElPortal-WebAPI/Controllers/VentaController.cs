using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        public readonly IVentaRepository _ventaRepository;
        public readonly IClienteRepository _clienteRepository;  

        public VentaController(IVentaRepository ventaRepository, IClienteRepository clienteRepository)
        {
            _ventaRepository = ventaRepository;
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetVentas() 
        {
            var ventas = await _ventaRepository.GetVentasAsync();
            if (ventas == null)
            {
                return NotFound("No se encontraron registros.");
            }

            return Ok(ventas);
        }

        [HttpGet ("{clienteId}")]
        public async Task<IActionResult> GetVentas(int clienteId)
        {
            var ventas = await _ventaRepository.GetVentasByClienteAsync(clienteId);
            if (ventas == null)
            {
                return NotFound("No se encontraron registros.");
            }
            return Ok(ventas);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenta([FromBody] AgregarVentaDto venta)
        {            
            var existeCliente = await _clienteRepository.ExisteClienteAsync(venta.ClienteId);
            if(!existeCliente)
            {
                return BadRequest($"No se encuentra al cliente con ID = {venta.ClienteId}. Para realizar la venta se debe registrar al cliente.");
            }
            var nuevaVenta = await _ventaRepository.CreateVentaAsync(venta);
            if (nuevaVenta == null)
            {
                return StatusCode(500, $"Hubo un error al registrar la venta.");
            }
            return CreatedAtAction(nameof(CreateVenta), new { id = nuevaVenta.VentaId }, nuevaVenta);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVenta([FromBody] VentaDto venta)
        {
            var existeVenta = await _ventaRepository.ExisteVentaAsync(venta.VentaId);
            if (!existeVenta)
            {
                return BadRequest($"No se encuentra la venta que intenta actualizar.");
            }
            var ventaActualizada = await _ventaRepository.UpdateVentaAsync(venta);
            if (ventaActualizada == false)
            {
                return StatusCode(500, $"Hubo un error al actualizar la venta.");
            }
            return NoContent();
        }
    }
}
