using Microsoft.AspNetCore.Mvc;
using ServiceCodigos;

namespace SisFelApi.Impuestos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturacionCodigosController : ControllerBase
{
    public string Address = "https://pilotosiatservicios.impuestos.gob.bo/v2/FacturacionCodigos?wsdl";
    public FacturacionCodigosController()
    {
    }

    [HttpPost("cuis")]
    public async Task<respuestaCuis> getCuis(solicitudCuis request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionCodigosClient servicio = new ServicioFacturacionCodigosClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.cuisAsync(new solicitudCuis() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                codigoModalidad = request.codigoModalidad,
                codigoPuntoVentaSpecified = true
            });
            return resp.RespuestaCuis;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("cufd")]
    public async Task<respuestaCufd> getCufd(solicitudCufd request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionCodigosClient servicio = new ServicioFacturacionCodigosClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.cufdAsync(new solicitudCufd() {
                cuis = request.cuis, 
                codigoAmbiente = 2,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                codigoModalidad = request.codigoModalidad,
                codigoPuntoVentaSpecified = true
            });
            return resp.RespuestaCufd;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("verificarNit")]
    public async Task<respuestaVerificarNit> getVerificarNit(solicitudVerificarNit request)
    {
        Helper.Configurar(Address);
        await Helper.actualizarCodigos(0);
        ServicioFacturacionCodigosClient servicio = new ServicioFacturacionCodigosClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
                var resp = await servicio.verificarNitAsync (new solicitudVerificarNit() {
                cuis = Helper.general.Cuis, 
                codigoAmbiente = request.codigoAmbiente,
                nitParaVerificacion = request.nitParaVerificacion,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                codigoModalidad = request.codigoModalidad
            });
            return resp.RespuestaVerificarNit;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("notificaCertificadoRevocado")]
    public async Task<respuestaNotificaRevocado> getNotificaCertificadoRevocado(solicitudNotifcaRevocado request)
    {
        Helper.Configurar(Address);
        await Helper.actualizarCodigos(0);
        ServicioFacturacionCodigosClient servicio = new ServicioFacturacionCodigosClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
                var resp = await servicio.notificaCertificadoRevocadoAsync (new solicitudNotifcaRevocado() {
                cuis = Helper.general.Cuis, 
                codigoAmbiente = request.codigoAmbiente,
                certificado = request.certificado,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                fechaRevocacion = request.fechaRevocacion,
                razonRevocacion = request.razonRevocacion,
            });
            return resp.RespuestaNotificaRevocado;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }
    [HttpPost("cuisMasivo")]
    public async Task<respuestaCuisMasivo> getCuisMasivo(solicitudCuisMasivoSistemas request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionCodigosClient servicio = new ServicioFacturacionCodigosClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
                var resp = await servicio.cuisMasivoAsync (new solicitudCuisMasivoSistemas() { 
                codigoAmbiente = request.codigoAmbiente,
                datosSolicitud = request.datosSolicitud,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoModalidad = request.codigoModalidad
            });
            return resp.RespuestaCuisMasivo;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }
    
    [HttpPost("cufdMasivo")]
    public async Task<respuestaCufdMasivo> getCudfMasivo(solicitudCufdMasivo request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionCodigosClient servicio = new ServicioFacturacionCodigosClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
                var resp = await servicio.cufdMasivoAsync (new solicitudCufdMasivo() { 
                codigoAmbiente = request.codigoAmbiente,
                datosSolicitud = request.datosSolicitud,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoModalidad = request.codigoModalidad
            });
            return resp.RespuestaCufdMasivo;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("verificarComunicacion")]
    public async Task<respuestaComunicacion> getverificarComunicacion(verificarComunicacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionCodigosClient servicio = new ServicioFacturacionCodigosClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.verificarComunicacionAsync();

            return resp.RespuestaComunicacion;

        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }
}