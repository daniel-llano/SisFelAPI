using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisFelApi.Impuestos.Controllers;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.DTOs;
using SisFelApi.Negocio.Models;

namespace SisFelApi.Negocio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaqueteRecepcionCompraController : ControllerBase
    {
        public readonly SisfelbdContext _context;
        private readonly IMapper _mapper;
        public PaqueteRecepcionCompraController(SisfelbdContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("lista")]
        public async Task<ActionResult<List<Paqueterecepcioncompra>>> listaPaquetesRecepcionCompra()
        {
            var lista = await _context.Paqueterecepcioncompras.Include(z => z.Registrocompras).ToListAsync();
            //return Ok(await _context.Paqueterecepcioncompras.ToListAsync());
            return Ok(lista);

            
        }
        [HttpGet("id")]
        public async Task<ActionResult<Paqueterecepcioncompra>> paqueteRecepcionCompra(int Codigopaqueterecepcioncompra)
        {
            Paqueterecepcioncompra obj = await _context.Paqueterecepcioncompras.FindAsync(Codigopaqueterecepcioncompra);
            var objDto = _mapper.Map<PaqueterecepcioncompraDto>(obj);
            if (objDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(objDto);
            }
        }
        [HttpPost("agregar")]
        public async Task<ActionResult> agregarPaqueteRecepcionCompra(PaqueterecepcioncompraDto objDto)
        {
            objDto.Fechaenvio = DateTime.Now;
            objDto.Gestion = (short?)objDto.Fechaenvio.Value.Year;
            objDto.Periodo = (short?)objDto.Fechaenvio.Value.Month;

            var obj = _mapper.Map<Paqueterecepcioncompra>(objDto);
            _context.Paqueterecepcioncompras.Add(obj);
            await _context.SaveChangesAsync();
            return Created($"/id?codigoCompra={obj.Codigopaqueterecepcioncompra}", obj);
        }
        [HttpPut("modificar")]
        public async Task<ActionResult> actualizarPaqueteRecepcionCompra(PaqueterecepcioncompraDto objDto)
        {
            var obj = _mapper.Map<Paqueterecepcioncompra>(objDto);
            _context.Paqueterecepcioncompras.Update(obj);
            await _context.SaveChangesAsync();
            return Created($"/id?codigoCompra={obj.Codigopaqueterecepcioncompra}", obj);
        }
        [HttpPut("cambio de estado")]
        public async Task<ActionResult> actualizarEstadoPaqueteRecepcionCompra(int Codigopaqueterecepcioncompra, int estado)
        {
            Paqueterecepcioncompra obj = await _context.Paqueterecepcioncompras.FindAsync(Codigopaqueterecepcioncompra);

            if (obj == null)
            {
                return NotFound();
            }
            else if(estado == obj.Estado){
                return Ok();
            }
            else
            {
                obj.Estado = (short)estado;
                _context.Paqueterecepcioncompras.Update(obj);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPost("enviarPaqueteCompras")]
        public async Task<List<ServiceCompras.respuestaRecepcion>> enviarPaqueteCompras(int codigoPaqueteRecepcionCompra, int codigoPuntoVenta, int codigoSucursal)
        {

            int numeroDeFacturasPorPaquete = 500;

            // Recuperando objeto
            Paqueterecepcioncompra paquete = await _context.Paqueterecepcioncompras.Include(z => z.Registrocompras).FirstOrDefaultAsync(x=> x.Codigopaqueterecepcioncompra.Equals(codigoPaqueteRecepcionCompra));

            // Teniendo en cuenta que las facturas se envian de 500 en 500 primero obtendremos el numero de envios que realizaremos para esto
            // Obteniendo el numero de registro compras de la lista de paquete recepcion compra
            int numeroRegistrosCompras = paquete.Registrocompras.ToList().Count();
            
            // Obtenemos el numero de envios a realizar
            int numeroEnvios = numeroRegistrosCompras / numeroDeFacturasPorPaquete;

            // El resto de facturas que se enviar se encontrara en el resto del calculo
            int numeroRestanteDeRegistros = numeroRegistrosCompras % numeroDeFacturasPorPaquete;

            // Teniendo estos dos datos primero realizaremos el numero de envios que tenenemos enviando paquetes de 500 en 500 y luego realizaremos el envio 
            // con el numero restante de registros en un ultimo paquete, por lo tanto

            
            // Creando variables base
            string pathCarpetaRegistroCompras = "../SisFelApi/Impuestos/XML/FacturacionCompras/RegistrosCompras/";
            string pathPaqueteRegistroCompras = "../SisFelApi/Impuestos/XML/FacturacionCompras/paqueteCompras.tar.gz";
            List<ServiceCompras.respuestaRecepcion> listaRespuestasRecepcion = new List<ServiceCompras.respuestaRecepcion>();
            RecepcionComprasController recepcionComprasController = new RecepcionComprasController();
            
            // Realizando envios de paquetes de 500
            int indiceRegistroInicialPaquete = 0;
            int indiceRegistroFinalPaquete = numeroDeFacturasPorPaquete - 1;
            
            for(int i=0; i<numeroEnvios; i++){
                
                // Obtenemos las 500 facturas para el paquete
                for(int indice= indiceRegistroInicialPaquete; indice<indiceRegistroFinalPaquete; indice++){
                    
                    // Recuperamos el registro
                    Registrocompra registroBD = paquete.Registrocompras.ToList()[indice];

                    // Pasamos los datos de nuestro modelo de Registrocompra de la BD al modelo de RegistroCompra del xml
                    RegistroCompra registroXML = new RegistroCompra(){
                        codigoAutorizacion = registroBD.Codigoautorizacion,
                        codigoControl = registroBD.Codigocontrol,
                        creditoFiscal = (float)registroBD.Creditofiscal,
                        descuento = (float)registroBD.Descuento,
                        fechaEmision = (DateTime)registroBD.Fechaemision,
                        importeIce = (float)registroBD.Importeice,
                        importeIehd = (float)registroBD.Importeiehd,
                        importeIpj = (float)registroBD.Importeipj,
                        importesExentos = (float)registroBD.Importesexentos,
                        importeTasaCero = (float)registroBD.Importetasacero,
                        montoGiftCard = (float)registroBD.Montogiftcard,
                        montoTotalCompra = (float)registroBD.Montototalcompra,
                        montoTotalSujetoIva = (float)registroBD.Montototalsujetoiva,
                        nitEmisor = (long)registroBD.Nitemisor,
                        nro = registroBD.Nrocompra,
                        numeroDuiDim = registroBD.Numeroduidim,
                        numeroFactura = long.Parse(registroBD.Numerofactura),
                        otroNoSujetoCredito = (float)registroBD.Otronosujetocredito,
                        razonSocialEmisor = registroBD.Razonsocialemisor,
                        subTotal = (float)registroBD.Subtotal,
                        tasas = (float)registroBD.Tasas,
                        tipoCompra = byte.Parse(registroBD.Tipocompra)
                    };

                    // Serializamos el objeto en un archivo xml
                    Serializacion serializador = new Serializacion();
                    serializador.GenerarRegistroCompraXml(registroXML, pathCarpetaRegistroCompras + "RegistroCompra" + indice + ".xml");
                }

                // Una vez generados los 500 registros .xml

                // Los empaquetamos
                Helper.empaquetarFacturas(pathCarpetaRegistroCompras, pathPaqueteRegistroCompras);

                // Lo enviamos a end point de impuestos para esto
                try{
                    // Consumiendo metodo de recepcion
                    ServiceCompras.respuestaRecepcion respuesta = await recepcionComprasController.getRecepcionPaqueteCompras( new ServiceCompras.solicitudRecepcionCompras(){
                        archivo = "No requerido",
                        cantidadFacturas = 500,
                        codigoAmbiente = 2,
                        codigoPuntoVenta = 0,// No requerido
                        codigoSistema = "No requerido",
                        codigoSucursal = codigoSucursal,
                        cufd = "No requerido",
                        cuis = "No requerido",
                        fechaEnvio = (DateTime)paquete.Fechaenvio,// No requerido,
                        gestion = (int)paquete.Gestion,
                        hashArchivo = "No requerido",
                        nit = 0, // No requerido,
                        periodo = (int)paquete.Periodo                    
                    }, codigoPuntoVenta, pathPaqueteRegistroCompras);

                    listaRespuestasRecepcion.Add(respuesta);
                    
                }catch(Exception e){
                    Console.WriteLine($"{e.Message}");

                    // Creamos la lista de mensajes 
                    ServiceCompras.mensajeRecepcion[] xmensajesList = new ServiceCompras.mensajeRecepcion[1];
                    
                    // Creamos el mensaje con el error
                    ServiceCompras.mensajeRecepcion mensaje = new ServiceCompras.mensajeRecepcion(){
                        descripcion = e.Message
                    };
                    xmensajesList[0] = mensaje;

                    // Devolviendo error
                    new ServiceCompras.respuestaRecepcion(){
                        codigoDescripcion = "RECHAZADA",
                        mensajesList = xmensajesList,
                        codigoRecepcion = "-1" // Colocando un codigo que sera identificado en el fron-end
                    };
                }

                // Actualizando los indices para los demas registros
                indiceRegistroInicialPaquete = indiceRegistroFinalPaquete;
                indiceRegistroFinalPaquete += 500;
            }


            // Realizando envio de paquete con registros restantes
            // Actualizando los indices
            if(numeroEnvios>0){
                indiceRegistroInicialPaquete = indiceRegistroFinalPaquete;
                indiceRegistroFinalPaquete += numeroRestanteDeRegistros;
            }else{
                indiceRegistroInicialPaquete = 0;
                indiceRegistroFinalPaquete = indiceRegistroInicialPaquete + numeroRestanteDeRegistros;
            }

            for(int indice= indiceRegistroInicialPaquete; indice<indiceRegistroFinalPaquete; indice++){
                    
                // Recuperamos el registro
                Registrocompra registroBD = paquete.Registrocompras.ToList()[indice];

                // Pasamos los datos de nuestro modelo de Registrocompra de la BD al modelo de RegistroCompra del xml
                RegistroCompra registroXML = new RegistroCompra(){
                    codigoAutorizacion = registroBD.Codigoautorizacion,
                    codigoControl = registroBD.Codigocontrol,
                    creditoFiscal = (float)registroBD.Creditofiscal,
                    descuento = (float)registroBD.Descuento,
                    fechaEmision = (DateTime)registroBD.Fechaemision,
                    importeIce = (float)registroBD.Importeice,
                    importeIehd = (float)registroBD.Importeiehd,
                    importeIpj = (float)registroBD.Importeipj,
                    importesExentos = (float)registroBD.Importesexentos,
                    importeTasaCero = (float)registroBD.Importetasacero,
                    montoGiftCard = (float)registroBD.Montogiftcard,
                    montoTotalCompra = (float)registroBD.Montototalcompra,
                    montoTotalSujetoIva = (float)registroBD.Montototalsujetoiva,
                    nitEmisor = (long)registroBD.Nitemisor,
                    nro = registroBD.Nrocompra,
                    numeroDuiDim = registroBD.Numeroduidim,
                    numeroFactura = long.Parse(registroBD.Numerofactura),
                    otroNoSujetoCredito = (float)registroBD.Otronosujetocredito,
                    razonSocialEmisor = registroBD.Razonsocialemisor,
                    subTotal = (float)registroBD.Subtotal,
                    tasas = (float)registroBD.Tasas,
                    tipoCompra = int.Parse(registroBD.Tipocompra)
                };

                // Serializamos el objeto en un archivo xml
                Serializacion serializador = new Serializacion();
                serializador.GenerarRegistroCompraXml(registroXML, pathCarpetaRegistroCompras + "RegistroCompra" + indice + ".xml");
            }

            // Una vez generados los 500 registros .xml

            // Los empaquetamos
            Helper.empaquetarFacturas(pathCarpetaRegistroCompras, pathPaqueteRegistroCompras);

            // Lo enviamos a end point de impuestos para esto
            try{
                // Consumiendo metodo de recepcion
                ServiceCompras.respuestaRecepcion respuesta = await recepcionComprasController.getRecepcionPaqueteCompras( new ServiceCompras.solicitudRecepcionCompras(){
                    archivo = "No requerido",
                    cantidadFacturas = 4,
                    codigoAmbiente = 2,
                    codigoPuntoVenta = 0,// No requerido
                    codigoSistema = "No requerido",
                    codigoSucursal = codigoSucursal,
                    cufd = "No requerido",
                    cuis = "No requerido",
                    fechaEnvio = (DateTime)paquete.Fechaenvio,// No requerido,
                    gestion = (int)paquete.Gestion,
                    hashArchivo = "No requerido",
                    nit = 0, // No requerido,
                    periodo = (int)paquete.Periodo                    
                }, codigoPuntoVenta, pathPaqueteRegistroCompras);

                listaRespuestasRecepcion.Add(respuesta);
                
            }catch(Exception e){
                Console.WriteLine($"{e.Message}");

                // Creamos la lista de mensajes 
                ServiceCompras.mensajeRecepcion[] xmensajesList = new ServiceCompras.mensajeRecepcion[1];
                
                // Creamos el mensaje con el error
                ServiceCompras.mensajeRecepcion mensaje = new ServiceCompras.mensajeRecepcion(){
                    descripcion = e.Message
                };
                xmensajesList[0] = mensaje;

                // Devolviendo error
                new ServiceCompras.respuestaRecepcion(){
                    codigoDescripcion = "RECHAZADA",
                    mensajesList = xmensajesList,
                    codigoRecepcion = "-1" // Colocando un codigo que sera identificado en el fron-end
                };
            }
            return listaRespuestasRecepcion;
        }
    }
}
