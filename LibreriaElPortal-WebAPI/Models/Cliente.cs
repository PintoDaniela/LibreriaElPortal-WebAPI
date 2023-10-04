using System;
using System.Collections.Generic;

namespace LibreriaElPortal_WebAPI.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Venta = new HashSet<Venta>();
        }

        public int ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }

        public virtual ICollection<Venta> Venta { get; set; }
    }
}
