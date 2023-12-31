﻿using AutoMapper;
using Shared.DTOs;
using LibreriaElPortal_WebAPI.Helper;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly elportalContext _Context;
        private readonly IMapper _mapper;
        private readonly ILogger<LibroRepository> _logger;

        public LibroRepository(elportalContext elportalContext, IMapper mapper, ILogger<LibroRepository> logger)
        {
            _Context = elportalContext;
            _mapper = mapper;
            _logger = logger;
        }



        public async Task<ICollection<LibroDto>?> GetLibrosAsync()
        {
            try
            {
                var listaLibros = await _Context.Libros.ToListAsync();
                if (listaLibros != null)
                {
                    var libros = _mapper.Map<List<LibroDto>>(listaLibros);
                    return libros;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "GetLibrosAsync");
                return null;
            }


        }
        public async Task<LibroDto?> GetLibroAsync(string Isbn)
        {
            try
            {
                var libro = await _Context.Libros.Where(l => l.Isbn == Isbn).FirstOrDefaultAsync();
                if (libro == null)
                {
                    return null;
                }
                return _mapper.Map<LibroDto>(libro);
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "GetLibroAsync");
                return null;
            }
        }


        public async Task<LibroDto> CreateLibroAsync(LibroDto libro)
        {
            try
            {
                var libroNuevo = _mapper.Map<Libro>(libro);
                await _Context.Libros.AddAsync(libroNuevo);
                if (await _Context.SaveChangesAsync() > 0)
                {
                    return _mapper.Map<LibroDto>(libroNuevo);
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "CreateLibroAsync");
                return null;
            }
        }

        public async Task<bool> DeleteLibroAsync(string Isbn)
        {
            try
            {
                var libroParaBorrar = await _Context.Libros
                    .Where(l => l.Isbn.Equals(Isbn))
                    .FirstOrDefaultAsync();

                if (libroParaBorrar != null)
                {
                    _Context.Libros.Remove(libroParaBorrar);
                    var resultado = await _Context.SaveChangesAsync();
                    return resultado > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "DeleteLibroAsync");
                return false; 
            }
            
        }
           
        public async Task<bool> UpdateLibroAsync(LibroDto libro)
        {
            var libroeParaActualizar = _mapper.Map<Libro>(libro);
            _Context.Libros.Update(libroeParaActualizar);

            var resultado = await _Context.SaveChangesAsync();
            return resultado > 0;
        }

        public async Task<bool> ExisteLibroAsync(string Isbn)
        {
            try
            {
                bool existe = await _Context.Libros.AnyAsync(l => l.Isbn == Isbn);
                return existe;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "UpdateLibroAsync");
                return false;
            }
        }

        public async Task<bool> UpdateStockLibroAsync(string isbn, int cantidadVendida)
        {
            try
            {
                bool updateOk = false;
                var libroParaActualizar = await _Context.Libros
                           .Where(l => l.Isbn == isbn)
                           .FirstOrDefaultAsync();

                if (libroParaActualizar != null)
                {
                    libroParaActualizar.Stock -= cantidadVendida;
                    _Context.Libros.Update(libroParaActualizar);
                    updateOk = _Context.SaveChanges() > 0 ? true : false;
                }

                return updateOk;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "UpdateStockLibroAsync");
                return false;
            }           
        }

        public async Task<int> GetStockLibroAsync(string isbn)
        {
            try
            {
                int stock = 0;
                var libro = await _Context.Libros
                           .Where(l => l.Isbn == isbn)
                           .FirstOrDefaultAsync();

                if (libro != null)
                {
                    stock = libro.Stock;
                }
                return stock;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "GetStockLibroAsync");
                return 0;
            }            
        }

        public async Task<decimal> GetPrecioLibroAsync(string isbn)
        {
            try
            {
                decimal PrecioUnitario = await _Context.Libros
                                                 .Where(l => l.Isbn == isbn)
                                                 .Select(l => l.Precio)
                                                 .FirstOrDefaultAsync();
                return PrecioUnitario;
            }
            catch (Exception ex)
            {
                ExceptionLogs(ex, "GetPrecioLibroAsync");
                return decimal.Zero;
            }
        }
        //--------------------------------------
        //Métodos auxiliares
        //--------------------------------------

        ///// LOGS /////

        private void ExceptionLogs(Exception ex, string metodo)
        {
            string mensaje = LogMessages.ExceptionLogMessage(ex, metodo);
            _logger.LogError(mensaje);
        }
    }
}
