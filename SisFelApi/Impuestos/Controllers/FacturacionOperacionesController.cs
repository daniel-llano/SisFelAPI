using Microsoft.AspNetCore.Mvc;
using ServiceOperaciones;
using System.ServiceModel;
using System.Xml;

namespace SisFelApi.Impuestos.Controllers;

[ApiController]
[Route("[controller]")]
public class FacturacionOperacionesController : ControllerBase
{
    /*private readonly ILogger<FacturacionOperacionesController> _logger;

    public FacturacionOperacionesController(ILogger<FacturacionOperacionesController> logger)
    {
        _logger = logger;
    }*/
    public string Address = "https://pilotosiatservicios.impuestos.gob.bo/v2/FacturacionOperaciones?wsdl";
    public FacturacionOperacionesController()
    {
    }

    [HttpPost("/registroPuntoVenta")]
    public async Task<respuestaRegistroPuntoVenta> postPuntodeVenta(solicitudRegistroPuntoVenta request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionOperacionesClient servicio = new ServicioFacturacionOperacionesClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.registroPuntoVentaAsync(new solicitudRegistroPuntoVenta() {
                codigoAmbiente = request.codigoAmbiente,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                codigoModalidad = request.codigoModalidad,
                cuis = request.cuis,
                descripcion = request.descripcion,
                nombrePuntoVenta = request.nombrePuntoVenta,
                codigoTipoPuntoVenta = request.codigoTipoPuntoVenta
            });
            return resp.RespuestaRegistroPuntoVenta;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("/consultaPuntoVenta")]
    public async Task<respuestaConsultaPuntoVenta> getPuntoVenta(solicitudConsultaPuntoVenta request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionOperacionesClient servicio = new ServicioFacturacionOperacionesClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.consultaPuntoVentaAsync(new solicitudConsultaPuntoVenta() {
                codigoAmbiente = request.codigoAmbiente,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis
            });
            return resp.RespuestaConsultaPuntoVenta;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("/cierrePuntoVenta")]
    public async Task<respuestaCierrePuntoVenta> deletePuntoVenta(solicitudCierrePuntoVenta request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionOperacionesClient servicio = new ServicioFacturacionOperacionesClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.cierrePuntoVentaAsync(new solicitudCierrePuntoVenta() {
                codigoAmbiente = request.codigoAmbiente,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVenta = request.codigoPuntoVenta
            });
            return resp.RespuestaCierrePuntoVenta;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }
    [HttpPost("/registroEventoSignificativo")]
    public async Task<respuestaListaEventos> registroEventoSignificativo(solicitudEventoSignificativo request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionOperacionesClient servicio = new ServicioFacturacionOperacionesClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            
            var resp = await servicio.registroEventoSignificativoAsync(new solicitudEventoSignificativo() {
                codigoAmbiente = request.codigoAmbiente,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoMotivoEvento= request.codigoMotivoEvento,
                cufd = request.cufd,
                cufdEvento = request.cufdEvento,
                descripcion = request.descripcion,
                fechaHoraInicioEvento = request.fechaHoraInicioEvento,
                fechaHoraFinEvento = request.fechaHoraFinEvento,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified

            });
            return resp.RespuestaListaEventos;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("/consultaEventoSignificativo")]
    public async Task<respuestaListaEventos> consultaEventoSignificativo(solicitudConsultaEvento request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionOperacionesClient servicio = new ServicioFacturacionOperacionesClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            
            var resp = await servicio.consultaEventoSignificativoAsync(new solicitudConsultaEvento() {
                codigoAmbiente = request.codigoAmbiente,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVenta = request.codigoPuntoVenta,
                cufd = request.cufd,
                fechaEvento = request.fechaEvento

            });
            return resp.RespuestaListaEventos;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("/cierreOperacionesSistema")]
    public async Task<respuestaCierreSistemas> cierreOperacionesSistema(solicitudOperaciones request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionOperacionesClient servicio = new ServicioFacturacionOperacionesClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            
            var resp = await servicio.cierreOperacionesSistemaAsync(new solicitudOperaciones() {
                codigoAmbiente = request.codigoAmbiente,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoModalidad = request.codigoModalidad,

            });
            return resp.RespuestaCierreSistemas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("/registroPuntoVentaComisionista")]
    public async Task<respuestaPuntoVentaComisionista> registroPuntoVentaComisionista(solicitudPuntoVentaComisionista request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionOperacionesClient servicio = new ServicioFacturacionOperacionesClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            
            var resp = await servicio.registroPuntoVentaComisionistaAsync(new solicitudPuntoVentaComisionista() {
                codigoAmbiente = request.codigoAmbiente,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                nitComisionista = request.nitComisionista,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoModalidad = request.codigoModalidad,
                fechaInicio = request.fechaInicio,
                fechaFin = request.fechaFin,
                numeroContrato = request.numeroContrato,
                nombrePuntoVenta = request.nombrePuntoVenta

            });
            return resp.RespuestaPuntoVentaComisionista;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }
    
}