using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly elportalContext _context;
        private readonly IMapper _mapper;

        public LibroRepository(elportalContext elportalContext, IMapper mapper)
        {
            _context = elportalContext;
            _mapper = mapper;
        }



        public async Task<ICollection<LibroDto>?> GetLibrosAsync()
        {
            try
            {
                var listaLibros = await _context.Libros.ToListAsync();
                if (listaLibros != null)
                {
                    var libros = _mapper.Map<List<LibroDto>>(listaLibros);
                    return libros;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            
           
        }
        public async Task<LibroDto?> GetLibroAsync(string Isbn)
        {
            try
            {
                var libro = await _context.Libros.Where(l => l.Isbn == Isbn).FirstOrDefaultAsync();
                if (libro == null)
                {
                    return null;
                }
                return _mapper.Map<LibroDto>(libro);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<LibroDto> CreateLibroAsync(LibroDto libro)
        {
            try
            {
                var libroParaCrear = _mapper.Map<Libro>(libro);
                var nuevoLibro = await _context.Libros.AddAsync(libroParaCrear);
                if (nuevoLibro != null)
                {
                    return _mapper.Map<LibroDto>(nuevoLibro);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteLibroAsync(string Isbn)
        {
            try
            {
                var libroParaBorrar = await _context.Libros.Where(l => l.Isbn.Equals(Isbn)).FirstOrDefaultAsync();
                if (libroParaBorrar != null)
                {
                    _context.Libros.Remove(libroParaBorrar);
                    var resultado = await _context.SaveChangesAsync();
                    return resultado > 0;
                }
                return false;
            }
            catch 
            { 
                return false; 
            }
            
        }
           
        public async Task<bool> UpdateLibroAsync(LibroDto libro)
        {
            var libroeParaActualizar = _mapper.Map<Libro>(libro);
            _context.Libros.Update(libroeParaActualizar);

            var resultado = await _context.SaveChangesAsync();
            return resultado > 0;
        }

        public async Task<bool> ExisteLibro(string Isbn)
        {
            try
            {
                bool existe = await _context.Libros.AllAsync(l => l.Isbn == Isbn);
                return existe;
            }
            catch
            {
                return false;
            }
        }
    }
}
