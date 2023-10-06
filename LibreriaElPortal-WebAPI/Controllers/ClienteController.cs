using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using LibreriaElPortal_WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly IClienteRepository _clienteRepository;
        public readonly IMapper _mapper;
        public ClienteController(IClienteRepository clienteRepository, IMapper mapper) 
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var listaClientes = await _clienteRepository.GetClientesAsync();

            if (listaClientes == null)
            {
                return NotFound("No se encontraron registros.");
            }
            
            return Ok(listaClientes);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _clienteRepository.GetClienteAsync(id);

            if (cliente == null)
            {
                return NotFound("No se encontró el cliente.");
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] AgregarClienteDto cliente)
        {     
            var existe = _clienteRepository.GetClienteByEmailAsync(cliente.Email);
            if (existe != null)
            {
                return BadRequest("Ya existe un cliente con el email ingresado.");
            }
            var nuevoCliente = await _clienteRepository.CreateClienteAsync(cliente);
            if (nuevoCliente == null)
            {
                return StatusCode(500, "Hubo un error al crear el cliente");
            }
            
            return CreatedAtAction(nameof(GetCliente), new { id = nuevoCliente.ClienteId }, nuevoCliente);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCliente([FromBody] ClienteDto cliente)
        {
            var extiste = await _clienteRepository.ExisteClienteAsync(cliente.ClienteId);

            if(!extiste)
            {
               return NotFound("No se encontró el cliente que intenta actualizar.");
            }
            var clienteActualizado = await _clienteRepository.UpdateClienteAsync(cliente);
            if (!clienteActualizado)
            {
                return StatusCode(500, "Hubo un error al actualizar el cliente");
            }
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCliente(int clienteId)
        {
            var extiste = await _clienteRepository.ExisteClienteAsync(clienteId);

            if (!extiste)
            {
                return NotFound("No se encontró el cliente que intenta eliminar.");
            }

            var clienteEliminado = await _clienteRepository.DeleteClienteAsync(clienteId);
            if (!clienteEliminado)
            {
                return StatusCode(500, "Hubo un error al eliminar el cliente");
            }
            return NoContent();
        }
    }
}
