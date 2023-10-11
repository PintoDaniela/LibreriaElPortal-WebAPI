using System;
using System.Collections.Generic;

namespace LibreriaElPortal_WebAPI.Models
{
    public partial class Libro
    {
        public Libro()
        {
            DetalleVenta = new HashSet<DetalleVentum>();
        }

        public string Isbn { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string? Genero { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
    }
}
