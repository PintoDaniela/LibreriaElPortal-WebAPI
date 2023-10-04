using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
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
            var nuevoCliente = await _clienteRepository.CreateClienteAsync(cliente);
            if (nuevoCliente == null)
            {
                return StatusCode(500, "Hubo un error al crear el cliente");
            }
            
            return CreatedAtAction(nameof(GetCliente), new { id = nuevoCliente.ClienteId }, nuevoCliente);
        }
    
    }
}
