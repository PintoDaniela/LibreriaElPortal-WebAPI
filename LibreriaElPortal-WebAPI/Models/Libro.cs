using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public string? Autor { get; set; }
        public string? Genero { get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
        [JsonIgnore]
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
    }
}
