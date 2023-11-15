namespace Shared.DTOs
{
    public class RespuestaAPI
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Mensaje { get; set; } = string.Empty;        
        public List<DetalleError> Errores { get; set; } = new List<DetalleError>();
        public object Resultado { get; set; }
    }
}
