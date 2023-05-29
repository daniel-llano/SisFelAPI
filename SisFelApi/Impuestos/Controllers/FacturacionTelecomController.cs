using Microsoft.AspNetCore.Mvc;
using ServiceTelecom;
using SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos;
using SisFelApi.Negocio.Models;
using SisFelApi.Negocio.Controllers;

namespace SisFelApi.Impuestos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturacionTelecomController : ControllerBase
{
    public string Address = "https://pilotosiatservicios.impuestos.gob.bo/v2/ServicioFacturacionTelecomunicaciones?wsdl";

    [HttpPost("verificarComunicacionTelecom")]
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

    [HttpPost("recepcionFactura")]
    public async Task<respuestaRecepcion> getRecepcionFactura(solicitudRecepcionFactura request, int codigoFactura, int codigoPuntoVenta, int capa)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        int tipoFactura = 1;  //Facturas Telecom = 1, Facturas Compra Venta = 2 
        bool existeComunicacion = true;
        await Helper.actualizarCodigos(codigoPuntoVenta);
        await Helper.actualizarNumerosFacturas();
        Helper.evento = await Helper.eventoController.ultimoEvento();

        // VERIFICANDO COMUNICACION
        try
        {
            await servicio.verificarComunicacionAsync();
        }
        catch (Exception e)
        {
           Console.WriteLine($"{e.Message}");
           existeComunicacion = false;
        }

        // ARMANDO FACTURA A ENVIAR
        string xfechaEmision = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        int xcodigoEmision = request.codigoEmision;

        FacturaController controllerFacturaBD = new FacturaController(Helper.context, Helper._mapper);
        Factura facturaBD = await controllerFacturaBD.factura(codigoFactura);

        if(facturaBD != null)
        {
            // Cargando datos estaticos en la factura de la BD
            facturaBD.Nitemisor = 1024061022;
            facturaBD.Municipio = "Tarija";
            facturaBD.Telefonoemisor = 46643010;
            facturaBD.Cuf = Helper.obtenerCuf(tipoFactura, xfechaEmision, facturaBD.Numerofactura, codigoPuntoVenta, Helper.general.Codigocontrol, xcodigoEmision);
            facturaBD.Cufd = Helper.general.Cufd;
            facturaBD.Codigosucursal = 0; // Agregar sucursal 0;
            facturaBD.Direccion = "Calle Suipacha N 484 Barrio Las Panosas";
            facturaBD.Fechaemision = DateTime.Parse(xfechaEmision);

            FacturaTelecomunicaciones factura = new FacturaTelecomunicaciones();

            if((bool)!Helper.evento.Activo)
            {
                factura.cafc = null;
            }
            else
            {
                factura.cafc = Helper.evento.Cafctelecom;
                xcodigoEmision = 2;

            }

            factura.nitEmisor = (long)facturaBD.Nitemisor;
            factura.razonSocialEmisor = "COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R. L.";
            factura.municipio = facturaBD.Municipio;
            factura.telefono = facturaBD.Telefonoemisor+"";
            factura.nitConjunto = null;
            factura.numeroFactura = facturaBD.Numerofactura;
            factura.cuf = facturaBD.Cuf;
            factura.cufd = facturaBD.Cufd;
            factura.codigoSucursal = facturaBD.Codigosucursal;
            factura.direccion = facturaBD.Direccion;
            factura.codigoPuntoVenta = facturaBD.Codigopuntoventa;
            factura.fechaEmision = xfechaEmision;
            factura.nombreRazonSocial = facturaBD.Nombrerazonsocial;
            factura.codigoTipoDocumentoIdentidad = facturaBD.Codigotipodocumentoidentidad;
            factura.numeroDocumento = facturaBD.Numerodocumento;


            if(facturaBD.Complemento == null || facturaBD.Complemento.Trim() == "")
            {
                factura.complemento = null;
            }
            else 
            {
                factura.complemento = facturaBD.Complemento;
            }
            
            factura.codigoCliente = facturaBD.CodigotelefonoclienteNavigation.Telefono + "";
            factura.codigoMetodoPago = facturaBD.Codigometodopago; 
            factura.montoTotal = (float)facturaBD.Montototal;
            factura.montoTotalSujetoIva = (float)facturaBD.Montototalsujetoiva;
            factura.descuentoAdicional = (float)facturaBD.Descuentoadicional;

            factura.codigoMoneda = facturaBD.Codigomoneda;
            factura.tipoCambio = 1;

            factura.montoTotalMoneda = (float)facturaBD.Montototal;

            factura.leyenda = "Ley N° 453: Tienes derecho a recibir información sobre las características y contenidos de los servicios que utilices.";
            factura.usuario = facturaBD.Usuario;
            factura.codigoDocumentoSector = 22;

            facturaBD.Leyenda = factura.leyenda;

            // Detalle 
            //lista de detalles factura
            List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
            

            foreach(Facturadetalle detalle in facturaBD.Facturadetalles)
            {
                FacturaDetalleGral facturaDetalleGral = new FacturaDetalleGral();
                facturaDetalleGral.actividadEconomica = detalle.Actividadeconomica;
                facturaDetalleGral.codigoProductoSin = detalle.Codigoproductosin;
                facturaDetalleGral.codigoProducto = detalle.Codigoproducto;
                facturaDetalleGral.descripcion = detalle.Descripcion;
                facturaDetalleGral.cantidad = detalle.Cantidad;
                facturaDetalleGral.unidadMedida = detalle.Unidadmedida;
                facturaDetalleGral.precioUnitario = (float)detalle.Preciounitario;
                facturaDetalleGral.montoDescuento = (float)detalle.Montodescuento;
                facturaDetalleGral.subTotal = (float)detalle.Subtotal;
                listaDetalles.Add(facturaDetalleGral);
            }
            
            factura.detalles = listaDetalles;

            if (existeComunicacion && (bool)!Helper.evento.Activo)
            {
                //PROCESO DE FACTURACION INDIVIDUAL EN LINEA
                
                await Helper.actualizarPathFactura(tipoFactura, capa, 1);

                //Realizando el proceso de serializacion, firma y compresion del archivo
                if (!await Helper.facturar(factura, tipoFactura, 1, 1, ""))
                {
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Error: Por favor verificar que el programa Jacobitus Total este abierto y funcionando correctamente."
                    };
                }

                string xarchivo = Helper.obtenerStringBase64DeArchivo(Helper.pathArchivoCompreso);
                string xhashArchivo = Helper.obtenerHashDeArchivo(Helper.pathArchivoCompreso);

                try
                {
                    var resp = await servicio.recepcionFacturaAsync(new solicitudRecepcionFactura()
                    {
                        archivo = xarchivo,
                        codigoAmbiente = request.codigoAmbiente,
                        codigoDocumentoSector = request.codigoDocumentoSector,
                        codigoEmision = request.codigoEmision,
                        codigoModalidad = request.codigoModalidad,
                        codigoPuntoVenta = codigoPuntoVenta,
                        codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified,
                        codigoSistema = "723755F6B4AE27F5D61A57E",
                        codigoSucursal = request.codigoSucursal,
                        cufd = Helper.general.Cufd,
                        cuis = Helper.general.Cuis,
                        fechaEnvio = factura.fechaEmision,
                        hashArchivo = xhashArchivo,
                        nit = 1024061022,
                        tipoFacturaDocumento = request.tipoFacturaDocumento
                    });

                    Console.WriteLine("CUF:" + factura.cuf);
                    facturaBD.Codigorecepcion = resp.RespuestaServicioFacturacion.codigoRecepcion;
                    
                    await controllerFacturaBD.actualizarFactura(facturaBD);
                    return resp.RespuestaServicioFacturacion;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Error: Ocurrio un error inesperado por favor comuniquese con el personal de soporte."
                    };
                    
                }
            }
            else
            {
                // PROCESO DE FACTURACION INDIVIDUAL FUERA DE LINEA
                
                // Verificando si es la primera factura 
                if (Helper.datosbase.Nrofacturapaquetetl == 1 && Helper.datosbase.Nropaquetetl == 1 && (bool)!Helper.evento.Activo)
                {
                    // Solicitamos la inicializacion de un Evento
                    return new respuestaRecepcion(){
                        codigoEstado = 50,
                        codigoDescripcion = "Error: Se presentaron problemas al acceder al SIN, por favor registre un evento significativo."
                    };
                }

                await Helper.actualizarPathFactura(tipoFactura, capa, 2);

                //Realizando el proceso de serializacion, firma y compresion del archivo
                if (!await Helper.facturar(factura, tipoFactura, 2, 1, ""))
                {
                    return new respuestaRecepcion(){
                        codigoEstado = 52,
                        codigoDescripcion = "Error: Por favor verificar que el programa Jacobitus Total este abierto y funcionando correctamente."
                    };
                }

                //Actualizando numero de factura en Paquete
                Helper.datosbase.Nrofacturapaquetetl = Helper.datosbase.Nrofacturapaquetetl + 1;

                // GENERACION DE PAQUETE DE FACTURAS FUERA DE LINEA
                if (Helper.datosbase.Nrofacturapaquetetl == Helper.numeroLimiteDeFacturasPorPaquete + 1)
                {
                    Helper.empaquetarFacturas("Impuestos/XML/FacturacionPaquetes/FacturaTelecomunicacion", Helper.pathArchivoCompreso);

                    Helper.datosbase.Nrofacturapaquetetl = 1;
                    Helper.datosbase.Nropaquetetl = Helper.datosbase.Nropaquetetl + 1;

                    Helper.eliminarArchivos("Impuestos/XML/FacturacionPaquetes/FacturaTelecomunicacion");
                }

                await Helper.datosBaseController.actualizarDatosBase(Helper.datosbase);

                return new respuestaRecepcion(){
                    codigoEstado = 51,
                    codigoDescripcion = "Exito: Factura Generada dentro de un Evento Significativo."
                };
            }

        }else{
            return new respuestaRecepcion(){
                codigoEstado = 52,
                codigoDescripcion = "ERROR: NO SE PUDO ENCONTRAR LA FACTURA."
            };
        }
    }

    [HttpPost("recepcionPaqueteFactura")]
    public async Task<respuestaRecepcion> getRecepcionPaqueteFactura(solicitudRecepcionPaquete request, int puntoVenta, string pathPaqueteCompreso)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(puntoVenta);
        Helper.evento = await Helper.eventoController.ultimoEvento();

        string xarchivo = Helper.obtenerStringBase64DeArchivo(pathPaqueteCompreso);
        string xhashArchivo = Helper.obtenerHashDeArchivo(pathPaqueteCompreso);

        try
        {
            var resp = await servicio.recepcionPaqueteFacturaAsync(new solicitudRecepcionPaquete()
            {
                archivo = xarchivo,
                cafc = Helper.evento.Cafctelecom,
                cantidadFacturas = request.cantidadFacturas,
                codigoAmbiente = request.codigoAmbiente,
                codigoDocumentoSector = request.codigoDocumentoSector,
                codigoEmision = request.codigoEmision,
                codigoEvento = Helper.evento.Codigorecepcionevento,
                codigoModalidad = request.codigoModalidad,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                fechaEnvio = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                hashArchivo = xhashArchivo,
                nit = 1024061022,
                tipoFacturaDocumento = request.tipoFacturaDocumento
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("recepcionMasivaFactura")]
    public async Task<respuestaRecepcion> getRecepcionMasivaFactura(solicitudRecepcionMasiva request, int codigoPuntoVenta, string pathPaqueteCompreso)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        string xarchivo = Helper.obtenerStringBase64DeArchivo(pathPaqueteCompreso);
        string xhashArchivo = Helper.obtenerHashDeArchivo(pathPaqueteCompreso);

        try
        {
            var resp = await servicio.recepcionMasivaFacturaAsync(new solicitudRecepcionMasiva()
            {
                archivo = xarchivo,
                cantidadFacturas = request.cantidadFacturas,
                codigoAmbiente = request.codigoAmbiente,
                codigoDocumentoSector = request.codigoDocumentoSector,
                codigoEmision = request.codigoEmision,
                codigoModalidad = request.codigoModalidad,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                fechaEnvio = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                hashArchivo = xhashArchivo,
                nit = 1024061022,
                tipoFacturaDocumento = request.tipoFacturaDocumento
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("anulacionFactura")]
    public async Task<respuestaRecepcion> getAnulacionFactura(solicitudAnulacion request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.anulacionFacturaAsync(new solicitudAnulacion()
            {
                codigoAmbiente = request.codigoAmbiente,
                codigoDocumentoSector = request.codigoDocumentoSector,
                codigoEmision = request.codigoEmision,
                codigoModalidad = request.codigoModalidad,
                codigoMotivo = request.codigoMotivo,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cuf = request.cuf,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                nit = 1024061022,
                tipoFacturaDocumento = request.tipoFacturaDocumento
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("verificacionEstadoFactura")]
    public async Task<respuestaRecepcion> getVerificacionEstadoFactura(solicitudVerificacionEstado request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.verificacionEstadoFacturaAsync(new solicitudVerificacionEstado()
            {
                codigoAmbiente = request.codigoAmbiente,
                codigoDocumentoSector = request.codigoDocumentoSector,
                codigoEmision = request.codigoEmision,
                codigoModalidad = request.codigoModalidad,
                codigoPuntoVenta = request.codigoPuntoVenta,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cuf = request.cuf,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                nit = 1024061022,
                tipoFacturaDocumento = request.tipoFacturaDocumento
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("validacionRecepcionPaqueteFactura")]
    public async Task<respuestaRecepcion> getValidacionRecepcionPaqueteFactura(solicitudValidacionRecepcion request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.validacionRecepcionPaqueteFacturaAsync(new solicitudValidacionRecepcion()
            {
                codigoAmbiente = request.codigoAmbiente,
                codigoDocumentoSector = request.codigoDocumentoSector,
                codigoEmision = request.codigoEmision,
                codigoModalidad = request.codigoModalidad,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified,
                codigoRecepcion = request.codigoRecepcion,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                nit = 1024061022,
                tipoFacturaDocumento = request.tipoFacturaDocumento
            });
            return resp.RespuestaServicioFacturacion;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
        return null;
    }

    [HttpPost("validacionRecepcionMasivaFactura")]
    public async Task<respuestaRecepcion> getValidacionRecepcionMasivaFactura(solicitudValidacionRecepcion request, int codigoPuntoVenta)
    {
        Helper.Configurar(Address);
        ServicioFacturacionClient servicio = new ServicioFacturacionClient(Helper.binding, Helper.address);
        servicio.Endpoint.EndpointBehaviors.Add(Helper.behaviour);

        await Helper.actualizarCodigos(codigoPuntoVenta);

        try
        {
            var resp = await servicio.validacionRecepcionMasivaFacturaAsync(new solicitudValidacionRecepcion()
            {
                codigoAmbiente = request.codigoAmbiente,
                codigoDocumentoSector = request.codigoDocumentoSector,
                codigoEmision = request.codigoEmision,
                codigoModalidad = request.codigoModalidad,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoPuntoVentaSpecified = request.codigoPuntoVentaSpecified,
                codigoRecepcion = request.codigoRecepcion,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                codigoSucursal = request.codigoSucursal,
                cufd = Helper.general.Cufd,
                cuis = Helper.general.Cuis,
                nit = 1024061022,
                tipoFacturaDocumento = request.tipoFacturaDocumento
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