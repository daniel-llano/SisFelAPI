using AutoMapper;
using Correos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Impuestos.ServiceCommon.Armado_de_Objetos;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturacionMasivaController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
       
        public FacturacionMasivaController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //trae la lista de FacturaMasivas en gral.
        [HttpGet("lista")]
        public async Task<ActionResult<List<Facturacionmasiva>>> listaFacturaMasivas(int? estado)
        {
            if (estado != null)
            {
                return Ok(await _context.Facturacionmasivas.Where(x => x.Estado == estado).ToListAsync());
            }
            if (estado == null)
            {
                return Ok(await _context.Facturacionmasivas.ToListAsync());
            }
            return Ok();
        }
        //trae una FacturaMasiva en específica mandando el codigo de la misma
        [HttpGet("id")]
        public async Task<FacturacionMasivaDto> FacturaMasiva(int codigofacturacionmasiva)
        {
            Facturacionmasiva obj = await _context.Facturacionmasivas.FindAsync(codigofacturacionmasiva);
            var objDto = _mapper.Map<FacturacionMasivaDto>(obj);
            if (objDto == null)
            {
                return objDto;
            }
            else
            {
                return objDto;
            }
        }
        //agrega una nueva factura masiva mandando el objeto correspondiente de tipo factura masiva Dto
        [HttpPost("agregar")]
        public async Task<FacturacionMasivaDto> agregarFacturaMasiva (FacturacionMasivaDto objDto)
        {
            var obj = _mapper.Map<Facturacionmasiva>(objDto);
            _context.Facturacionmasivas.Add(obj);
            await _context.SaveChangesAsync();
            objDto = _mapper.Map<FacturacionMasivaDto>(obj);
            return objDto;
        }
        //modifica una nueva factura masiva mandando el objeto correspondiente de tipo factura masiva Dto
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarFacturaMasiva(FacturacionMasivaDto objDto)
        {
            Facturacionmasiva obj = await _context.Facturacionmasivas.FindAsync(objDto.Codigofacturacionmasiva);
            
            obj.Codigorecepcion = objDto.Codigorecepcion;
            obj.Fechafin = objDto.Fechafin;
            obj.Estado = objDto.Estado;

            _context.Facturacionmasivas.Update(obj);
            await _context.SaveChangesAsync();
            return Ok(new {  sms="actualizacion exitosa" });
        }
        //realiza el cambio de estado segun mandando el codigo de la misma y el tipo de estado a cambiar
        [HttpPut("cambio de estado")]
        public async Task<ActionResult> estadoFacturaMasiva(int codigofacturacionmasiva, int estado)
        {
            Facturacionmasiva obj = await _context.Facturacionmasivas.FindAsync(codigofacturacionmasiva);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                obj.Estado = estado;
                _context.Facturacionmasivas.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
        //agrega los txt de cabecera y detalle----observado
        [HttpPost("agregarTxt")]
        public async Task<ActionResult> agregarTxt(IFormFile file)
        {
            var path = string.Empty;
            var path2 = string.Empty;
            var nombreTxt = file.FileName;
            path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "MyStaticFiles/txt",
                    nombreTxt);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            path2 = $"StaticFiles/txt/{nombreTxt}";
            return Ok(new {nombre=nombreTxt});
        }
        //valida, llena y devuelve una lista de facturas telecomunicaciones
        [HttpGet("valida")]
        public async Task<ActionResult> ValidacionTxt(string nombreCab, string nombreDet, int sucursal, int puntoVenta, string usuario)
        {
            string mensaje = "Iniciando envio de archivos masivos a fecha: " + DateTime.Now.ToString();
            //creamos nuestro objeto de la clase que hicimos
            EnviadorCorreos enviador = new EnviadorCorreos("correopruebascosett@gmail.com","franciscojavierlopezperez85@gmail.com", mensaje, "Facturacion Masiva Pruebas");
            //y enviamos
            enviador.enviaMail();
                
            List<ServiceTelecom.respuestaRecepcion> respuestasImpuestos = new List<ServiceTelecom.respuestaRecepcion>();
            MetodosLecturacion metodosLecturacion = new MetodosLecturacion(_mapper);
            var pathCabecera = Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles/txt", nombreCab);
            var pathDetalle = Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles/txt", nombreDet);
            var objMasivo = metodosLecturacion.LecturaTxt(pathCabecera, pathDetalle);
            //objeto llenado de los txt y respuestas de las validaciones
            var obj = objMasivo.facturaTxt;
            var erroresDetalle = objMasivo.erroresDetalle;
            var erroresCabecera = objMasivo.erroresCabecera;
            if (objMasivo.erroresDetalle.Count > 0)
            {
                return Ok(new { tipo = 1, obj = obj, erroresDetalle = erroresDetalle });
            }
            if (objMasivo.erroresCabecera.Count > 0)
            {
                return Ok(new { tipo = 2, obj = obj, erroresCabecera = erroresCabecera });
            }

            //despues de la validacio se obtiene el cufd
            await Helper.actualizarCodigos(0);
            //insetando en la tabla facturas y detalles
            FacturaDto factura;
            FacturaDetalleDto detalle;
            FacturaController facturaController = new FacturaController(_context, _mapper);//controladores para la insercion de datos
            FacturaDetalleController facturaDetalleController = new FacturaDetalleController(_context, _mapper);//controladores para la insercion de datos
            List<FacturaCompraVenta> facturaCompraVentas = new List<FacturaCompraVenta>();

            mensaje = "Termino lecturacion de objetos masivos, empezando a cargar facturas en la bd a fecha: " + DateTime.Now.ToString();
            //creamos nuestro objeto de la clase que hicimos
            EnviadorCorreos enviador1 = new EnviadorCorreos("correopruebascosett@gmail.com","franciscojavierlopezperez85@gmail.com", mensaje, "Facturacion Masiva Pruebas");
            enviador1.enviaMail();

            foreach (var facturas in obj)
            {
                //generar fecha actual para los cufd
                var fechaactual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                //"yyyy-MM-ddTHH:mm:ss.fff"
                var hora = fechaactual.Substring(10, 13);
                
                decimal MontototalCalculado = 0;

                factura = new FacturaDto();
                string fecha = facturas.fechaEmision.Substring(0, 9) + hora;
                factura.Nitemisor = facturas.nitEmisor;
                factura.Municipio = "Tarija";
                factura.Telefonoemisor = int.Parse("2846005");
                factura.Nitconjunto = null;
                factura.Numerofactura = (int)facturas.numeroFactura;
                factura.Cuf = Helper.obtenerCuf(1, fechaactual, (int)facturas.numeroFactura, 0, Helper.general.Codigocontrol, 3);//observado la fecha actual
                factura.Cufd = Helper.general.Cufd;
                facturas.cuf = factura.Cuf;
                facturas.cufd = factura.Cufd;
                factura.Codigosucursal = sucursal;//capturadas desde los parametros del controlador
                facturas.codigoSucursal = sucursal;//asingacion de la sucursal para el obj
                factura.Direccion = "AV.JORGE LOPEZ #123";
                factura.Codigopuntoventa = puntoVenta;//capturadas desde los parametros del controlador
                facturas.codigoPuntoVenta = puntoVenta;//asingacion del punto de venta para el obj
                factura.Fechaemision = DateTime.Parse(facturas.fechaEmision);
                facturas.fechaEmision = fechaactual;
                factura.Nombrerazonsocial = facturas.nombreRazonSocial;
                factura.Codigotipodocumentoidentidad = facturas.codigoTipoDocumentoIdentidad;
                factura.Numerodocumento = facturas.numeroDocumento;
                factura.Complemento = null;
                factura.Codigotelefonocliente = "1";//observado buscar como recuperar
                factura.Codigometodopago = 1;
                factura.Montototal = (decimal)facturas.montoTotal;
                factura.Montototalsujetoiva = (decimal)facturas.montoTotalSujetoIva;
                factura.Leyenda = facturas.leyenda;
                factura.Usuario = usuario;//capturadas desde los parametros del controlador
                facturas.usuario = usuario;//asingacion del usuario para el obj
                factura.Codigodocumentosector = 22;
                factura.Cafc = "";
                factura.Codigorecepcion = "0";
                factura.Nrotarjeta = null;
                factura.Codigomoneda = facturas.codigoMoneda;
                factura.EstadoFactura = "PENDIENTE";
                factura.Descuentoadicional = 0;
                
                int codigoFactura = ((Factura)(await facturaController.agregarFactura(factura))).Codigofactura;
                factura.Codigofactura = codigoFactura;

                foreach (var detalles in facturas.detalles)
                {
                    
                    detalle = new FacturaDetalleDto();
                    detalle.Codigofactura = codigoFactura;
                    detalle.Codigoproducto = "1";//detalles.codigoProducto;//observado
                    detalle.Actividadeconomica = "610000";//observada
                    detalle.Codigoproductosin = 84120;
                    detalle.Descripcion = detalles.descripcion;
                    detalle.Cantidad = detalles.cantidad;
                    detalle.Unidadmedida = detalles.unidadMedida;
                    detalle.Preciounitario =  Decimal.Round((decimal)detalles.precioUnitario, 2);
                    detalle.Montodescuento = 0;
                    //detalle.Subtotal = (decimal)detalles.subTotal; Este es el subtotal que envian en el txt el cuaL es invalido
                    detalle.Cuenta = detalles.Cuenta;
                    detalle.Numeroserie = null;
                    detalle.Numeroimei = null;
                    detalle.Codigogrupo = null;

                    // Calculando el subtotal del detalle
                    detalle.Subtotal = Decimal.Round(detalle.Preciounitario * detalle.Cantidad, 2);
            
                    // Sumando el subtotal del detalles al de los demas detalles para el calculo del munto total interno 
                    MontototalCalculado =  Decimal.Round(MontototalCalculado + detalle.Subtotal, 2);

                    // Actualizando en el objeto lecturado el cual sera enviado para generar el xml
                    detalles.subTotal = (float)detalle.Subtotal;

                    await facturaDetalleController.agregarFacturaDetalle(detalle);
                }

                // Actualizando el montoTotalDeLaFactura calculado sumando los subtotales de los detalles 
                factura.Montototal = Decimal.Round(MontototalCalculado, 2);
                factura.Montototalsujetoiva = factura.Montototal;

                // Actualizando en el objeto lecturado el cual sera enviado para generar el xml
                facturas.montoTotal = (float)factura.Montototal;
                facturas.montoTotalSujetoIva = (float)factura.Montototal;
                facturas.montoTotalMoneda = (float)factura.Montototal;

                await facturaController.actualizarFactura(factura);
            }


            var contador = 0;
            var insertar = true;
            List<Impuestos.ServiceCommon.Armado_de_Objetos.modelos.FacturaTxt> listaClon = new List<Impuestos.ServiceCommon.Armado_de_Objetos.modelos.FacturaTxt>();
            List<Impuestos.ServiceCommon.Armado_de_Objetos.modelos.FacturaTxt> listaClonAgregadas = new List<Impuestos.ServiceCommon.Armado_de_Objetos.modelos.FacturaTxt>();
            List<Impuestos.ServiceCommon.Armado_de_Objetos.modelos.FacturaTxt> listaSobrantes = new List<Impuestos.ServiceCommon.Armado_de_Objetos.modelos.FacturaTxt>();
            //List<FacturacionMasivaDto> listaUpdate = new List<FacturacionMasivaDto>();
            //insertar tabla de facturas masiva de 1000 en 1000: -----------------------------------------------------
            mensaje = "Termino de cargar facturas en la bd, empezando a enviar paquetes de 1000 en 1000 a fecha: " + DateTime.Now.ToString();
            //creamos nuestro objeto de la clase que hicimos
            EnviadorCorreos enviador2 = new EnviadorCorreos("correopruebascosett@gmail.com","franciscojavierlopezperez85@gmail.com", mensaje, "Facturacion Masiva Pruebas");
            enviador2.enviaMail();

            for (int i = 0; i < obj.Count; i++)
            {
                listaClon.Add(obj[i]);
                contador++;

                if (contador == 1000)
                {
                    foreach (var item in listaClon)
                    {
                        listaClonAgregadas.Add(item);
                    }
                    FacturacionMasivaDto masivaDto = new FacturacionMasivaDto();
                    masivaDto.Cufdmasivo = Helper.general.Cufd;
                    masivaDto.Fechainicio = DateTime.Now;
                    masivaDto.Fechafin = DateTime.Now;
                    masivaDto.Numerofacturasenviadas = listaClon.Count;//obj.Count;
                    masivaDto.Numerofacturainicio = (int)listaClon[0].numeroFactura;
                    masivaDto.Numerofacturafin = (int)listaClon[listaClon.Count - 1].numeroFactura;
                    masivaDto.Estado = 3;
                    masivaDto.Codigorecepcion = "";

                    var objUpdate = await agregarFacturaMasiva(masivaDto);//para almacenar el obj guardado de la facturacíon masiva
                    objUpdate = (FacturacionMasivaDto) await FacturaMasiva(objUpdate.Codigofacturacionmasiva);
                    var lisTelecom = metodosLecturacion.mapeo(listaClon);
                    listaClon.Clear();//para eliminar los 1000 datos insertados y poder insertar datos nuevos
                    var resp = (ServiceTelecom.respuestaRecepcion)(await Helper.emisionMasiva(facturaCompraVentas, lisTelecom, 0, 0, lisTelecom.Count, 3, 1));// cod pun= 0, cod emi=3, capa=1

                    //actualizar la tabla facturacion masiva : fecha fin, codigo rececpcion:1
                    //objUpdate.
                    if(resp.codigoDescripcion == "VALIDADA"){
                        objUpdate.Estado = 1;
                    }else if(resp.codigoDescripcion == "OBSERVADA"){
                        objUpdate.Estado = 2;
                    }else if((resp.codigoDescripcion == "RECHAZADA")){
                        objUpdate.Estado = 4;
                    }else{
                        objUpdate.Estado = 4;
                    }

                    if (resp.codigoRecepcion == "0" || resp.codigoRecepcion == null)
                    {
                        objUpdate.Fechafin = DateTime.Now;
                        objUpdate.Codigorecepcion = "0";
                    }
                    
                    if (resp.codigoRecepcion != "0")
                    {
                        objUpdate.Fechafin = DateTime.Now;
                        objUpdate.Codigorecepcion = resp.codigoRecepcion;
                        
                    }
                    
                    await actualizarFacturaMasiva(objUpdate);
                    //listaUpdate.Add(objUpdate);
                    respuestasImpuestos.Add(resp);
                    contador = 0;

                    mensaje = "Termino de enviar el 1 paquete a fecha: " + DateTime.Now.ToString();
                    //creamos nuestro objeto de la clase que hicimos
                    EnviadorCorreos enviador4 = new EnviadorCorreos("correopruebascosett@gmail.com","franciscojavierlopezperez85@gmail.com", mensaje, "Facturacion Masiva Pruebas");
                    enviador4.enviaMail();
                }
            }

            mensaje = "Termino de enviar el todos los paquetes de 1000, empezo a enviar el ultimo paquete a fecha: " + DateTime.Now.ToString();
            //creamos nuestro objeto de la clase que hicimos
            EnviadorCorreos enviador5 = new EnviadorCorreos("correopruebascosett@gmail.com","franciscojavierlopezperez85@gmail.com", mensaje, "Facturacion Masiva Pruebas");
            enviador5.enviaMail();
            //--------------------------------------------------------------

            //insertando las facturas sobrantes que no cumplen el tamaño de 1000 -------------------------------
            listaSobrantes = obj.Except(listaClonAgregadas).ToList();
            FacturacionMasivaDto masivaDtoSobrante = new FacturacionMasivaDto();
            masivaDtoSobrante.Cufdmasivo = Helper.general.Cufd;
            masivaDtoSobrante.Fechainicio = DateTime.Now;
            masivaDtoSobrante.Fechafin = DateTime.Now;
            masivaDtoSobrante.Numerofacturasenviadas = listaSobrantes.Count;//obj.Count;
            masivaDtoSobrante.Numerofacturainicio = (int)listaSobrantes[0].numeroFactura;
            masivaDtoSobrante.Numerofacturafin = (int)listaSobrantes[listaSobrantes.Count - 1].numeroFactura;
            masivaDtoSobrante.Estado = 3;
            masivaDtoSobrante.Codigorecepcion = "";

            var objUpdateSobrante = await agregarFacturaMasiva(masivaDtoSobrante);//para almacenar el obj guardado de la facturacíon masiva
            objUpdateSobrante = (FacturacionMasivaDto)await FacturaMasiva(objUpdateSobrante.Codigofacturacionmasiva);
            var lisTelecomSobrante = metodosLecturacion.mapeo(listaSobrantes);
            var respSobrante = (ServiceTelecom.respuestaRecepcion)(await Helper.emisionMasiva(facturaCompraVentas, lisTelecomSobrante, 0, 0, lisTelecomSobrante.Count, 3, 1));// cod pun= 0, cod emi=3, capa=1
            
            //actualizar la tabla facturacion masiva : fecha fin, codigo rececpcion:1
            //objUpdate.

            if(respSobrante.codigoDescripcion == "VALIDADA"){
                objUpdateSobrante.Estado = 1;
            }else if(respSobrante.codigoDescripcion == "OBSERVADA"){
                objUpdateSobrante.Estado = 2;
            }else{
                objUpdateSobrante.Estado = 4;
            }


            if (respSobrante.codigoRecepcion == "0" || respSobrante.codigoRecepcion == null)
            {
                objUpdateSobrante.Fechafin = DateTime.Now;
                objUpdateSobrante.Codigorecepcion = "0";
            }
            if (respSobrante.codigoRecepcion == null)
            {
                objUpdateSobrante.Fechafin = DateTime.Now;
                objUpdateSobrante.Codigorecepcion = respSobrante.codigoRecepcion;
            }
            if (respSobrante.codigoRecepcion != "0")
            {
                objUpdateSobrante.Fechafin = DateTime.Now;
                objUpdateSobrante.Codigorecepcion = respSobrante.codigoRecepcion;//respSobrante.codigoRecepcion;
            }
            
            await actualizarFacturaMasiva(objUpdateSobrante);
            //listaUpdate.Add(objUpdateSobrante);
            respuestasImpuestos.Add(respSobrante);
            // return Ok(new { tipo = 0, obj = obj, resp=respuestasImpuestos});

            //await actualizarFacturaMasiva(listaUpdate);
            //return Ok(new { tipo = 0, obj = listaUpdate, resp = respuestasImpuestos });

            mensaje = "Termino de enviar el todos los paquetes: " + DateTime.Now.ToString();
            //creamos nuestro objeto de la clase que hicimos
            EnviadorCorreos enviador6 = new EnviadorCorreos("correopruebascosett@gmail.com","franciscojavierlopezperez85@gmail.com", mensaje, "Facturacion Masiva Pruebas");
            enviador6.enviaMail();
            return Ok(new { tipo = 0, resp = respuestasImpuestos });
        }

    }
}