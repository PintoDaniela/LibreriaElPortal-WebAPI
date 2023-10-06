using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly elportalContext _Context;
        private readonly IMapper _mapper;

        public ClienteRepository(elportalContext elportalContext, IMapper mapper)
        {
            _Context = elportalContext;
            _mapper = mapper;
        }


        public async Task<ICollection<ClienteDto>?> GetClientesAsync()
        {
            try
            {
                var clientes = await _Context.Clientes.ToListAsync();
               
                if (clientes != null)
                {
                    var listaClientes = _mapper.Map<List<ClienteDto>>(clientes);
                    return listaClientes;
                }
               
                //con el mapper no hace falta hacer la asignación de valores. Sólo se vale si los atributos del DTO y la entidad se llaman igual.
                //var listaClientes = clinetes.Select(c => new ClienteDto
                //{
                //    ClienteId = c.ClienteId,
                //    Nombre = c.Nombre,
                //    Email = c.Email,
                //    Telefono = c.Telefono,

                //}).ToList();               
                return null;
                
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        public async Task<ClienteDto?> GetClienteAsync(int id)
        {
            try
            {
                var cliente = await _Context.Clientes
                .Where(c => c.ClienteId == id)
                .FirstOrDefaultAsync();

                if (cliente != null)
                {
                    ClienteDto clienteDto = _mapper.Map<ClienteDto>(cliente);
                    return clienteDto;
                }
                
                return null;
                
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<ClienteDto> CreateClienteAsync(AgregarClienteDto cliente)
        {
            try
            {
                var newCliente = _mapper.Map<Cliente>(cliente);
                await _Context.Clientes.AddAsync(newCliente);
                if (await _Context.SaveChangesAsync() > 0)
                {
                    // Después de guardar, newCliente.ClienteId contendrá el ID generado automáticamente.
                    
                    var clienteDto = _mapper.Map<ClienteDto>(newCliente);
                   /* var clienteDto = new ClienteDto
                    {
                        ClienteId = newCliente.ClienteId,
                        Nombre = newCliente.Nombre,
                        Email = newCliente.Email,
                        Telefono = newCliente.Telefono
                    };*/

                    // Devuelve el objeto ClienteDto.
                    return clienteDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Maneja las excepciones aquí si es necesario.
                return null;
            }
        }


        public async Task<bool> DeleteClienteAsync(int id)
        {
            try
            {
                var cliente = await _Context.Clientes
                 .Where(c => c.ClienteId == id)
                 .FirstOrDefaultAsync();

                if (cliente == null)
                {
                    return false;
                }
                _Context.Remove(cliente);
                return await _Context.SaveChangesAsync() > 0 ? true : false;
            }
            catch 
            {
                return false; 
            }
        }

        public async Task<bool> UpdateClienteAsync(ClienteDto clienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);                
                _Context.Update(cliente);
                var resultado = await _Context.SaveChangesAsync() > 0 ? true : false;
                return resultado;
            }
            catch 
            { 
                return false;
            }
        }

        public async Task<bool> ExisteClienteAsync(int id)
        {
            var existeCliente =  await _Context.Clientes.AnyAsync(c => c.ClienteId == id);
            return existeCliente;
        }

        public async Task<ClienteDto?> GetClienteByEmailAsync(string email)
        {
            try
            {
                var cliente = await _Context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
                if (cliente == null)
                {
                    return null;
                }
                return _mapper.Map<ClienteDto?>(cliente);
            }
            catch 
            { 
                return null; 
            }
        }
    }
}
