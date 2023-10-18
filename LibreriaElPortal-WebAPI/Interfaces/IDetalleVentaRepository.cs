namespace LibreriaElPortal_WebAPI.Interfaces
{
    public interface IDetalleVentaRepository
    {
        Task<bool> ExistenDetallesVentaByLibroAsync(string isbn);
    }
}
