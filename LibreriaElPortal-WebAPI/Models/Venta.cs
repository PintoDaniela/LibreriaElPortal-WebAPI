using System;
using System.Collections.Generic;

namespace LibreriaElPortal_WebAPI.Models
{
    public partial class Venta
    {
        public Venta()
        {
            DetalleVenta = new HashSet<DetalleVentum>();
        }

        public int VentaId { get; set; }
        public DateTime FechaVenta { get; set; }
        public int? ClienteId { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
    }
}
