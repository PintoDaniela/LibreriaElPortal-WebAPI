using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
                    //Seteo los atributos del detalle.
                    nuevoDetalle.VentaId = ventaNueva.VentaId;
                    nuevoDetalle.PrecioUnitario = _libroRepository.GetPrecioLibroAsync(nuevoDetalle.Isbn).Result;

                    await _Context.DetalleVenta.AddAsync(nuevoDetalle);
                    _Context.SaveChanges();

                    //Descuento el stock del libro.
                    await _libroRepository.UpdateStockLibroAsync(nuevoDetalle.Isbn, nuevoDetalle.Cantidad);

                    //Agrego el detalle de la venta a la lista de detalles del objeto "ventaCreada".
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

        public async Task<bool> UpdateVentaAsync(VentaDto venta)
        {
            bool resultado = false;
            try
            {
                //Borro los detalles anteriores para agregar los detalles actualizados.
                var detallesAnteriores = _Context.DetalleVenta.Where(dv => dv.VentaId == venta.VentaId).ToList();
                foreach (var detalle in detallesAnteriores)
                {
                    _Context.DetalleVenta.Remove(detalle);
                    _Context.SaveChanges();
                    //Devuelvo el stock del libro.
                    await _libroRepository.UpdateStockLibroAsync(detalle.Isbn, (detalle.Cantidad * -1));
                }

                //Actualizo la venta:
                var ventaParaActualizar = _mapper.Map<Venta>(venta);

                _Context.Ventas.Update(ventaParaActualizar); //También inserta los detalles de la venta.
                resultado = _Context.SaveChanges() > 0 ? true : false;                


                //Recorro la lista de detalles actualizados para actualizar el stock de cada libro.
                var detallesVenta = _mapper.Map<List<DetalleVentum>>(venta.DetalleVenta);
                foreach (var nuevoDetalle in detallesVenta)
                {                   
                    await _libroRepository.UpdateStockLibroAsync(nuevoDetalle.Isbn, nuevoDetalle.Cantidad);
                }

                return resultado;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ExisteVentaAsync(int ventaId)
        {
            try
            {
                bool existe = await _Context.Ventas.AnyAsync(v => v.VentaId == ventaId);
                return existe;
            }
            catch
            {
                return false;
            }
        }
    }
}
