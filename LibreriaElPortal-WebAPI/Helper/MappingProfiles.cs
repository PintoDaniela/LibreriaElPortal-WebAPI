using AutoMapper;
using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Models;
using System.Diagnostics.Metrics;

namespace LibreriaElPortal_WebAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Libro, LibroDto>();
            CreateMap<LibroDto, Libro>();
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteDto, Cliente>();
            CreateMap<AgregarClienteDto, Cliente>();
            CreateMap<VentaDto, Venta>();
            CreateMap<Venta, VentaDto>();
            CreateMap<DetalleVentaDto, DetalleVentum>();
            CreateMap<DetalleVentum, DetalleVentaDto>();
        }
    }
}
