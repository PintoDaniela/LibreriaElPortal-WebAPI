using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class DetalleVentaRepository : IDetalleVentaRepository
    {
        private readonly elportalContext _Context;

        public DetalleVentaRepository( elportalContext elportalContext )
        {
            _Context = elportalContext;
        }



        public async Task<bool> ExistenDetallesVentaByLibroAsync(string isbn)
        {
            try
            {
                var existe = await _Context.DetalleVenta.AnyAsync(dv => dv.Isbn == isbn);
                return existe;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}
