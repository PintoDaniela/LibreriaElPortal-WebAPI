using LibreriaElPortal_WebAPI.Helper;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreriaElPortal_WebAPI.Repositories
{
    public class DetalleVentaRepository : IDetalleVentaRepository
    {
        private readonly elportalContext _Context;
        private readonly ILogger<DetalleVentaRepository> _logger;

        public DetalleVentaRepository( elportalContext elportalContext, ILogger<DetalleVentaRepository> logger )
        {
            _Context = elportalContext;
            _logger = logger;
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
                ExceptionLogs(ex, "ExistenDetallesVentaByLibroAsync");
                return false;
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
