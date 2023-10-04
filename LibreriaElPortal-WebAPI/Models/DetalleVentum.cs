using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibreriaElPortal_WebAPI.Models
{
    public partial class DetalleVentum
    {
        public int DetalleVentaId { get; set; }
        public int? VentaId { get; set; }
        public string? Isbn { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }        
        public virtual Libro? IsbnNavigation { get; set; }
        public virtual Venta? Venta { get; set; }
    }
}
