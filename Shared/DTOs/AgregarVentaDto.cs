﻿namespace Shared.DTOs
{
    public class AgregarVentaDto
    {       
        public int ClienteId { get; set; }        
        public ICollection<AgregarDetalleVentaDto> Detalles { get; set; } = new List<AgregarDetalleVentaDto>();
    }
}
