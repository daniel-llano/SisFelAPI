using System.ServiceModel;
using System.Xml;
using System.Security.Cryptography;
using System.Numerics;
using System.Xml.Schema;

// ACCESO A CAPA DE NEGOCIO
using SisFelApi.Negocio.Controllers;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.Models;
using AutoMapper;

// ACCESO A CAPA IMPUESTOS
using ServiceCodigos;
using SisFelApi.Impuestos.Controllers;
using ServiceOperaciones;

// EMPAQUETADO Y COMPRESION
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
using System.IO.Compression;

// CONEXION CON JACOBITUS
using System.Text.Json;
using System.Text;
using ServiceCompraVenta;
using SisFelApi.Impuestos.ServiceCommon.Armado_de_Objetos.modelos;
using SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos;
using Correos;

public static class Helper
{
    // CONFIGURACION
    public static BasicHttpBinding binding { get; set; }
    public static EndpointAddress address { get; set; }
    public static CustomAuthenticationBehaviour behaviour { get; set; }
    public static SisfelbdContext context = new SisfelbdContext();
    public static readonly IMapper _mapper;

    // CONTROLADORES
    public static FacturacionCodigosController generadorCodigos = new FacturacionCodigosController();
    private static GeneralController generalController = new GeneralController(context);
    public static DatosBaseController datosBaseController = new DatosBaseController(context);
    public static EventoController eventoController = new EventoController(context, _mapper);

    // MODELOS
    public static General general = new General();
    public static Datosbase datosbase = new Datosbase();
    public static Evento evento = new Evento();

    // VARIABLES BASE
    public static string pathArchivoXML = "Impuestos/XML/";
    public static string pathArchivoXMLFirmado = "";
    public static string pathArchivoCompreso = "Impuestos/XML/";

    // Paquetes Eventos
    public static string pathCarpetaFacturasTelecom = "Impuestos/XML/FacturacionPaquetes/FacturaTelecomunicacion";
    public static string pathCarpetaFacturasCompraVenta = "Impuestos/XML/FacturacionPaquetes/FacturaCompraVenta";
    public static string pathCarpetaPaquetesEventos = "Impuestos/XML/FacturacionPaquetes/Paquetes";

    // Paquetes Masivos
    public static string pathCarpetaFacturasMasivasTelecom = "Impuestos/XML/FacturacionMasiva/FacturaTelecomunicacion";
    public static string pathCarpetaFacturasMasivasCompraVenta = "Impuestos/XML/FacturacionMasiva/FacturaCompraVenta";
    
    public static string pathCarpetaPaquetesMasivos = "Impuestos/XML/FacturacionMasiva/Paquetes";
    public static string pathArchivoXSD = "Impuestos/XML/ArchivosXSD/facturaElectronica";
    public static string pathArchivoFirmaXSD = "Impuestos/XML/SignatureSchema.xsd";
    public static int numeroLimiteDeFacturasPorPaquete = 500;
    public static int numeroLimiteDeFacturasPorPaqueteMasivo = 1000;





    public static void Configurar(string endpointAddress)
    {
        string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJDb29wZXJhdGl2YTIiLCJjb2RpZ29TaXN0ZW1hIjoiNzIzNzU1RjZCNEFFMjdGNUQ2MUE1N0UiLCJuaXQiOiJINHNJQUFBQUFBQUFBRE0wTURJeE1ETTBNRElDQU0yaERySUtBQUFBIiwiaWQiOjk2OTA2LCJleHAiOjE2OTU2ODY0MDAsImlhdCI6MTY2NDIzMzE4NSwibml0RGVsZWdhZG8iOjEwMjQwNjEwMjIsInN1YnNpc3RlbWEiOiJTRkUifQ._6Ba9qPbx9dKRh3Bii3vFvw8xlPwb9S-4ob__oWHlrFhwvjLXBZ9oDFbKaSHHTDS2mIIlMx3ffZze-qpSsFSNQ";
        //if (binding == null) {
        binding = new BasicHttpBinding
        {
            SendTimeout = TimeSpan.FromSeconds(1000),
            MaxBufferSize = int.MaxValue,
            MaxBufferPoolSize = int.MaxValue,
            MaxReceivedMessageSize = int.MaxValue,
            AllowCookies = true,
            ReaderQuotas = XmlDictionaryReaderQuotas.Max
        };
        binding.Security.Mode = BasicHttpSecurityMode.Transport;
        //}
        //if (address == null)
        address = new EndpointAddress(endpointAddress);
        // if (behaviour == null)
        behaviour = new CustomAuthenticationBehaviour($"TokenApi {token}");
    }




    //METODOS PARA ACTUALIZAR CODIGOS DE ACUERDO AL PUNTO DE VENTA
    public static async Task<int> actualizarCodigos(int codigoPuntoVenta)
    {

        // Cargando Datos Generales en BD
        try
        {
            general = await generalController.getGeneralById(codigoPuntoVenta);

            if (general == null)
            {
                general = new General();
                general = await actualizarCuis(codigoPuntoVenta, 1);
                general = await actualizarCufd(codigoPuntoVenta, 1);
            }


            //Verificando si los datos siguen vigentes
            DateOnly fechaActual = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            if (general.Fechavigenciacuis == null || general.Fechavigenciacuis <= fechaActual)
            {

                try
                {
                    general = await actualizarCuis(codigoPuntoVenta, 2);
                }
                catch (Exception e)
                {
                    //Fallo en la generacion del cuis
                    Console.WriteLine($"{e.Message}");
                    return -2;
                }

            }

            if (general.Fechavigenciacufd == null || general.Fechavigenciacufd <= fechaActual)
            {
                try
                {
                    general = await actualizarCufd(codigoPuntoVenta, 2);
                }
                catch (Exception e)
                {
                    //Fallo en la generacion del cufd
                    Console.WriteLine($"{e.Message}");
                    return -3;
                }
            }

            return 1; // Correcto
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            return -1; //Fallo en la conexion con la BD
        }
    }

    public static async Task<General> actualizarCuis(int codigoPuntoVenta, int accion)
    {
        try
        {
            var respuestaCuis = (respuestaCuis)await generadorCodigos.getCuis(new solicitudCuis()
            {
                codigoAmbiente = 2,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoSucursal = 0,
                codigoModalidad = 1,
                codigoPuntoVentaSpecified = true
            });

            general.Codigopuntoventa = codigoPuntoVenta;
            general.Cuis = respuestaCuis.codigo;
            general.Fechavigenciacuis = new DateOnly(respuestaCuis.fechaVigencia.Year, respuestaCuis.fechaVigencia.Month, respuestaCuis.fechaVigencia.Day);

            try
            {
                if (accion == 1) // Agregar
                {
                    await generalController.agregarGeneral(general);
                }
                else if (accion == 2) // Actualizar
                {
                    await generalController.actualizarGeneral(general);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return null;
            }

            return general;
        }
        catch(Exception e)
        {
            Console.WriteLine($"{e.Message}");
            return null;
        }
    }

    public static async Task<General> actualizarCufd(int codigoPuntoVenta, int accion)
    {
        try
        {
            respuestaCufd respuestaCufd = (respuestaCufd)await generadorCodigos.getCufd(new solicitudCufd()
            {
                cuis = general.Cuis,
                codigoAmbiente = 2,
                codigoPuntoVenta = codigoPuntoVenta,
                codigoSucursal = 0,
                codigoModalidad = 1,
                codigoPuntoVentaSpecified = true
            });

            general.Codigopuntoventa = codigoPuntoVenta;
            general.Cufd = respuestaCufd.codigo;
            general.Codigocontrol = respuestaCufd.codigoControl;
            general.Fechavigenciacufd = new DateOnly(respuestaCufd.fechaVigencia.Year, respuestaCufd.fechaVigencia.Month, respuestaCufd.fechaVigencia.Day);

            try
            {
                await generalController.actualizarGeneral(general);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return null;
            }

            return general;
        }
        catch(Exception e)
        {
            Console.WriteLine($"{e.Message}");
            return null;
        }
    }




    //METODOS PARA LA CONFIGURACION DE LOS PATH DE FACTURAS 
    private static void inicializarPathFactura(int codigoEmision)
    {
        string emision = "";
        string archivoCompreso = "";

        if (codigoEmision == 1)
        {
            emision = "FacturacionIndividual/";
            archivoCompreso = "factura";
        }
        else if (codigoEmision == 2)
        {
            emision = "FacturacionPaquetes/";
            archivoCompreso = "Paquetes/paquete";
        }
        else if (codigoEmision == 3)
        {
            emision = "FacturacionMasiva/";
            archivoCompreso = "Paquetes/paquete";
        }

        pathArchivoXSD = "Impuestos/XML/ArchivosXSD/facturaElectronica";
        pathArchivoXML = "Impuestos/XML/" + emision;
        pathArchivoCompreso = "Impuestos/XML/" + emision + archivoCompreso;
    }

    public static async Task actualizarPathFactura(int tipoFactura, int capa, int codigoEmision)
    {
        if (await actualizarNumerosFacturas())
        {
            inicializarPathFactura(codigoEmision);
            actualizarPathFacturaTipo(tipoFactura);
            actualizarPathFacturaEmision(codigoEmision, tipoFactura);
            actualizarPathFacturaCapa(capa);
        }

    }

    public static async Task<bool> actualizarNumerosFacturas()
    {
        try
        {
            datosbase = await datosBaseController.listaDatosBases();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            return false;
        }

    }

    private static void actualizarPathFacturaTipo(int tipoFactura)
    {
        string tipo = "";

        if (tipoFactura == 1)
        {
            tipo = "Telecomunicacion";
        }
        else if (tipoFactura == 2)
        {
            tipo = "CompraVenta";
        }

        pathArchivoXSD = pathArchivoXSD + tipo + ".xsd";
        pathArchivoXML = pathArchivoXML + "Factura" + tipo + "/facturaElectronica" + tipo;
        pathArchivoCompreso = pathArchivoCompreso + tipo;
    }

    private static void actualizarPathFacturaEmision(int codigoEmision, int tipoFactura)
    {
        if (codigoEmision == 1)
        { // Emision Individual
            pathArchivoXMLFirmado = pathArchivoXML + "Firmado.xml";
            pathArchivoCompreso = pathArchivoCompreso + ".gz";
        }
        else if (codigoEmision == 2)
        { // Emision Paquetes

            if (tipoFactura == 1)
            {
                pathArchivoXMLFirmado = pathArchivoXML + "Firmado" + datosbase.Nrofacturapaquetetl + ".xml";
                pathArchivoCompreso = pathArchivoCompreso + datosbase.Nropaquetetl;
            }
            else if (tipoFactura == 2)
            {
                pathArchivoXMLFirmado = pathArchivoXML + "Firmado" + datosbase.Nrofacturapaquetecv + ".xml";
                pathArchivoCompreso = pathArchivoCompreso + datosbase.Nropaquetecv;
            }

            pathArchivoCompreso = pathArchivoCompreso + ".tar.gz";

        }
        else if (codigoEmision == 3)
        { // Emision Masiva

            if (tipoFactura == 1)
            {
                pathArchivoXMLFirmado = pathArchivoXML + "Firmado" + datosbase.Nrofacturapaquetemasivotl + ".xml";
                pathArchivoCompreso = pathArchivoCompreso + datosbase.Nropaquetemasivotl;
            }
            else if (tipoFactura == 2)
            {
                pathArchivoXMLFirmado = pathArchivoXML + "Firmado" + datosbase.Nrofacturapaquetemasivocv + ".xml";
                pathArchivoCompreso = pathArchivoCompreso + datosbase.Nropaquetemasivocv;
            }

            pathArchivoCompreso = pathArchivoCompreso + ".tar.gz";

        }

        pathArchivoXML = pathArchivoXML + ".xml";
    }

    private static void actualizarPathFacturaCapa(int capa)
    {
        if (capa == 2)
        {
            string pathRelativo = "..//..//..//..//SisFelApi/";
            pathArchivoXML = pathRelativo + pathArchivoXML;
            pathArchivoXMLFirmado = pathRelativo + pathArchivoXMLFirmado;
            pathArchivoCompreso = pathRelativo + pathArchivoCompreso;

            pathCarpetaFacturasCompraVenta = pathRelativo + pathCarpetaFacturasCompraVenta;
            pathCarpetaFacturasTelecom = pathRelativo + pathCarpetaFacturasTelecom;

            pathCarpetaFacturasMasivasCompraVenta = pathRelativo + pathCarpetaFacturasMasivasCompraVenta;
            pathCarpetaFacturasMasivasTelecom = pathRelativo + pathCarpetaFacturasMasivasTelecom;

            pathArchivoXSD = pathRelativo + pathArchivoXSD;
        }
    }




    // METODO PARA FACTURAR DE ACUERDO AL TIPO DE FACTURA Y EL CODIGO DE EMISION
    public static async Task<bool> facturar(object factura, int tipoFactura, int codigoEmision, int accion, string pathArchivoReConstruido)
    {

        string pathArchivoXML  = "";
        string pathArchivoXMLFirmado = "";

        if(accion == 1) // Emitiendo factura 
        {
            pathArchivoXML = Helper.pathArchivoXML;
            pathArchivoXMLFirmado = Helper.pathArchivoXMLFirmado;
        }
        else if(accion == 2) // Reconstruyendo factura
        {
            pathArchivoXML = pathArchivoReConstruido;
            pathArchivoXMLFirmado = pathArchivoReConstruido;
        }

        //Serializando Objeto factura a XML
        Serializacion serializador = new Serializacion();
        if (tipoFactura == 1)
        {
            serializador.GenerarFacturaTelecomunicacionesXml((FacturaTelecomunicaciones)factura, pathArchivoXML);
        }
        else if (tipoFactura == 2)
        {
            serializador.GenerarFacturaCompraVentaXml((FacturaCompraVenta)factura, pathArchivoXML);
        }


        // Firmando XML
        if (!await firmarArchivo(pathArchivoXML, pathArchivoXMLFirmado))
        {
            return false;
        }

        //validarFacturaXML(pathArchivoXMLFirmado,pathArchivoXSD, pathArchivoFirmaXSD, true);

        if (codigoEmision == 1)
        {
            comprimirArchivo(pathArchivoXMLFirmado, pathArchivoCompreso);
        }
        else if (codigoEmision == 2 || codigoEmision == 3)
        {

            Helper.eliminarArchivo(pathArchivoXML);
        }
        else if (codigoEmision == 0) // Reconstruyendo factura
        {
            // No eliminamos ni comprimimos el archivo
        }

        return true;
    }




    // METODOS PARA GENERACION DE CUF
    public static string obtenerCuf(int tipo, string fechaHora, long numeroDeFactura, int codigoPuntoVenta, string codigoControl, int codigoEmision)
    {
        if (tipo == 1)
        {
            // Factura Telecom
            int nitEmisor = 1024061022;
            int codigoSucursal = 0;
            int codigoModalidad = 1;
            int tipoFacturaDocumento = 1;
            int codigoDocumentoSector = 22;

            return obtenerCuf(nitEmisor, fechaHora, codigoSucursal, codigoModalidad, codigoEmision, tipoFacturaDocumento, codigoDocumentoSector,
            numeroDeFactura, codigoPuntoVenta, codigoControl);
        }
        else if (tipo == 2)
        {
            // Factura CompraVenta
            int nitEmisor = 1024061022;
            int codigoSucursal = 0;
            int codigoModalidad = 1;
            int tipoFacturaDocumento = 1;
            int codigoDocumentoSector = 1;

            return obtenerCuf(nitEmisor, fechaHora, codigoSucursal, codigoModalidad, codigoEmision, tipoFacturaDocumento, codigoDocumentoSector,
            numeroDeFactura, codigoPuntoVenta, codigoControl);
        }
        else
        {
            return "";
        }
    }

    private static string obtenerCuf(int nitEmisor, string fechaHora, int codigoSurcursal, int codigoModalidad, int codigoEmision,
     int tipoFacturaDocumento, int codigoDocumentoSector, long numeroDeFactura, int codigoPuntoVenta, string codigoControl)
    {

        string cadenaNitEmisor = ajustarLongitud(nitEmisor, 13);
        string cadenaFechaHora = ajustarFormatoFecha(fechaHora);
        string cadenaCodigoSurcursal = ajustarLongitud(codigoSurcursal, 4);
        string cadenaCodigoModalidad = ajustarLongitud(codigoModalidad, 1);
        string cadenaCodigoEmision = ajustarLongitud(codigoEmision, 1);
        string cadenaTipoFacturaDocumento = ajustarLongitud(tipoFacturaDocumento, 1);
        string cadenaCodigoDocumentoSector = ajustarLongitud(codigoDocumentoSector, 2);
        string cadenaNumeroDeFactura = ajustarLongitud(numeroDeFactura, 10);
        string cadenaCodigoPuntoVenta = ajustarLongitud(codigoPuntoVenta, 4);

        string[] lista = {
            cadenaNitEmisor,
            cadenaFechaHora,
            cadenaCodigoSurcursal,
            cadenaCodigoModalidad,
            cadenaCodigoEmision,
            cadenaTipoFacturaDocumento,
            cadenaCodigoDocumentoSector,
            cadenaNumeroDeFactura,
            cadenaCodigoPuntoVenta
        };

        // Armando cuf
        // Obteniendo cadena de lista de campos
        string cuf = concatenarCampos(lista);

        // Agregando respuestaJsonado de Modulo 11 a la cadena
        cuf = cuf + calcularDigitoMod11(cuf, 1, 9, false);

        // Convirtiendo cuf a base 16
        BigInteger cufBig = BigInteger.Parse(cuf);
        cuf = String.Format("{0:X}", cufBig);

        // Concatenando cuf con codigo de control
        return cuf + codigoControl;
    }
    private static string obtenerCadenaFecha()
    {
        string cadena = DateTime.Now.ToString("yyyyMMddhhmmssfff");
        return cadena;
    }

    private static string ajustarFormatoFecha(string fechaHora)
    {
        //Fecha de entrada 2022-12-12T10:40:00.414
        return fechaHora.Replace("-", "").Replace("T", "").Replace(":", "").Replace(".", "");
    }

    private static string ajustarLongitud(long xdato, int longitudRequerida)
    {
        string dato = xdato.ToString();
        int diferencia = longitudRequerida - dato.Length;
        for (int i = 0; i < diferencia; i++)
        {
            dato = "0" + dato;
        }
        return dato;
    }
    private static string concatenarCampos(string[] ListaCampos)
    {
        string cadena = "";

        for (int i = 0; i < ListaCampos.Length; i++)
        {
            cadena = cadena + ListaCampos[i];
        }

        return cadena;
    }
    private static string calcularDigitoMod11(string cadena, int numDig, int limMult, bool x10)
    {
        int mult, suma, i, n, dig;
        if (!x10) numDig = 1;
        for (n = 1; n <= numDig; n++)
        {
            suma = 0;
            mult = 2;
            for (i = cadena.Length - 1; i >= 0; i--)
            {
                suma += (mult * Int32.Parse(cadena.Substring(i, 1)));
                if (++mult > limMult) mult = 2;
            }
            if (x10)
            {
                dig = ((suma * 10) % 11) % 10;
            }
            else
            {
                dig = suma % 11;
            }
            if (dig == 10)
            {
                cadena += "1";
            }
            if (dig == 11)
            {
                cadena += "0";
            }
            if (dig < 10)
            {
                cadena += dig.ToString();
            }
        }
        return cadena.Substring(cadena.Length - numDig, cadena.Length - (cadena.Length - numDig));
    }


    // METODOS PARA OBTENER DATOS DEL CERTIFICADO
    public static async Task<RespuestaCertificado> obtenerDatosCertificado(){
        return await obtenerDatosCertificadoPrivado();
    }

    private static async Task<RespuestaCertificado> obtenerDatosCertificadoPrivado(){
        // Creando cliente Http
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);

        // Estableciendo direccion base para uso de api Jacobitus
        //string apiJacobitus = "https://localhost:9000/api";
        string apiJacobitus = "https://a455-200-13-152-19.sa.ngrok.io/api";
        
        try
        {
            string verificandoEstadoJacobitus = await client.GetStringAsync(apiJacobitus + "/status");

            // Armando solicitud a enviar
            SolicitudDatosCertificado solicitud = new SolicitudDatosCertificado()
            {
                slot = 1,
                pin = "Z1x2c3v$.,"
            };

            // Serializando solicitud
            string json = serializarSolicitudCertificadoToJson(solicitud);

            // Enviando solicitud
            string respuestaJson = await enviarSolicitudCertificado(client, json);

            if(respuestaJson == null)
            {
                return new RespuestaCertificado(){
                    emisor = "",
                    fechaInicio = "",
                    id = ""
                };
            }
            else
            {
                int indiceId = respuestaJson.IndexOf("id");
            
                string id = respuestaJson.Substring(indiceId + 5, 12);

                int indiceFechaValidez = respuestaJson.IndexOf("validez");

                string fechaInicio = respuestaJson.Substring(indiceFechaValidez + 19, 23);

                int indiceEmisor = respuestaJson.IndexOf("emisor");

                string emisor = respuestaJson.Substring(indiceEmisor+15, 35);

                return new RespuestaCertificado(){
                    emisor = emisor,
                    fechaInicio = fechaInicio,
                    id = id
                };
            }

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return new RespuestaCertificado(){
                    emisor = "",
                    fechaInicio = "",
                    id = ""
                };
        }

    } 

    private static string serializarSolicitudCertificadoToJson(SolicitudDatosCertificado solicitud)
    {
        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(solicitud, serializeOptions);
    }

    private static async Task<string> enviarSolicitudCertificado(HttpClient client, string json)
    {
        // Enviando solicitud
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        //var response = await client.PostAsync("https://localhost:9000/api/token/firmar_xml", requestContent);
        var response = await client.PostAsync("https://a455-200-13-152-19.sa.ngrok.io/api/token/data", requestContent);
        
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }




    // METODOS PARA FIRMAR ARCHIVO
    private static async Task<bool> firmarArchivo(string pathArchivo, string pathArchivoFirmado)
    {
        // Creando cliente Http
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);

        // Estableciendo direccion base para uso de api Jacobitus
        //string apiJacobitus = "https://localhost:9000/api";
        string apiJacobitus = "https://a455-200-13-152-19.sa.ngrok.io/api";
        
        try
        {
            string verificandoEstadoJacobitus = await client.GetStringAsync(apiJacobitus + "/status");

            // Armando solicitud a enviar
            SolicitudFirmaXml solicitud = new SolicitudFirmaXml
            {
                slot = 1,
                pin = "Z1x2c3v$.,",
                alias = "907056811950",
                file = obtenerStringBase64DeArchivo(pathArchivo)
            };

            // Serializando solicitud
            string json = serializarSolicitudToJson(solicitud);

            // Enviando solicitud
            string respuestaJson = await enviarSolicitud(client, json);

            //Deserializando respuesta de json a objeto
            var respuesta = JsonSerializer.Deserialize<RespuestaFirmaXml>(respuestaJson);

            if (respuesta == null || respuesta.datos.xml == null)
            {
                return false;
            }
            else
            {
                RespuestaFirmaXml respuestaObjeto = respuesta;

                // Obteniendo seccion de xml firmado devuelto
                string archivoFirmadoBase64 = respuestaObjeto.datos.xml;

                // Convirtiendo string Base64 a Array de Bytes
                byte[] archivoBytes = convertirBase64StringToArray(archivoFirmadoBase64);

                // Convirtiendo array de bytes en archivo xml
                convertirArrayByteToArchivoXML(archivoBytes, pathArchivoFirmado);
                return true;
            }

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return false;
        }
    }

    private static string serializarSolicitudToJson(SolicitudFirmaXml solicitud)
    {
        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(solicitud, serializeOptions);
    }

    private static async Task<string> enviarSolicitud(HttpClient client, string json)
    {
        // Enviando solicitud
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        //var response = await client.PostAsync("https://localhost:9000/api/token/firmar_xml", requestContent);
        var response = await client.PostAsync("https://a455-200-13-152-19.sa.ngrok.io/api/token/firmar_xml", requestContent);
        
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    private static void convertirArrayByteToArchivoXML(byte[] array, string pathArchivoConvertido)
    {
        XmlDocument archivoXML = new XmlDocument();
        archivoXML.PreserveWhitespace = true;

        // Obteniendo los datos del xml formato string
        String stringXml = obtenerStringXmlDeArrayByte(array);

        // Cargando el string del xml al archivo xml
        archivoXML.LoadXml(stringXml);

        // Guardando el archivo xml fisicamente
        archivoXML.Save(pathArchivoConvertido);
    }

    private static string obtenerStringXmlDeArrayByte(byte[] array)
    {
        using (var ms = new MemoryStream(array))
        using (var reader = new StreamReader(ms))
        {
            return reader.ReadToEnd();
        }
    }

    public static string obtenerStringBase64DeArchivo(string pathArchivo)
    {
        byte[] archivobyte = File.ReadAllBytes(pathArchivo);
        return convertirArrayByteToBase64String(archivobyte);
    }

    private static byte[] obtenerArrayByteDeArchivo(string pathArchivo)
    {
        byte[] archivobyte = File.ReadAllBytes(pathArchivo);
        return archivobyte;
    }

    private static byte[] convertirBase64StringToArray(string cadena)
    {
        byte[] archivobyte = Convert.FromBase64String(cadena);
        return archivobyte;
    }




    // METODOS PARA VALIDAR LOS ARCHIVOS XSD Y XML
    public static void validarFacturaXML(string rutaArchivoXML, string rutaArchivoXSD, string rutaFirmaXSD, bool validarXSD = false)
    {
        if (validarXSD)
        {
            validarArchivoXSD(rutaArchivoXSD);
            validarArchivoXSD(rutaFirmaXSD);
        }
        XmlReaderSettings configXML = new XmlReaderSettings();
        configXML.ValidationType = ValidationType.Schema;
        configXML.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
        configXML.ConformanceLevel = ConformanceLevel.Document;
        configXML.Schemas.Add(null, rutaArchivoXSD);
        configXML.Schemas.Add(null, rutaFirmaXSD);

        configXML.ValidationEventHandler += facturasValidationEventHandler;

        XmlReader lectorXML = XmlReader.Create(rutaArchivoXML, configXML);
        try
        {
            while (lectorXML.Read()) { }
        }
        catch (Exception e)
        {
            throw new Exception("Factura no válida: " + e.Message);
        }
    }
    private static void validarArchivoXSD(string archivoXSD)
    {
        try
        {
            using (FileStream fs = File.OpenRead(archivoXSD))
            {
                XmlSchema schema = XmlSchema.Read(fs, facturasValidationEventHandler);
                fs.Close();
            }
        }
        catch (Exception e)
        {
            throw new Exception("Schema file is invalid: " + e.Message);
        }
    }
    private static void facturasValidationEventHandler(object? sender, ValidationEventArgs e)
    {
        if (e.Severity == XmlSeverityType.Warning)
        {
            Console.Write("WARNING: ");
            Console.WriteLine(e.Message);
        }
        else if (e.Severity == XmlSeverityType.Error)
        {
            Console.Write("ERROR: ");
            Console.WriteLine(e.Message);
        }
    }




    //METODOS PARA COMPRESION Y EMPAQUETADO
    public static void comprimirArchivo(string pathArchivoAComprimir, string pathArchivoCompreso)
    {
        using FileStream originalFileStream = File.Open(pathArchivoAComprimir, FileMode.Open);
        using FileStream compressedFileStream = File.Create(pathArchivoCompreso);
        using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
        originalFileStream.CopyTo(compressor);
    }

    public static void empaquetarFacturas(string pathCarpetaFacturas, string pathPaqueteFactura)
    {
        using (var outStream = File.Create(pathPaqueteFactura))
        using (var gzoStream = new GZipOutputStream(outStream))
        using (var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream))
        {
            foreach (string factura in Directory.GetFiles(pathCarpetaFacturas))
            {
                var tarEntry = TarEntry.CreateEntryFromFile(factura);
                tarEntry.Name = Path.GetFileName(factura);

                tarArchive.WriteEntry(tarEntry, true);
            }
        }
    }





    //METODOS PARA GENERAR HASH
    public static string obtenerHashDeArchivo(string pathArchivo)
    {
        byte[] archivobyte = File.ReadAllBytes(pathArchivo);

        //Obteniendo hash del array de byte del array de bytes del archivo 
        return obtenerHashDeArray(archivobyte);
    }

    private static string obtenerHashDeArray(byte[] archivo)
    {
        using (SHA256 mySHA256 = SHA256.Create())
        {
            byte[] hashValue = mySHA256.ComputeHash(archivo);
            return convertirArrayByteToBase64String(hashValue);
        }
    }

    private static string convertirArrayByteToBase64String(byte[] data)
    {
        string cadena = Convert.ToBase64String(data);
        return cadena;
    }




    //METODO PARA TRABAJAR CON ARCHIVOS
    public static bool eliminarArchivo(string pathArchivo)
    {
        if (!File.Exists(pathArchivo))
        {
            return false;
        }
        else
        {
            File.Delete(pathArchivo);
            return true;
        }
    }

    public static bool eliminarArchivos(string pathCarpeta)
    {

        if (!Directory.Exists(pathCarpeta))
        {
            return false;
        }
        else
        {
            // Obtenemos todos los archivos en la carpeta
            string[] carpeta = Directory.GetFiles(pathCarpeta);

            // Recorremos cada archivo y lo eliminamos
            foreach (string archivo in carpeta)
            {
                File.Delete(archivo);
            }
            return true;
        }

    }

    public static int obtenerNumeroDeArchivos(string pathCarpeta)
    {
        if (!Directory.Exists(pathCarpeta))
        {
            return -1;
        }
        else
        {
            string[] carpeta = Directory.GetFiles(pathCarpeta);

            return carpeta.Length;
        }
    }


    // METODOS PARA ENVIO DE PAQUETES - EMISION POR EVENTOS
    public static async Task<int> enviarFacturasGeneradasPorEvento(int codigoPuntoVenta, int codigoMotivoEvento, int capa)
    {

        int numeroFacturas = Helper.obtenerNumeroDeArchivos(Helper.pathCarpetaFacturasTelecom) + 1;

        // Facturas Telecom
        if (numeroFacturas > 1)
        {
            await Helper.actualizarPathFactura(1, capa, 2);
            Helper.empaquetarFacturas(Helper.pathCarpetaFacturasTelecom, Helper.pathArchivoCompreso);
            Helper.eliminarArchivos(Helper.pathCarpetaFacturasTelecom);
        }

        numeroFacturas = Helper.obtenerNumeroDeArchivos(Helper.pathCarpetaFacturasCompraVenta) + 1;

        // FacturasCompraVenta
        if (numeroFacturas > 1)
        {
            await Helper.actualizarPathFactura(2, capa, 2);
            Helper.empaquetarFacturas(Helper.pathCarpetaFacturasCompraVenta, Helper.pathArchivoCompreso);
            Helper.eliminarArchivos(Helper.pathCarpetaFacturasCompraVenta);
        }

        if (Helper.existenPaquetes(pathCarpetaPaquetesEventos, 2))
        {
            // PROCESO DE FACTURACION FUERA DE LINEA 
            if (await Helper.registrarEvento(codigoPuntoVenta, codigoMotivoEvento))
            {
                List<string> codigosRecepcion = await Helper.enviarPaquetes(codigoPuntoVenta);

                // Verificando que todos llegaron correctamente
                if(!codigosRecepcion.Contains("0"))
                {
                    await Helper.eventoController.eliminarEvento(evento.Codigoevento, false);
                    Helper.datosbase.Nropaquetetl = 1;
                    Helper.datosbase.Nropaquetecv = 1;
                    await Helper.datosBaseController.actualizarDatosBase(datosbase);

                    // Exito : Se enviaron todas los paquetes correctamente
                    return 1;
                }
                else
                {
                    // Error: No se envio uno o todos los paquetes
                    return -1;
                }
            }
            else
            {
                // Error: No se pudo registrar el evento 
                return -2;
            }
        }
        else
        {
            // Alerta: No existen facturas a enviar
            return 2;
        }

        
    }
    public static List<string> obtenerListaPaquetes(int tipoFactura, int codigoEmision, string pathCarpetaPaquetes)
    {
        string tipo = "";
        int numeroCaracteresPath = 50; // Numero de caracteres del path de facturacion por paquetes
        int n = 0;

        if(codigoEmision == 3)
        {
            numeroCaracteresPath = 48; // Numero de caracteres del path de facturacion masiva
        }

        if (tipoFactura == 1)
        {
            tipo = "Telecomunicacion";
            n = 16;
        }
        else if (tipoFactura == 2)
        {
            tipo = "CompraVenta";
            n = 11;
        }


        // Obtenemos todos los archivos de la carpeta
        string[] paquete = Directory.GetFiles(pathCarpetaPaquetes);

        List<string> paqueteTipo = new List<string>();

        // Buscamos los paquetes del tipo indicado
        foreach (string archivo in paquete)
        {
            if (archivo.Substring(numeroCaracteresPath, n) == tipo)
            {
                paqueteTipo.Add(archivo);
            }
        }

        return paqueteTipo;
    }

    public static bool existenPaquetes(string pathCarpetaPaquetes, int codigoEmision)
    {
        // Obteniendo lista de paths de paquetes agrupados por tipo donde 1:Telecom 2:CompraVenta 
        List<string> paquetesTelecom = Helper.obtenerListaPaquetes(1, codigoEmision, pathCarpetaPaquetes);
        List<string> paquetesCompraVenta = Helper.obtenerListaPaquetes(2, codigoEmision, pathCarpetaPaquetes);

        return paquetesTelecom.Count > 0 || paquetesCompraVenta.Count > 0;
    }

    public async static Task<bool> registrarEvento(int codigoPuntoVenta, int codigoMotivoEvento)
    {
        FacturacionOperacionesController operacionesController = new FacturacionOperacionesController();

        try
        {
            evento = await eventoController.ultimoEvento();
            await actualizarCodigos(evento.Codigopuntoventa);
            evento.Fechahorafinevento = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            evento.Cufd = general.Cufd;
            evento.Cuis = general.Cuis;

            var respuesta = (respuestaListaEventos)await operacionesController.registroEventoSignificativo(new solicitudEventoSignificativo()
            {
                codigoAmbiente = 2,
                codigoSistema = "723755F6B4AE27F5D61A57E",
                nit = 1024061022,
                codigoSucursal = 0,
                cuis = evento.Cuis,
                codigoPuntoVenta = evento.Codigopuntoventa,
                codigoMotivoEvento = evento.Codigomotivoevento,
                cufd = evento.Cufd,
                cufdEvento = evento.Cufdevento,
                descripcion = evento.Descripcion,
                fechaHoraInicioEvento = evento.Fechahorainicioevento,
                fechaHoraFinEvento = evento.Fechahorafinevento,
                codigoPuntoVentaSpecified = true
            });

            evento.Codigorecepcionevento = respuesta.codigoRecepcionEventoSignificativo;
            Console.WriteLine("CodigoRecepcionEvento: " + evento.Codigorecepcionevento);
            await eventoController.actualizarEvento(evento);

            if(evento.Codigorecepcionevento == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
            return false;
        }
    }

    public async static Task<List<string>> enviarPaquetes(int codigoPuntoVenta)
    {
        List<string> codigosRecepcion = new List<string>();
        evento = await eventoController.ultimoEvento();
        datosbase = await datosBaseController.listaDatosBases();

        codigosRecepcion = await enviarPaquetesTelecom(codigoPuntoVenta, 22, codigosRecepcion);
        codigosRecepcion = await enviarPaquetesCompraVenta(codigoPuntoVenta, 1, codigosRecepcion);
        return codigosRecepcion;
    }

    public static async Task<List<string>> enviarPaquetesTelecom(int codigoPuntoVenta, int codigoDocumentoSector, List<string> codigosRecepcion)
    {
        int numeroDeFacturas = numeroLimiteDeFacturasPorPaquete;
        int n = 66;

        FacturacionTelecomController controllerTelecom = new FacturacionTelecomController();
        List<string> paquetes = obtenerListaPaquetes(1, 2, pathCarpetaPaquetesEventos);
        int numeroDeDigitosPaquete = 1;

        if(paquetes.Count > 9)
        {
            numeroDeDigitosPaquete = 2;
        }

        foreach (string paquete in paquetes)
        {      
            // Preguntando si es el ultimo paquete, este talvez no tenga 500 facturas
            if ((paquetes.Count + "") == paquete.Substring(n, numeroDeDigitosPaquete))
            {

                numeroDeFacturas = (int)datosbase.Nrofacturapaquetetl - 1;
                if (numeroDeFacturas == 0)
                {
                    numeroDeFacturas = numeroLimiteDeFacturasPorPaquete;
                }
            }

            Console.WriteLine("Paquete Recepcionado: " + paquete);
            Console.WriteLine("Nro Facturas: " + numeroDeFacturas);

            try
            {
                ServiceTelecom.respuestaRecepcion respuesta = (ServiceTelecom.respuestaRecepcion)await controllerTelecom.getRecepcionPaqueteFactura(new ServiceTelecom.solicitudRecepcionPaquete()
                {
                    archivo = "no requerido",
                    cafc = evento.Cafctelecom,
                    cantidadFacturas = numeroDeFacturas,
                    codigoAmbiente = 2,
                    codigoDocumentoSector = codigoDocumentoSector,
                    codigoEmision = 2,
                    codigoEvento = evento.Codigorecepcionevento,
                    codigoModalidad = 1,
                    codigoPuntoVenta = codigoPuntoVenta,
                    codigoPuntoVentaSpecified = true,
                    codigoSistema = "723755F6B4AE27F5D61A57E",
                    codigoSucursal = 0,
                    cufd = "no requerido",
                    cuis = "no requerido",
                    fechaEnvio = "no requerido",
                    hashArchivo = "no requerido",
                    nit = 1024061022,
                    tipoFacturaDocumento = 1
                }, codigoPuntoVenta, paquete);

                if (respuesta.codigoRecepcion != null)
                {
                    codigosRecepcion.Add(respuesta.codigoRecepcion);
                    Console.WriteLine("CodigoRecepcionPaquete: " + respuesta.codigoRecepcion);
                    eliminarArchivo(paquete);
                }
                else
                {
                    codigosRecepcion.Add("0");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
                break;
            }

            numeroDeFacturas = numeroLimiteDeFacturasPorPaquete;
        }

        datosbase.Nrofacturapaquetetl = 1;
        await datosBaseController.actualizarDatosBase(datosbase);

        return codigosRecepcion;
    }

    public static async Task<List<string>> enviarPaquetesCompraVenta(int codigoPuntoVenta, int codigoDocumentoSector, List<string> codigosRecepcion)
    {
        int numeroDeFacturas = numeroLimiteDeFacturasPorPaquete;
        int n = 61;

        FacturacionCompraVentaController controllerCompraVenta = new FacturacionCompraVentaController();
        List<string> paquetes = obtenerListaPaquetes(2, 2, pathCarpetaPaquetesEventos);
        int numeroDeDigitosPaquete = 1;

        if(paquetes.Count > 9)
        {
            numeroDeDigitosPaquete = 2;
        }

        foreach (string paquete in paquetes)
        {
            // Preguntando si es el ultimo paquete, este talvez no tenga 500 facturas
            if ((paquetes.Count + "") == paquete.Substring(n, numeroDeDigitosPaquete))
            {

                numeroDeFacturas = (int)datosbase.Nrofacturapaquetecv - 1;
                if (numeroDeFacturas == 0)
                {
                    numeroDeFacturas = numeroLimiteDeFacturasPorPaquete;
                }
            }

            Console.WriteLine("Paquete Recepcionado: " + paquete);
            Console.WriteLine("Nro Facturas: " + numeroDeFacturas);

            try
            {
                ServiceCompraVenta.respuestaRecepcion respuesta = (ServiceCompraVenta.respuestaRecepcion)await controllerCompraVenta.getRecepcionPaqueteFactura(new ServiceCompraVenta.solicitudRecepcionPaquete()
                {
                    archivo = "no requerido",
                    cafc = evento.Cafccompraventa,
                    cantidadFacturas = numeroDeFacturas,
                    codigoAmbiente = 2,
                    codigoDocumentoSector = codigoDocumentoSector,
                    codigoEmision = 2,
                    codigoEvento = evento.Codigorecepcionevento,
                    codigoModalidad = 1,
                    codigoPuntoVenta = codigoPuntoVenta,
                    codigoPuntoVentaSpecified = true,
                    codigoSistema = "723755F6B4AE27F5D61A57E",
                    codigoSucursal = 0,
                    cufd = "no requerido",
                    cuis = "no requerido",
                    fechaEnvio = "no requerido",
                    hashArchivo = "no requerido",
                    nit = 1024061022,
                    tipoFacturaDocumento = 1
                }, codigoPuntoVenta, paquete);

                if (respuesta.codigoRecepcion != null)
                {
                    codigosRecepcion.Add(respuesta.codigoRecepcion);
                    Console.WriteLine("CodigoRecepcionPaquete: " + respuesta.codigoRecepcion);
                    eliminarArchivo(paquete);
                }
                else
                {
                    codigosRecepcion.Add("0");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }

            numeroDeFacturas = numeroLimiteDeFacturasPorPaquete;
        }

        datosbase.Nrofacturapaquetecv = 1;
        await datosBaseController.actualizarDatosBase(datosbase);

        return codigosRecepcion;
    }




    // METODOS PARA ENVIO DE PAQUETES - EMISION MASIVA

    public static async Task<ServiceTelecom.respuestaRecepcion> emisionMasiva(List<FacturaCompraVenta> listaFacturasCompraVenta, List<FacturaTelecomunicaciones> listaFacturasTelecom,int codigoPuntoVenta, int numeroDeFacturasCompraVenta, int numeroDeFacturasTelecom, int codigoEmision, int capa)
    {
        try 
        {
            // Armando XMLS y Firmando las Facturas Generadas
            ServiceTelecom.respuestaRecepcion respuesta = await generarPaquetesTelecomMasivas(listaFacturasTelecom, 1, capa);
            if(respuesta.transaccion == false)
            {
                return respuesta;
            }

            return await enviarFacturasGeneradasPorArchivo(codigoPuntoVenta, capa);
        }
        catch(Exception e)
        {
            Console.WriteLine($"{e.Message}");
            return new ServiceTelecom.respuestaRecepcion(){
                codigoEstado = 52,
                codigoDescripcion = "Ocurrio un error inesperado:" + e.Message,
                transaccion = true
            };
        }

    }

    
    public static async Task<List<FacturaCompraVenta>> generarFacturasCompraVentaMasivas(int codigoPuntoVenta, int numeroDeFacturas, int codigoEmision)
    {
        List<FacturaCompraVenta> listaFacturas = new List<FacturaCompraVenta>();

        for(int i=0; i<numeroDeFacturas; i++)
        {
            int tipoFactura = 2;  //Facturas Telecom = 1, Facturas Compra Venta = 2 
            await Helper.actualizarCodigos(codigoPuntoVenta);
            await Helper.actualizarNumerosFacturas();
            string xfechaEmision = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            int xcodigoEmision = codigoEmision;
            int xnumeroFactura = i+1;

            FacturaCompraVenta factura = new FacturaCompraVenta();

            factura.cafc = null;
            factura.nitEmisor = 1024061022;
            factura.razonSocialEmisor = "COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R.";
            factura.municipio = "Tarija";
            factura.telefono = "2846005";
            factura.numeroFactura = xnumeroFactura;
            factura.cuf = Helper.obtenerCuf(tipoFactura, xfechaEmision, xnumeroFactura, codigoPuntoVenta, Helper.general.Codigocontrol, xcodigoEmision);
            factura.cufd = Helper.general.Cufd;
            factura.codigoSucursal = 0;
            factura.direccion = "AV.JORGE LOPEZ #123";
            factura.codigoPuntoVenta = codigoPuntoVenta;
            factura.fechaEmision = xfechaEmision;
            factura.nombreRazonSocial = "Mi razon social";
            factura.codigoTipoDocumentoIdentidad = 1;
            factura.numeroDocumento = "5115889";
            factura.complemento = null;
            factura.codigoCliente = "51158891";
            factura.codigoMetodoPago = 1;
            factura.montoTotal = 100;
            factura.montoTotalSujetoIva = 100;
            factura.codigoMoneda = 1;
            factura.tipoCambio = 1;
            factura.montoTotalMoneda = 100;
            factura.leyenda = "Ley N° 453: Tienes derecho a recibir información sobre las características y contenidos de los servicios que utilices.";
            factura.usuario = "pperez";
            factura.codigoDocumentoSector = 1;
            

            // Detalle 
            //lista de detalles factura
            List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
            FacturaDetalleGral facturaDetalleGral = new FacturaDetalleGral();
            facturaDetalleGral.actividadEconomica = "461021";
            facturaDetalleGral.codigoProductoSin = 991009;
            facturaDetalleGral.codigoProducto = "JN-131231";
            facturaDetalleGral.descripcion = "Instalacion";
            facturaDetalleGral.cantidad = 1;
            facturaDetalleGral.unidadMedida = 1;
            facturaDetalleGral.precioUnitario = 100;
            facturaDetalleGral.montoDescuento = 0;
            facturaDetalleGral.subTotal = 100;
            listaDetalles.Add(facturaDetalleGral);
            factura.detalles = listaDetalles;
            listaFacturas.Add(factura);

            Thread.Sleep(10);
        }

        return listaFacturas;
    }

    public static async Task<List<FacturaTelecomunicaciones>> generarFacturasTelecomMasivas(int codigoPuntoVenta, int numeroDeFacturas, int codigoEmision)
    {
        List<FacturaTelecomunicaciones> listaFacturas = new List<FacturaTelecomunicaciones>();

        for(int i=0; i<numeroDeFacturas; i++)
        {
            int tipoFactura = 1;  //Facturas Telecom = 1, Facturas Compra Venta = 2 
            await Helper.actualizarCodigos(codigoPuntoVenta);
            await Helper.actualizarNumerosFacturas();
            string xfechaEmision = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            int xcodigoEmision = codigoEmision;
            int xnumeroFactura = i+1;

            FacturaTelecomunicaciones factura = new FacturaTelecomunicaciones();

            factura.cafc = null;
            factura.nitEmisor = 1024061022;
            factura.razonSocialEmisor = "COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R.";
            factura.municipio = "Tarija";
            factura.telefono = "2846005";
            factura.nitConjunto = null;
            factura.numeroFactura = xnumeroFactura;
            factura.cuf = Helper.obtenerCuf(tipoFactura, xfechaEmision, xnumeroFactura, codigoPuntoVenta, Helper.general.Codigocontrol, xcodigoEmision);
            factura.cufd = Helper.general.Cufd;
            factura.codigoSucursal = 0;
            factura.direccion = "AV.JORGE LOPEZ #123";
            factura.codigoPuntoVenta = codigoPuntoVenta;
            factura.fechaEmision = xfechaEmision;
            factura.nombreRazonSocial = "Mi razon social";
            factura.codigoTipoDocumentoIdentidad = 1;
            factura.numeroDocumento = "5115889";
            factura.complemento = null;
            factura.codigoCliente = "51158891";
            factura.codigoMetodoPago = 1;
            factura.montoTotal = 150;
            factura.montoTotalSujetoIva = 150;
            factura.codigoMoneda = 1;
            factura.tipoCambio = 1;
            factura.montoTotalMoneda = 150;
            factura.leyenda = "Ley N° 453: Tienes derecho a recibir información sobre las características y contenidos de los servicios que utilices.";
            factura.usuario = "Ricardo Mendez";
            factura.codigoDocumentoSector = 22;
            

            //lista de detalles factura
            List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
            FacturaDetalleGral facturaDetalleGral = new FacturaDetalleGral();
            facturaDetalleGral.actividadEconomica = "610000";
            facturaDetalleGral.codigoProductoSin = 84120;
            facturaDetalleGral.codigoProducto = "POST - 131231";
            facturaDetalleGral.descripcion = "PAGO DE MES DE ABRIL DE POSTPAGO";
            facturaDetalleGral.cantidad = 1;
            facturaDetalleGral.unidadMedida = 58;
            facturaDetalleGral.precioUnitario = 150;
            facturaDetalleGral.montoDescuento = null;
            facturaDetalleGral.subTotal = 150;
            listaDetalles.Add(facturaDetalleGral);
            factura.detalles = listaDetalles;
            listaFacturas.Add(factura);
            Thread.Sleep(10);
        }

        return listaFacturas;
    }

    // METODO DE GENERACION DE PAQUETES MASIVOS  A PARTIR DE LISTA DE OBJETOS
    public static async Task<respuestaRecepcion> generarPaquetesCompraVentaMasivas(List<FacturaCompraVenta> listaFacturasMasivas, int tipoFactura, int capa)
    {
        foreach(FacturaCompraVenta factura in listaFacturasMasivas)
        {
            await Helper.actualizarPathFactura(tipoFactura, capa, 3);

            // Realizando el proceso de serializacion, firma y compresion del archivo
            if (!await Helper.facturar(factura, tipoFactura, 2, 1, ""))
            {
                return new respuestaRecepcion(){
                    codigoEstado = 52,
                    codigoDescripcion = "Error: Por favor verificar que el programa Jacobitus Total este abierto y funcionando correctamente.",
                    transaccion = false
                };
            }

            // Actualizando numero de factura en Paquete
            Helper.datosbase.Nrofacturapaquetemasivocv = Helper.datosbase.Nrofacturapaquetemasivocv + 1;

            // GENERACION DE PAQUETE DE FACTURAS FUERA DE LINEA
            if (Helper.datosbase.Nrofacturapaquetemasivocv == Helper.numeroLimiteDeFacturasPorPaqueteMasivo + 1)
            {
                Helper.empaquetarFacturas("Impuestos/XML/FacturacionMasiva/FacturaCompraVenta", Helper.pathArchivoCompreso);

                Helper.datosbase.Nrofacturapaquetemasivocv = 1;
                Helper.datosbase.Nropaquetemasivocv = Helper.datosbase.Nropaquetemasivocv + 1;

                Helper.eliminarArchivos("Impuestos/XML/FacturacionMasiva/FacturaCompraVenta");
            }

            await Helper.datosBaseController.actualizarDatosBase(Helper.datosbase);   
        }

        return new respuestaRecepcion(){
            codigoEstado = 51,
            codigoDescripcion = "Exito: Todas las facturas fueron generadas correctamente.",
            transaccion = true
        };
    }
    

    public static async Task<ServiceTelecom.respuestaRecepcion> generarPaquetesTelecomMasivas(List<FacturaTelecomunicaciones> listaFacturasMasivas, int tipoFactura, int capa)
    {   
        foreach(FacturaTelecomunicaciones factura in listaFacturasMasivas)//observado
        {
            await Helper.actualizarPathFactura(tipoFactura, capa, 3);
            
            // Realizando el proceso de serializacion, firma y compresion del archivo
            if (!await Helper.facturar(factura, tipoFactura, 3, 1, ""))
            {
                return new ServiceTelecom.respuestaRecepcion(){
                    codigoRecepcion = "0",
                    codigoEstado = 52,
                    codigoDescripcion = "Error: Por favor verificar que el programa Jacobitus Total este abierto y funcionando correctamente.",
                    transaccion = false
                };
            }

            // Actualizando numero de factura en Paquete
            Helper.datosbase.Nrofacturapaquetemasivotl = Helper.datosbase.Nrofacturapaquetemasivotl + 1;

            // GENERACION DE PAQUETE DE FACTURAS FUERA DE LINEA
            if (Helper.datosbase.Nrofacturapaquetemasivotl == Helper.numeroLimiteDeFacturasPorPaqueteMasivo + 1)
            {
                Helper.empaquetarFacturas("Impuestos/XML/FacturacionMasiva/FacturaTelecomunicacion", Helper.pathArchivoCompreso);

                Helper.datosbase.Nrofacturapaquetemasivotl = 1;
                Helper.datosbase.Nropaquetemasivotl = Helper.datosbase.Nropaquetemasivotl + 1;

                Helper.eliminarArchivos("Impuestos/XML/FacturacionMasiva/FacturaTelecomunicacion");
            }

            await Helper.datosBaseController.actualizarDatosBase(Helper.datosbase);   
        }

        return new ServiceTelecom.respuestaRecepcion(){
            codigoEstado = 51,
            codigoDescripcion = "Exito: Todas las facturas fueron generadas correctamente.",
            transaccion = true
        };

    }

    public static async Task<ServiceTelecom.respuestaRecepcion> enviarFacturasGeneradasPorArchivo(int codigoPuntoVenta, int capa){
        
        int numeroFacturas = Helper.obtenerNumeroDeArchivos(Helper.pathCarpetaFacturasMasivasTelecom) + 1;

        // Facturas Telecom
        if (numeroFacturas > 1)
        {
            await Helper.actualizarPathFactura(1, capa, 3);
            Helper.empaquetarFacturas(Helper.pathCarpetaFacturasMasivasTelecom, Helper.pathArchivoCompreso);
            Helper.eliminarArchivos(Helper.pathCarpetaFacturasMasivasTelecom);
        }

        if (Helper.existenPaquetes(pathCarpetaPaquetesMasivos, 3))
        {
            try 
            {
                return await Helper.enviarPaquetesMasivos(codigoPuntoVenta);
            }
            catch(Exception e)
            {
                return new ServiceTelecom.respuestaRecepcion(){
                    codigoEstado = 51,
                    codigoDescripcion = "Ocurrio un error inesperado: "+ e.Message,
                    transaccion = false
                }; 
            }
        }

        // Alerta: No hay paquetes por enviar
        return new ServiceTelecom.respuestaRecepcion(){
            codigoEstado = 65,
            codigoDescripcion = "Alerta: No existen facturas a enviar. Por favor genere facturas e intente nuevamente. En caso de no haber generado facturas dentro del evento deshabilite el evento.",
            transaccion = false
        }; 
    }

    public async static Task<ServiceTelecom.respuestaRecepcion> enviarPaquetesMasivos(int codigoPuntoVenta)
    {
        List<string> codigosRecepcion = new List<string>();
        datosbase = await datosBaseController.listaDatosBases();

        return await enviarPaquetesMasivosTelecom(codigoPuntoVenta, 22, codigosRecepcion);
    }

    public static async Task<ServiceTelecom.respuestaRecepcion> enviarPaquetesMasivosTelecom(int codigoPuntoVenta, int codigoDocumentoSector, List<string> codigosRecepcion)
    {
        int numeroDeFacturas = numeroLimiteDeFacturasPorPaqueteMasivo;
        int n = 64;

        FacturacionTelecomController controllerTelecom = new FacturacionTelecomController();
        List<string> paquetes = obtenerListaPaquetes(1, 3, pathCarpetaPaquetesMasivos);
        int numeroDeDigitosPaquete = 1;

        if(paquetes.Count > 9)
        {
            numeroDeDigitosPaquete = 2;
        }

        foreach (string paquete in paquetes)
        {      
            // Preguntando si es el ultimo paquete, este talvez no tenga 500 facturas
            if ((paquetes.Count + "") == paquete.Substring(n, numeroDeDigitosPaquete))
            {

                numeroDeFacturas = (int)datosbase.Nrofacturapaquetemasivotl - 1;
                if (numeroDeFacturas == 0)
                {
                    numeroDeFacturas = numeroLimiteDeFacturasPorPaqueteMasivo;
                }
            }

            Console.WriteLine("Paquete Recepcionado: " + paquete);
            Console.WriteLine("Nro Facturas: " + numeroDeFacturas);

            try
            {
                ServiceTelecom.respuestaRecepcion respuesta = (ServiceTelecom.respuestaRecepcion)await controllerTelecom.getRecepcionMasivaFactura(new ServiceTelecom.solicitudRecepcionMasiva()
                {
                    archivo = "No requerido",
                    cantidadFacturas = numeroDeFacturas,
                    codigoAmbiente = 2,
                    codigoDocumentoSector = codigoDocumentoSector,
                    codigoEmision = 3,
                    codigoModalidad = 1,
                    codigoPuntoVenta = codigoPuntoVenta,
                    codigoPuntoVentaSpecified = true,
                    codigoSistema = "723755F6B4AE27F5D61A57E",
                    codigoSucursal = 0,
                    cufd = "No requerido",
                    cuis = "No requerido",
                    fechaEnvio = "No requerido",
                    hashArchivo = "No requerido",
                    nit = 1024061022,
                    tipoFacturaDocumento = 1
                }, codigoPuntoVenta, paquete);


                datosbase.Nrofacturapaquetemasivotl = 1;
                await datosBaseController.actualizarDatosBase(datosbase);
                numeroDeFacturas = numeroLimiteDeFacturasPorPaqueteMasivo;

                if (respuesta.codigoRecepcion != null)
                {
                    codigosRecepcion.Add(respuesta.codigoRecepcion);
                    Console.WriteLine("CodigoRecepcionPaqueteMasivo: " + respuesta.codigoRecepcion);
                    eliminarArchivo(paquete);

                    return await validarRecepcionPaqueteMasivo(codigoPuntoVenta,1,respuesta.codigoRecepcion);
                }
                else
                {
                    eliminarArchivo(paquete);
                    return respuesta;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
                datosbase.Nrofacturapaquetemasivotl = 1;
                await datosBaseController.actualizarDatosBase(datosbase);

                return new ServiceTelecom.respuestaRecepcion(){
                    codigoEstado = 51,
                    codigoDescripcion = "Ocurrio un error inesperado: "+ e.Message,
                    transaccion = false
                }; 
            }
        }

        return new ServiceTelecom.respuestaRecepcion(){
            codigoEstado = 51,
            codigoDescripcion = "Ocurrio un error inesperado: Por favor comuniquese con personal de soporte",
            transaccion = true
        };
    }

    public static async Task<List<string>> enviarPaquetesMasivosCompraVenta(int codigoPuntoVenta, int codigoDocumentoSector, List<string> codigosRecepcion)
    {
        int numeroDeFacturas = numeroLimiteDeFacturasPorPaqueteMasivo;
        int n = 59;

        FacturacionCompraVentaController controllerCompraVenta = new FacturacionCompraVentaController();
        List<string> paquetes = obtenerListaPaquetes(2, 3, pathCarpetaPaquetesMasivos);
        int numeroDeDigitosPaquete = 1;

        if(paquetes.Count > 9)
        {
            numeroDeDigitosPaquete = 2;
        }

        foreach (string paquete in paquetes)
        {
            // Preguntando si es el ultimo paquete, este talvez no tenga 500 facturas
            if ((paquetes.Count + "") == paquete.Substring(n, numeroDeDigitosPaquete))
            {

                numeroDeFacturas = (int)datosbase.Nrofacturapaquetemasivocv - 1;
                if (numeroDeFacturas == 0)
                {
                    numeroDeFacturas = numeroLimiteDeFacturasPorPaqueteMasivo;
                }
            }

            Console.WriteLine("Paquete Recepcionado: " + paquete);
            Console.WriteLine("Nro Facturas: " + numeroDeFacturas);

            try
            {
                ServiceCompraVenta.respuestaRecepcion respuesta = (ServiceCompraVenta.respuestaRecepcion)await controllerCompraVenta.getRecepcionMasivaFactura(new ServiceCompraVenta.solicitudRecepcionMasiva()
                {
                    archivo = "no requerido",
                    cantidadFacturas = numeroDeFacturas,
                    codigoAmbiente = 2,
                    codigoDocumentoSector = codigoDocumentoSector,
                    codigoEmision = 3,
                    codigoModalidad = 1,
                    codigoPuntoVenta = codigoPuntoVenta,
                    codigoPuntoVentaSpecified = true,
                    codigoSistema = "723755F6B4AE27F5D61A57E",
                    codigoSucursal = 0,
                    cufd = "no requerido",
                    cuis = "no requerido",
                    fechaEnvio = "no requerido",
                    hashArchivo = "no requerido",
                    nit = 1024061022,
                    tipoFacturaDocumento = 1
                }, codigoPuntoVenta, paquete);

                if (respuesta.codigoRecepcion != null)
                {
                    codigosRecepcion.Add(respuesta.codigoRecepcion);
                    Console.WriteLine("CodigoRecepcionPaqueteMasivo: " + respuesta.codigoRecepcion);
                    eliminarArchivo(paquete);
                    
                    await validarRecepcionPaqueteMasivo(codigoPuntoVenta,2,respuesta.codigoRecepcion);
                }
                else
                {
                    codigosRecepcion.Add("0");
                    eliminarArchivo(paquete);
                }

                String mensaje = "Termino de enviar 1 paquete de 1000 a fecha: " + DateTime.Now.ToString() +" Respuesta: "+ respuesta.codigoDescripcion + "Observaciones:" + respuesta.mensajesList[0]?.descripcion;
                //creamos nuestro objeto de la clase que hicimos
                EnviadorCorreos enviador2 = new EnviadorCorreos("correopruebascosett@gmail.com","franciscojavierlopezperez85@gmail.com", mensaje, "Facturacion Masiva Pruebas");
                enviador2.enviaMail();
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }

            numeroDeFacturas = numeroLimiteDeFacturasPorPaqueteMasivo;
        }

        datosbase.Nrofacturapaquetemasivocv = 1;
        await datosBaseController.actualizarDatosBase(datosbase);

        return codigosRecepcion;
    }


    public static async Task<ServiceTelecom.respuestaRecepcion> validarRecepcionPaqueteMasivo(int codigoPuntoVenta, int tipo, string codigoRecepcion)
    {
        if(tipo == 1)
        {
            FacturacionTelecomController controller = new FacturacionTelecomController();
            ServiceTelecom.respuestaRecepcion respuestaRecepcion = await controller.getValidacionRecepcionMasivaFactura(new ServiceTelecom.solicitudValidacionRecepcion() {
                codigoAmbiente = 2,
                codigoDocumentoSector = 22,
                codigoEmision = 3,
                codigoModalidad = 1,
                codigoPuntoVenta = 0,
                codigoPuntoVentaSpecified = true,
                codigoRecepcion = codigoRecepcion,
                codigoSistema = "No requerido",
                codigoSucursal = 0,
                cufd = "No requerido",
                cuis = "No requerido",
                nit = 0,
                tipoFacturaDocumento = 1
            }, codigoPuntoVenta);

            Console.WriteLine("Validacion Paquete:" + respuestaRecepcion.codigoDescripcion);
            return respuestaRecepcion;
        }

        return new ServiceTelecom.respuestaRecepcion(){
            codigoEstado = 51,
            codigoDescripcion = "Ocurrio un error inesperado: Por favor comuniquese con personal de soporte",
            transaccion = true
        };
    }




    // METODOS UTILIZADOS PARA ENVIO DE ARCHIVOS POR CORREO
    public static bool reconstruirFacturaXML(string facturaBase64, string pathArchivo)
    {

        try
        {
            // Convirtiendo string Base64 a Array de Bytes
            byte[] archivoBytes = convertirBase64StringToArray(facturaBase64);

            // Convirtiendo array de bytes en archivo xml
            convertirArrayByteToArchivoXML(archivoBytes, pathArchivo);
            return true;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        
    }
    


    //MODELOS DE SOLICITUD Y RESPUESTA API JACOBITUS

    public class SolicitudFirmaXml
    {

        //Opcional
        public long slot { get; set; }
        public string pin { get; set; }
        public string alias { get; set; }
        public string file { get; set; }

    }

    public class RespuestaFirmaXml
    {
        public bool finalizado { get; set; }
        public string mensaje { get; set; }
        public Datos datos { get; set; }
    }

    public class Datos
    {
        public string xml { get; set; }
    }

    public class SolicitudDatosCertificado
    {

        //Opcional
        public long slot { get; set; }
        public string pin { get; set; }

    }

    public class RespuestaCertificado
    {
        public string id { get; set; }
        public string fechaInicio { get; set; }
        public string emisor { get; set; }
    }

}

