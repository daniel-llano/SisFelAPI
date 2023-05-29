using AutoMapper;
using Correos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Helpers;
using SisFelApi.Negocio.Models;
using SisFelApi.Impuestos.Controllers;
using ServiceCompraVenta;
using SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos;

namespace SisFelApi.Negocio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public FacturaController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //trae la lista de facturas en gral si no se manda los parámetros caso contrario trae las facturas en específico mandando el código teléfono del cliente.
        [HttpGet("lista")]
        public async Task<ActionResult<List<Factura>>> listaFacturas(string? codigoTelefonoCliente, string? estado)
        {
            ConversionList conversionList = new ConversionList();
            if (codigoTelefonoCliente!="0" && estado==null)
            {
                var lista = await _context.Facturas.Include(z => z.CodigotelefonoclienteNavigation).Include(z => z.Facturadetalles).Where(x => x.Codigotelefonocliente == codigoTelefonoCliente && x.EstadoFactura.Equals("PENDIENTE")).ToListAsync();
                return Ok(conversionList.conversionListFactura(lista));
            }
            else if (codigoTelefonoCliente==null && estado != null)
            {
                    var lista = await _context.Facturas.Include(z => z.CodigotelefonoclienteNavigation).Include(z=>z.Facturadetalles).Where(x => x.EstadoFactura.Equals(estado)).ToListAsync();
                    return Ok(conversionList.conversionListFactura(lista));
            }
            else if(codigoTelefonoCliente == null && estado==null)
            {
                var lista = await _context.Facturas.Include(z => z.CodigotelefonoclienteNavigation).Include(z => z.Facturadetalles).ToListAsync();
                return Ok(conversionList.conversionListFactura(lista));
            }
            return Ok();
        }
        //trae una factura en especifico segun el codigo de las misma
        [HttpGet("id")]
        public async Task<Factura> factura(int codigoFactura)
        {
            
            var obj = await _context.Facturas.Include(z => z.CodigotelefonoclienteNavigation).Include(z => z.Facturadetalles).FirstOrDefaultAsync(x=>x.Codigofactura.Equals(codigoFactura));
            return obj;
        }

        //agrega una nueva factura mandando el objeto correspondiente de tipo FacturaDto
        [HttpPost("agregar")]
        public async Task<Factura> agregarFactura(FacturaDto objDto)
        {
            //objDto.Fechaemision= DateTime.Now;
            var obj = _mapper.Map<Factura>(objDto);

            if(objDto.Codigofactura != 0){
                obj.EstadoFactura = "PENDIENTE";
                _context.Facturas.Update(obj);
                await _context.SaveChangesAsync();
            }else{
                _context.Facturas.Add(obj);
                await _context.SaveChangesAsync();
            }

            
            return await _context.Facturas.FindAsync(obj.Codigofactura);
        }

        //modifica una nueva factura en especifico mandando el objeto correspondiente de tipo FacturaDto
        [HttpPut("modificar")]
        public async Task<FacturaDto> actualizarFactura(FacturaDto objDto)
        {
            Factura obj = await _context.Facturas.FindAsync(objDto.Codigofactura);
            
            // Mapeando objeto 
            // obj = _mapper.Map<Factura>(objDto); Provoca error de tracker
            obj.Nitemisor = objDto.Nitemisor;
            obj.Municipio = objDto.Municipio;
            obj.Telefonoemisor = objDto.Telefonoemisor;
            obj.Nitconjunto = null;
            obj.Numerofactura = objDto.Numerofactura;
            obj.Cuf = objDto.Cuf;//observado la fecha actual
            obj.Cufd = objDto.Cufd;
            obj.Codigosucursal = objDto.Codigosucursal;
            obj.Direccion = objDto.Direccion;
            obj.Codigopuntoventa = objDto.Codigopuntoventa;
            obj.Fechaemision = objDto.Fechaemision;
            obj.Nombrerazonsocial = objDto.Nombrerazonsocial;
            obj.Codigotipodocumentoidentidad = objDto.Codigotipodocumentoidentidad;
            obj.Numerodocumento = objDto.Numerodocumento;
            obj.Complemento = objDto.Complemento;
            obj.Codigotelefonocliente = objDto.Codigotelefonocliente;
            obj.Codigometodopago = objDto.Codigometodopago;
            obj.Montototal = objDto.Montototal;
            obj.Montototalsujetoiva = objDto.Montototalsujetoiva;
            obj.Leyenda = objDto.Leyenda;
            obj.Usuario = objDto.Usuario;
            obj.Codigodocumentosector = objDto.Codigodocumentosector;
            obj.Cafc = objDto.Cafc;
            obj.Codigorecepcion = objDto.Codigorecepcion;
            obj.Nrotarjeta = objDto.Nrotarjeta;
            obj.Codigomoneda = objDto.Codigomoneda;
            obj.EstadoFactura = objDto.EstadoFactura;
            obj.Descuentoadicional = objDto.Descuentoadicional;
            obj.Auditoriafacturas = obj.Auditoriafacturas;
            obj.Facturadetalles = obj.Facturadetalles;
            obj.CodigopuntoventaNavigation = obj.CodigopuntoventaNavigation;
            obj.CodigosucursalNavigation = obj.CodigosucursalNavigation;
            obj.CodigotelefonoclienteNavigation = obj.CodigotelefonoclienteNavigation;
            obj.UsuarioNavigation = obj.UsuarioNavigation;

            _context.Facturas.Update(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<FacturaDto>(obj);
            return objDto;
        }

        [HttpPut("modificarFacturaCompleta")]
        public async Task<ActionResult> actualizarFactura(Factura obj)
        {
            _context.Facturas.Update(obj);
            await _context.SaveChangesAsync();
            return Created($"/id?codigoFactura={obj.Codigofactura}", obj);
        }



        //cambia el estado de una factura mandando el string especificamente: "PENDIENTE", "CANCELADA","ANULADA"  y el código de la misma
        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoFactura(int CodigoFactura, string estado)
        {
            Factura obj = await _context.Facturas.FindAsync(CodigoFactura);

            if (obj == null)
            {
                return NotFound();
            }
            if (estado.Equals("CANCELADA"))
            {
                obj.EstadoFactura = estado;
                _context.Facturas.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado.Equals("ANULADA"))
            {
                obj.EstadoFactura = estado;
                _context.Facturas.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            if (estado.Equals("PENDIENTE"))
            {
                obj.EstadoFactura = estado;
                _context.Facturas.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }

            if (estado.Equals("RECHAZADA"))
            {
                obj.EstadoFactura = estado;
                _context.Facturas.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }

        [HttpPost("enviarFacturaImpuestosCompraVenta")]
        public async Task<respuestaRecepcion> enviarFacturaImpuestosCompraVenta(int codigoPuntoVenta, int codigoFactura, int codigoDocumentoSector, int codigoSucursal)
        {
            FacturacionCompraVentaController controller = new FacturacionCompraVentaController();

            try
            {
                var resp = await controller.getRecepcionFactura(new solicitudRecepcionFactura()
                {
                    archivo = "No requerido",
                    codigoAmbiente = 2,
                    codigoDocumentoSector = codigoDocumentoSector,
                    codigoEmision = 1,
                    codigoModalidad = 1,
                    codigoPuntoVenta = codigoPuntoVenta,
                    codigoPuntoVentaSpecified = true,
                    codigoSistema = "No requerido",
                    codigoSucursal = codigoSucursal,
                    cufd = "No requerido",
                    cuis = "No requerido",
                    fechaEnvio = "No requerido",
                    hashArchivo = "No requerido",
                    nit = 1024061022,// No requerido
                    tipoFacturaDocumento = 1
                },codigoFactura ,codigoPuntoVenta, 1);   

                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return new respuestaRecepcion(){
                    codigoEstado = 52,
                    codigoDescripcion = "RECHAZADA"
                };
                
            }
        }

        [HttpPost("enviarFacturaImpuestosTelecom")]
        public async Task<ServiceTelecom.respuestaRecepcion> enviarFacturaImpuestosTelecom(int codigoPuntoVenta, int codigoFactura, int codigoDocumentoSector, int codigoSucursal)
        {
            FacturacionTelecomController controller = new FacturacionTelecomController();

            try
            {
                var resp = await controller.getRecepcionFactura(new ServiceTelecom.solicitudRecepcionFactura()
                {
                    archivo = "No requerido",
                    codigoAmbiente = 2,
                    codigoDocumentoSector = codigoDocumentoSector,
                    codigoEmision = 1,
                    codigoModalidad = 1,
                    codigoPuntoVenta = codigoPuntoVenta,
                    codigoPuntoVentaSpecified = true,
                    codigoSistema = "No requerido",
                    codigoSucursal = codigoSucursal,
                    cufd = "No requerido",
                    cuis = "No requerido",
                    fechaEnvio = "No requerido",
                    hashArchivo = "No requerido",
                    nit = 1024061022,// No requerido
                    tipoFacturaDocumento = 1
                },codigoFactura ,codigoPuntoVenta, 1);   

                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return new ServiceTelecom.respuestaRecepcion(){
                    codigoEstado = 52,
                    codigoDescripcion = "RECHAZADA"
                };
            }
        }


        [HttpPost("enviarArchivos")]
        public async Task<bool> enviarArchivos(int codigoFactura, string correoReceptor )
        {

            Factura obj = await _context.Facturas.Include(z => z.CodigotelefonoclienteNavigation).Include(z => z.Facturadetalles).FirstOrDefaultAsync(x=>x.Codigofactura.Equals(codigoFactura));
            if(obj == null)
            {
                return false;
            }
            else
            {
                string enlacePDF = "https://pilotosiat.impuestos.gob.bo/consulta/QR?nit=1024061022&cuf="+obj.Cuf+"&numero="+obj.Numerofactura;
                string pathArchivoXML = "Impuestos/XML/FacturasCorreo/"+"FacturaElectronicaCosett-" +obj.Cuf + ".xml";

                // Reconstuyendo la factura 
                if(obj.Codigodocumentosector == 22)
                {
                    FacturaTelecomunicaciones factura = new FacturaTelecomunicaciones();

                    factura.nitEmisor = (long)obj.Nitemisor;
                    factura.razonSocialEmisor = "COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R.";
                    factura.municipio = "Tarija";
                    factura.telefono = "2846005";
                    factura.nitConjunto = null;
                    factura.numeroFactura = obj.Numerofactura;
                    factura.cuf = obj.Cuf;
                    factura.cufd = obj.Cufd;
                    factura.codigoSucursal = obj.Codigosucursal;
                    factura.direccion = obj.Direccion;
                    factura.codigoPuntoVenta = obj.Codigopuntoventa;
                    factura.fechaEmision = obj.Fechaemision.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    factura.nombreRazonSocial = obj.Nombrerazonsocial;
                    factura.codigoTipoDocumentoIdentidad = obj.Codigotipodocumentoidentidad;
                    factura.numeroDocumento = obj.Numerodocumento;

                    if(obj.Complemento != "")
                    {
                        factura.complemento = obj.Complemento;    
                    }
                    else
                    {
                        factura.complemento = null;
                    }
                    
                    factura.codigoCliente = obj.Codigotelefonocliente + "";
                    factura.codigoMetodoPago = obj.Codigometodopago;
                    factura.montoTotal = (float)obj.Montototal;
                    factura.montoTotalSujetoIva = (float)obj.Montototalsujetoiva;
                    factura.codigoMoneda = obj.Codigomoneda;
                    factura.tipoCambio = 1;
                    factura.montoTotalMoneda = (int)obj.Montototal;
                    factura.leyenda = obj.Leyenda;
                    factura.usuario = obj.Usuario;
                    factura.codigoDocumentoSector = obj.Codigodocumentosector;

                    // Cargado de Detalle facturas
                    List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
                    foreach(Facturadetalle detalle in obj.Facturadetalles)
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

                    await Helper.facturar(factura, 1, 0, 2, pathArchivoXML);
                }
                else if(obj.Codigodocumentosector == 1)
                {
                    FacturaCompraVenta factura = new FacturaCompraVenta();

                    factura.nitEmisor = (long)obj.Nitemisor;
                    factura.razonSocialEmisor = "COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R.";
                    factura.municipio = "Tarija";
                    factura.telefono = "2846005";
                    factura.numeroFactura = obj.Numerofactura;
                    factura.cuf = obj.Cuf;
                    factura.cufd = obj.Cufd;
                    factura.codigoSucursal = obj.Codigosucursal;
                    factura.direccion = obj.Direccion;
                    factura.codigoPuntoVenta = obj.Codigopuntoventa;
                    factura.fechaEmision = obj.Fechaemision.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    factura.nombreRazonSocial = obj.Nombrerazonsocial;
                    factura.codigoTipoDocumentoIdentidad = obj.Codigotipodocumentoidentidad;
                    factura.numeroDocumento = obj.Numerodocumento;

                    if(obj.Complemento != "")
                    {
                        factura.complemento = obj.Complemento;    
                    }
                    else
                    {
                        factura.complemento = null;
                    }
                    
                    factura.codigoCliente = obj.CodigotelefonoclienteNavigation.Telefono + "";
                    factura.codigoMetodoPago = obj.Codigometodopago;
                    factura.montoTotal = (float)obj.Montototal;
                    factura.montoTotalSujetoIva = (float)obj.Montototalsujetoiva;
                    factura.codigoMoneda = obj.Codigomoneda;
                    factura.tipoCambio = 1;
                    factura.montoTotalMoneda = (float)obj.Montototal;
                    factura.leyenda = obj.Leyenda;
                    factura.usuario = obj.Usuario;
                    factura.codigoDocumentoSector = obj.Codigodocumentosector;

                    // Cargado de detalles de factura
                    List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
                    foreach(Facturadetalle detalle in obj.Facturadetalles)
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

                    await Helper.facturar(factura, 2, 0, 2, pathArchivoXML);
                }

                //creamos nuestra lista de archivos a enviar
                List<string> lstArchivos = new List<string>();
                lstArchivos.Add(pathArchivoXML);
                
                // Creando el mensaje a enviar
                string mensaje = 
                "<html><head></head>"+
                "<body><div style='margin: 0 auto; width:575px; text-align :center;font-size: 19px'>"+
                "<label style='font-size: 24px; font-weight: bold;'>Facturación Electrónica en Línea</label><br>"+
                "<label style='font-weight: bold;'>Estimado Usuario:</label>"+
                "<p>Le informamos que su factura electrónica se encuentra disponible en formato pdf en el siguiente enlace:</p>"
                + enlacePDF +
                "<p>Si usted desea verificar su factura en formato XML puede encontrarla adjunta en el presente correo.</p>"+
                "<p style ='font-size: 17px'><label style='font-weight: bold;'>Nota:</label> Este e-mail es generado de manera automática, por favor no responda a este mensaje. La información obtenida por medio de nuestro canal digital por Internet, debe estar bajo seguridad personal del cliente, proteja su información personal, no permita que se exponga a terceros.<p>"+
                "<label style='font-weight: bold; font-size: 18px'>Cooperativa de Servicios Públicos de Telecomunicaciones de Tarija R.L. COSETT R. C.</label> <br>"+
                "<label style='font-weight: bold;'font-size: 18px>Tel: 46643010 - NIT: 1024061022 - Tarija, Bolivia</label>"
                +"</div></body></html>";
                
                //creamos nuestro objeto de la clase que hicimos
                EnviadorCorreos enviador = new EnviadorCorreos("correopruebascosett@gmail.com",correoReceptor, mensaje, "Factura Digital de Servicios COSETT R.L.", lstArchivos);
                
                //y enviamos
                Boolean pudoEnviar = enviador.enviaMail();

                // Eliminamos el archivo generado
                Helper.eliminarArchivo(pathArchivoXML);
                
                return pudoEnviar;
            }
            
            
        }


    }
}
