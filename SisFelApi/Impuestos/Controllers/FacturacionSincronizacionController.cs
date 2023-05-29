using Microsoft.AspNetCore.Mvc;
using System;
using System.ServiceModel;
using System.Xml;
using ServiceSincronizacion;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;

namespace SisFelApi.Impuestos.Controllers;

[ApiController]
[Route("[controller]")]
public class FacturacionSincronizacionController : ControllerBase
{
    public FacturacionSincronizacionController()
    {
    }

    public string Address = "https://pilotosiatservicios.impuestos.gob.bo/v2/FacturacionSincronizacion?wsdl";

    public static void FinalizarClienteSicronizacion(ClientBase<ServicioFacturacionSincronizacion> cliente)
    {
        try
        {
            if (cliente.State == CommunicationState.Faulted)
            {
                cliente.Abort();
            }
            else
            {
                cliente.Close();
            }
        }
        catch (TimeoutException)
        {
            cliente.Abort();
        }
        catch (CommunicationObjectFaultedException)
        {
            cliente.Abort();
        }
    }

    [HttpPost("/sincronizarActividades")]
    public async Task<respuestaListaActividades> getSincronizarActividades(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarActividadesAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaActividades;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarFechaHora")]
    public async Task<respuestaFechaHora> getSincronizarFechaHora(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarFechaHoraAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaFechaHora;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarListaActividadesDocumentoSector")]
    public async Task<respuestaListaActividadesDocumentoSector> getSincronizarListaActividadesDocumentoSector(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarListaActividadesDocumentoSectorAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaActividadesDocumentoSector;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarListaLeyendasFactura")]
    public async Task<respuestaListaParametricasLeyendas> getSincronizarListaLeyendasFactura(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarListaLeyendasFacturaAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricasLeyendas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarListaMensajesServicios")]
    public async Task<respuestaListaParametricas> getSincronizarListaMensajesServicios(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarListaMensajesServiciosAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarListaProductosServicios")]
    public async Task<respuestaListaProductos> getSincronizarListaProductosServicios(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarListaProductosServiciosAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaProductos;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaEventosSignificativos")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaEventosSignificativos(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaEventosSignificativosAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaMotivoAnulacion")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaMotivoAnulacion(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaMotivoAnulacionAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaPaisOrigen")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaPaisOrigen(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaPaisOrigenAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTipoDocumentoIdentidad")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTipoDocumentoIdentidad(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTipoDocumentoIdentidadAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTipoDocumentoSector")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTipoDocumentoSector(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTipoDocumentoSectorAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTipoEmision")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTipoEmision(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTipoEmisionAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTipoHabitacion")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTipoHabitacion(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTipoHabitacionAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTipoMetodoPago")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTipoMetodoPago(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTipoMetodoPagoAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTipoMoneda")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTipoMoneda(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTipoMonedaAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTipoPuntoVenta")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTipoPuntoVenta(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTipoPuntoVentaAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaTiposFactura")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaTiposFactura(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaTiposFacturaAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }

    [HttpPost("/sincronizarParametricaUnidadMedida")]
    public async Task<respuestaListaParametricas> getSincronizarParametricaUnidadMedida(solicitudSincronizacion request)
    {
        Helper.Configurar(Address);
        ServicioFacturacionSincronizacionClient servicio = new ServicioFacturacionSincronizacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);
        try
        {
            var resp = await servicio.sincronizarParametricaUnidadMedidaAsync(new solicitudSincronizacion() {
                codigoAmbiente = request.codigoAmbiente,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = request.codigoSucursal,
                cuis = request.cuis,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified
            });
            return resp.RespuestaListaParametricas;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        finally
        {
            if (servicio is not null)
                FinalizarClienteSicronizacion(servicio);
        }
        return null;
    }
}