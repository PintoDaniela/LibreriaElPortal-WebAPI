namespace Shared.DTOs
{
    public class RespuestaAPI
    {
        public int StatusCode { get; set; }
        public string DescripcionStatusCode{ get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public List<DetalleError> Errores { get; set; } = new List<DetalleError>();
    }
}
