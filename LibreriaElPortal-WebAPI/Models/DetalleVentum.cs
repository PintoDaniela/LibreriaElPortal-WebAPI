using System;
using System.Collections.Generic;

namespace LibreriaElPortal_WebAPI.Models
{
    public partial class DetalleVentum
    {
        public int DetalleVentaId { get; set; }
        public int VentaId { get; set; }
        public string Isbn { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public virtual Libro IsbnNavigation { get; set; } = null!;
        public virtual Venta Venta { get; set; } = null!;
    }
}
