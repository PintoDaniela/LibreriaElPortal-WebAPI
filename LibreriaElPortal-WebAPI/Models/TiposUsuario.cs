using System;
using System.Collections.Generic;

namespace LibreriaElPortal_WebAPI.Models
{
    public partial class TiposUsuario
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}
