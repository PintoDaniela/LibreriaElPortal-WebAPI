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
            CreateMap<Cliente, AgregarClienteDto>();
            CreateMap<VentaDto, Venta>();
            CreateMap<Venta, VentaDto>();
            CreateMap<Venta, AgregarVentaDto>();
            CreateMap<AgregarVentaDto, VentaDto>();
            CreateMap<AgregarVentaDto, Venta>();
            CreateMap<VentaDto, AgregarVentaDto>();
            CreateMap<DetalleVentaDto, DetalleVentum>();
            CreateMap<DetalleVentum, DetalleVentaDto>();
            CreateMap<DetalleVentum, AgregarDetalleVentaDto>();
            CreateMap<AgregarDetalleVentaDto, DetalleVentum>();
        }
    }
}
