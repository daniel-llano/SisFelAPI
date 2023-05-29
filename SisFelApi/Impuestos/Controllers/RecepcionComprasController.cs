using Microsoft.AspNetCore.Mvc;
using ServiceCompras;

namespace SisFelApi.Impuestos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecepcionComprasController : ControllerBase
{

    public string Address = "https://pilotosiatservicios.impuestos.gob.bo/v2/ServicioRecepcionCompras?wsdl";

    public RecepcionComprasController()
    {
    }

    [HttpPost("verificarComunicacionRecepcionCompras")]
    public async Task<respuestaComunicacion> getVerificarComunicacion(verificarComunicacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.verificarComunicacionAsync();
            return resp.@return;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }


    [HttpPost("validacionRecepcionPaqueteCompras")]
    public async Task<respuestaRecepcion> getValidacionRecepcionPaqueteCompras(solicitudValidacionRecepcionCompras request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        
        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.validacionRecepcionPaqueteComprasAsync(new solicitudValidacionRecepcionCompras(){
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoRecepcion = request.codigoRecepcion,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                nit = 1024061022                
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }


    [HttpPost("recepcionPaqueteCompras")]
    public async Task<respuestaRecepcion> getRecepcionPaqueteCompras(solicitudRecepcionCompras request, int codigoPuntoVenta, string pathPaqueteCompreso)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        string xarchivo = Helper.obtenerStringBase64DeArchivo(pathPaqueteCompreso);
        string xhashArchivo = Helper.obtenerHashDeArchivo(pathPaqueteCompreso);

        try
        {
            var resp = await servicio.recepcionPaqueteComprasAsync(new solicitudRecepcionCompras(){
                archivo = xarchivo,
                cantidadFacturas = request.cantidadFacturas,
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                fechaEnvio = DateTime.Now,
                gestion = request.gestion,
                hashArchivo = xhashArchivo,
                nit = 1024061022,
                periodo = request.periodo                 
            });
            Console.WriteLine(resp.RespuestaServicioFacturacion.ToString());
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }


    [HttpPost("anulacionCompra")]
    public async Task<respuestaRecepcion> getAnulacionCompra(solicitudAnulacionCompra request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.anulacionCompraAsync(new solicitudAnulacionCompra(){
                codAutorizacion = request.codAutorizacion,
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                nit = 1024061022,
                nitProveedor = request.nitProveedor,
                nroDuiDim = request.nroDuiDim,
                nroFactura = request.nroFactura
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }


    [HttpPost("confirmacionCompras")]
    public async Task<respuestaRecepcion> getConfirmacionCompras(solicitudRecepcionCompras request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.confirmacionComprasAsync(new solicitudRecepcionCompras(){
                archivo = request.archivo,
                cantidadFacturas = request.cantidadFacturas,
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                fechaEnvio = request.fechaEnvio,
                gestion = request.gestion,
                hashArchivo = request.hashArchivo,
                nit = 1024061022,
                periodo = request.periodo
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }


    [HttpPost("consultaCompras")]
    public async Task<respuestaConsultaCompras> getConsultaCompras(solicitudConsultaCompras request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.consultaComprasAsync(new solicitudConsultaCompras(){
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                nit = 1024061022,
                fecha = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"))
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

}