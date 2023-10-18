using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly elportalContext _Context;
        private readonly IMapper _mapper;
        private readonly ILibroRepository _libroRepository;

        public VentaRepository(elportalContext context, IMapper mapper, ILibroRepository libroRepository)
        {
            _Context = context;
            _mapper = mapper;
            _libroRepository = libroRepository;
        }

        public async Task<ICollection<VentaDto>?> GetVentasAsync()
        {
            List<VentaDto> VentasDto = new List<VentaDto>();
            try
            {
                var ventas = await _Context.Ventas.ToListAsync();

                //Si tengo ventas, recorro la lista de ventas para buscar los detalles y agregarlos a cada una.
                if (ventas != null)
                {
                    foreach(var venta in ventas)
                    {
                        //Uso el mapper para convertir del modelo venta a ventaDTO
                        VentaDto ventaDto = _mapper.Map<VentaDto>(venta);

                        var detalles = await _Context.DetalleVenta
                            .Where(d => d.VentaId == venta.VentaId)
                            .ToListAsync();

                        ventaDto.DetalleVenta = _mapper.Map<List<DetalleVentaDto>>(detalles); // uso el mapper para convertir la lista "detalles" a una lista de DetalleVentaDto

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

        public async Task<ICollection<VentaDto>?> GetVentasByClienteAsync(int clienteId)
        {
            List<VentaDto> VentasDto = new List<VentaDto>();
            try
            {
                var ventas = await _Context.Ventas
                    .Where(v => v.ClienteId == clienteId)
                    .ToListAsync();

                //Si tengo ventas, recorro la lista de ventas para buscar los detalles y agregarlos a cada una.
                if (ventas != null)
                {
                    foreach (var venta in ventas)
                    {
                        //Uso el mapper para convertir del modelo venta a ventaDTO
                        VentaDto ventaDto = _mapper.Map<VentaDto>(venta);

                        var detalles = await _Context.DetalleVenta
                            .Where(d => d.VentaId == venta.VentaId)
                            .ToListAsync();

                        ventaDto.DetalleVenta = _mapper.Map<List<DetalleVentaDto>>(detalles); // uso el mapper para convertir la lista "detalles" a una lista de DetalleVentaDto

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

        public Task<ICollection<VentaDto>?> GetVentasDelDiaAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<VentaDto> CreateVentaAsync(AgregarVentaDto venta)
        {
            var ventaCreada = new VentaDto();
            try
            {                
                var ventaNueva = _mapper.Map<Venta>(venta);
                ventaNueva.FechaVenta = DateTime.Now;

                await _Context.Ventas.AddAsync(ventaNueva);
                _Context.SaveChanges();

                ventaCreada = _mapper.Map<VentaDto>(ventaNueva);

                var detallesVenta = _mapper.Map<List<DetalleVentum>>(venta.Detalles);
                foreach (var nuevoDetalle in detallesVenta)
                {                    
                    nuevoDetalle.VentaId = ventaNueva.VentaId;
                    nuevoDetalle.PrecioUnitario = _libroRepository.GetPrecioLibroAsync(nuevoDetalle.Isbn).Result;
                    /*
                    nuevoDetalle.PrecioUnitario = await _Context.Libros
                                                 .Where(l => l.Isbn == nuevoDetalle.Isbn)
                                                 .Select(l => l.Precio)
                                                 .FirstOrDefaultAsync();
                    */
                    await _Context.DetalleVenta.AddAsync(nuevoDetalle);
                    _Context.SaveChanges();

                    //Descuento el stock del libro
                    await _libroRepository.UpdateStockLibroAsync(nuevoDetalle.Isbn, nuevoDetalle.Cantidad);
                    /*
                    var libroParaActualizar = await _Context.Libros
                        .Where(l => l.Isbn == nuevoDetalle.Isbn)
                        .FirstOrDefaultAsync();

                    if(libroParaActualizar != null)
                    {
                        libroParaActualizar.Stock -= nuevoDetalle.Cantidad;
                        _Context.Libros.Update(libroParaActualizar);
                        _Context.SaveChanges();
                    }
                   */
                    var detalleCreado = _mapper.Map<DetalleVentaDto>(nuevoDetalle);
                    ventaCreada.DetalleVenta.Add(detalleCreado);
                }

                return ventaCreada;

            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
