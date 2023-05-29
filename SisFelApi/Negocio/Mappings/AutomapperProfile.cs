using AutoMapper;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Mappings
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Categorium, CategoriaDto>().ReverseMap();
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Enlace, EnlaceDto>().ReverseMap();
            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Permiso, PermisoDto>().ReverseMap();
            CreateMap<Telefonocliente, TelefonoClienteDto>().ReverseMap();
            CreateMap<Factura, FacturaDto>().ReverseMap();
            CreateMap<Facturadetalle, FacturaDetalleDto>().ReverseMap();
            CreateMap<Sucursal, SucursalDto>().ReverseMap();
            CreateMap<Puntoventum, PuntoVentaDto>().ReverseMap();
            CreateMap<Facturacionmasiva, FacturacionMasivaDto>().ReverseMap();
            CreateMap<FacturaTxt, FacturaTelecomunicaciones>().ReverseMap();
            CreateMap<Registrocompra, RegistroCompraDto>().ReverseMap();
            CreateMap<Paqueterecepcioncompra, PaqueterecepcioncompraDto>().ReverseMap();
            CreateMap<Usuariopuntoventum, UsuarioPuntoVentaDto>().ReverseMap();
        }
    }
}
