using AutoMapper;
using Shared.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using LibreriaElPortal_WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Expressions;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly IClienteRepository _clienteRepository;
        public readonly IVentaRepository _ventaRepository;
        public readonly IMapper _mapper;
        public ClienteController(IClienteRepository clienteRepository, IMapper mapper, IVentaRepository ventaRepository) 
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _ventaRepository = ventaRepository;
        }


        /// <summary>
        /// Devuelve el listado completo de clientes activos.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ListadoClientes")]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var listaClientes = await _clienteRepository.GetClientesAsync();

            if (listaClientes == null)
            {
                return NotFound("No se encontraron registros.");
            }

            return Ok(listaClientes);
        }

        /// <summary>
        /// Recibe un id de cliente por url y devuelve el cliente al cual corresponda.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _clienteRepository.GetClienteAsync(id);

            if (cliente == null)
            {
                return NotFound("No se encontró el cliente.");
            }

            return Ok(cliente);
        }

        /// <summary>
        /// Recibe información de un cliente desde el cuerpo de la solicitud (ver ejemplo de "Request Body") para crear un nuevo registro.
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ClienteDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearCliente([FromBody] AgregarClienteDto cliente)
        {
            var existe = await _clienteRepository.ExisteClienteByEmailAsync(cliente.Email);
            if (existe)
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


        /// <summary>
        /// Recibe información de un cliente desde el cuerpo de la solicitud (ver ejemplo de "Request Body") para actualizar un registro existente.
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      
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

        /// <summary>
        /// Recibe el id de un cliente por url y elimina el registro de la base de datos.
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCliente(int clienteId)
        {
            var extiste = await _clienteRepository.ExisteClienteAsync(clienteId);
            if (!extiste)
            {
                return BadRequest("El cliente que intenta eliminar no existe.");
            }

            var ventasAsociadas = await _ventaRepository.GetVentasByClienteAsync(clienteId);
            if (ventasAsociadas != null && ventasAsociadas.Count() > 0)
            {
                return BadRequest($"No se puede eliminar el cliente con ID: \"{clienteId}\" porque tiene ventas asociadas.");
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
