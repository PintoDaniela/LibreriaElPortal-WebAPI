using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly elportalContext _Context;
        private readonly IMapper _mapper;

        public VentaRepository(elportalContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<VentaDto>?> GetVentasAsync()
        {
            List<VentaDto> VentasDto = new List<VentaDto>();
            try
            {
                var ventas = await _Context.Ventas.ToListAsync();
                if (ventas != null)
                {
                    foreach(var venta in ventas)
                    {
                        VentaDto ventaDto = _mapper.Map<VentaDto>(venta);

                        var detalles = await _Context.DetalleVenta
                            .Where(d => d.VentaId == venta.VentaId)
                            .ToListAsync();

                        ventaDto.DetalleVenta = _mapper.Map<List<DetalleVentaDto>>(detalles);

                        VentasDto.Add(ventaDto);
                    }
                }
                return VentasDto;
                

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<ICollection<VentaDto>?> GetVentasByClienteAsync(int clienteId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<VentaDto>?> GetVentasDelDiaAsync()
        {
            throw new NotImplementedException();
        }

        public Task<VentaDto> CreateVentaAsync(AgregarVentaDto venta)
        {
            throw new NotImplementedException();
        }
    }
}
