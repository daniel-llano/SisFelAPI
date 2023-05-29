using AutoMapper;
using NPOI.XSSF.UserModel;
using SisFelApi.Impuestos.ServiceCommon.Armado_de_Objetos.modelos;
using SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos;
using SisFelApi.Negocio.Controllers;
using SisFelApi.Negocio.Data;
using SisFelApi.Negocio.Models;
using System;
using System.Diagnostics;
using System.Globalization;

namespace SisFelApi.Impuestos.ServiceCommon.Armado_de_Objetos
{
    public class MetodosLecturacion
    {
        public List<string> erroresCabeceras = new List<string>();
        public List<string> erroresDetalles = new List<string>();
        private readonly IMapper _mapper;

        public MetodosLecturacion(IMapper mapper)
        {
            _mapper = mapper;
        }

        //metodo para el armado de objetos de tipo factura telecomunicaciones
        //metodo para la lecturacion de los txt, donde como primer parametros se le manda el directorio del txt de la cabecera, y segundo parametro se envia el directorio del txt con los detalles
        //public List<FacturaTxt> LecturaTxt(string directoryCab, string directoryDet)
        public ObjMasivo LecturaTxt(string directoryCab, string directoryDet)
        {
            List<FacturaTxt> facturas = new List<FacturaTxt>();
            ObjMasivo objMasivo=new ObjMasivo();
            var cortar = false;
            foreach (string lineCab in File.ReadAllLines(directoryCab))
            {
                var objCab = lineCab.Split('|');
                if (verificarTamanioTxt(1, objCab)==true)
                {
                    break;
                }
                 objCab = verificarObjCab(objCab);
                if (erroresCabeceras.Count > 0)
                {
                    break;
                }
                FacturaTxt factura = new FacturaTxt();
                List<DetalleTxt> detalles = new List<DetalleTxt>();
                foreach (var lineDet in File.ReadAllLines(directoryDet))
                {
                    var objDet = lineDet.Split('|');
                    if (verificarTamanioTxt(2, objDet) == true)
                    {
                        break;
                    }
                    objDet = verificarObjDet(objDet);
                    if (erroresDetalles.Count>0)
                    {
                        cortar= true;
                        break;
                    }
                    if (objCab[0].Equals(objDet[0]))
                    {
                        DetalleTxt detalle = new DetalleTxt();
                        detalle.actividadEconomica = "610000";//observado
                        detalle.codigoProductoSin = 84120;
                        detalle.codigoProducto = objDet[3];//"1";//objDet[3];observado
                        detalle.descripcion = objDet[4];
                        detalle.cantidad = decimal.Parse(objDet[6]);
                        //detalle.unidadMedida = int.Parse(objDet[7]);
                        detalle.unidadMedida = int.Parse("64");
                        detalle.precioUnitario =  float.Parse(objDet[8]);
                        detalle.montoDescuento = null;
                        detalle.subTotal = float.Parse(objDet[9]);
                        detalle.Cuenta = objDet[5];
                        detalles.Add(detalle);
                    }
                }
                if (cortar)
                {
                    break;
                }
                factura.nitEmisor = int.Parse(objCab[1]);
                factura.razonSocialEmisor = "COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R.";
                factura.municipio = "Tarija";
                factura.telefono = "2846005";
                factura.nitConjunto = null;
                factura.numeroFactura = long.Parse(objCab[4]);
                factura.cuf = "";
                factura.cufd = "";
                factura.codigoSucursal = 0;
                factura.direccion = "AV.JORGE LOPEZ #123";
                factura.fechaEmision = objCab[5];
                factura.nombreRazonSocial = objCab[11];
                factura.codigoTipoDocumentoIdentidad = tipoDocumento(objCab[10]);
                factura.numeroDocumento = objCab[10];
                factura.complemento = null;
                factura.codigoCliente = objCab[0];
                factura.codigoMetodoPago = 1;
                factura.codigoMoneda = 1;
                factura.montoTotal = float.Parse(objCab[6]);
                factura.montoTotalSujetoIva = float.Parse(objCab[6]) - float.Parse(objCab[7]);
                factura.codigoPuntoVenta = 1;
                factura.tipoCambio = 1;
                factura.montoTotalMoneda = factura.montoTotal;
                factura.leyenda = "Ley N° 453: Tienes derecho a recibir información sobre las características y contenidos de los servicios que utilices.";
                factura.usuario = "admin";
                factura.codigoDocumentoSector = 22;
                factura.detalles = detalles;
                factura.cafc = null;
                facturas.Add(factura);
            }
            objMasivo.facturaTxt = facturas;
            objMasivo.erroresCabecera = erroresCabeceras;
            objMasivo.erroresDetalle = erroresDetalles;
            return objMasivo;
        }
        //14 cab tamño del txt
        //11 det tamañ del txt
        //validando el archivo TXT de tipo CABECERA
        public string[] verificarObjCab(string[] obj)
        {
            string campo = "";
            var error = "";
            //validacion para el nit emisor
            if (int.TryParse(obj[1], out int nit))
            {
                obj[1] = obj[1];
            }
            else
            {
                campo = "NIT EMISOR";
                error = capturandoErrores(obj[0], campo, obj[1]);
                erroresCabeceras.Add(error);
            }
            //validacion para el num de factura
            if (int.TryParse(obj[4], out int num))
            {
                obj[4] = obj[4];
            }
            else
            {
                campo = "NUMERO DE FACTURA";
                error = capturandoErrores(obj[0], campo, obj[4]);
                erroresCabeceras.Add(error);
            }
            //validacion para la fecha de emision
            if (DateTime.TryParse(convertFecha(obj[5]), out DateTime fecha))
            {
                obj[5] = (DateTime.Parse(convertFecha(obj[5]))).ToString();
            }
            else
            {
                campo = "FECHA DE EMISION";
                error = capturandoErrores(obj[0], campo, obj[5]);
                erroresCabeceras.Add(error);
            }
            //validacion para el nombre o razon social
            if (obj[11] == "")
            {
                campo = "NOMBRE O RAZON SOCIAL";
                error = capturandoErrores(obj[0], campo, obj[11]);
                erroresCabeceras.Add(error);
            }
            // validacion para el num de documento
            if(int.TryParse(obj[10], out int numerodocumento) && obj[10] == "")
            {
                campo = "NUMERO DE DOCUMENTO";
                error = capturandoErrores(obj[0], campo, obj[10]);
                erroresCabeceras.Add(error);
            }
            if (obj[10] == "")
            {
                obj[10] = "0";
            }
            //validacion para el codigo cliente
            if (obj[0] == "")
            {
                campo = "CODIGO CLIENTE";
                error = capturandoErrores(obj[0], campo, obj[0]);
                erroresCabeceras.Add(error);
            }
            //validacion para el monto total
            if (decimal.TryParse(obj[6], out decimal num1))
            {
                obj[6] = cultureInfo(obj[6]).ToString();
            }
            else
            {
                campo = "MONTO TOTAL";
                error = capturandoErrores(obj[0], campo, obj[6]);
                erroresCabeceras.Add(error);
            }
            //validacion para el monto total sujeto a IVA (TASA)
            if (obj[7] == "")
            {
                obj[7] = "0";
            }
            if (float.TryParse(obj[7], out float tasa))
            {
                obj[7] = cultureInfo(obj[7]).ToString();
            }
            else
            {
                campo = "MONTO TOTAL SUJETO A IVA";
                error = capturandoErrores(obj[0], campo, obj[7]);
                erroresCabeceras.Add(error);
            }
            return obj;
        }
        //validando el archivo TXT de tipo DETALLE
        public string[] verificarObjDet(string[] obj)
        {
            var campo = "";
            var error = "";
           
            //validando el codigo producto //1ra validacion del detalle
            if (obj[3] == "")
            {
               campo = "CODIGO PRODUCTO";
               error=capturandoErrores(obj[0],campo, obj[3]);
               erroresDetalles.Add(error);
            }
            //validando la descripcion
            if (obj[4] == "")
            {
                campo = "DESCRIPCION";
                error = capturandoErrores(obj[0], campo, obj[4]);
                erroresDetalles.Add(error);
            }
            //validando la cuenta
            if (obj[5] == "")
            {
                campo = "CUENTA";
                error = capturandoErrores(obj[0], campo, obj[5]);
                erroresDetalles.Add(error);
            }
            //validando la cantidad
            if (decimal.TryParse(obj[6], out decimal num1))
            {
                obj[6] = cultureInfo(obj[6]).ToString();
            }
            else if(obj[6] == ""){
                obj[6] = "1";
            }
            else
            {
                campo = "CANTIDAD";
                error = capturandoErrores(obj[0], campo, obj[6]);
                erroresDetalles.Add(error);
            }
            //validando la unidadad medida
            // if (obj[7] != "")
            // {
            //     var almacenadora = obj[7];
            //     //campo = "UNIDAD MEDIDA";
            //     obj[7] = buscarParametro(obj[7]);
            //     if (obj[7] == "0")
            //     {
            //         campo = "UNIDAD MEDIDA";
            //         error = capturandoErrores(obj[0], campo, almacenadora+ ", No Existe la Unidad Medida enviada");
            //         erroresDetalles.Add(error);
            //     }
            // }
            // else
            // {
            //     campo = "UNIDAD MEDIDA";
            //     error = capturandoErrores(obj[0], campo, obj[7]);
            //     erroresDetalles.Add(error);
            // }
            //validando el precio unitario
            if (float.TryParse(obj[8], out float precio))
            {
                obj[8] = decimal.Round(cultureInfo(obj[8]),2).ToString();
            }
            else if(obj[8] == "")
            {
                //validando el subtotal
                if (float.TryParse(obj[9], out float subTotal1))
                {
                    obj[8] = cultureInfo(obj[9]).ToString();
                }
                else
                {
                    campo = "SUB TOTAL";
                    error = capturandoErrores(obj[0], campo, obj[8]);
                    erroresDetalles.Add(error);
                }
            }
            else
            {
                campo = "PRECIO UNITARIO";
                error = capturandoErrores(obj[0], campo, obj[8]);
                erroresDetalles.Add(error);
            }
            //validando el subtotal
            if (float.TryParse(obj[9], out float subTotal))
            {
                obj[9] = cultureInfo(obj[9]).ToString();
            }
            else
            {
                campo = "SUB TOTAL";
                error = capturandoErrores(obj[0], campo, obj[9]);
                erroresDetalles.Add(error);
            }
            return obj;
        }
        public decimal cultureInfo(string numero)
        {
            decimal res = Convert.ToDecimal(numero, CultureInfo.InvariantCulture);
            decimal numeroConvertido = Decimal.Parse(string.Format("{0:f2}", res));
            return res;
        }
        public string convertFecha(string fecha)
        {
            if (fecha.Equals(""))
            {
                return "";
            }
            char[] letras = fecha.ToCharArray();
            return "" + letras[0] + letras[1] + letras[2] + letras[3] + "-" + letras[4] + letras[5] + "-" + letras[6] + letras[7];
        }
        public string capturandoErrores(string cliente, string campo, string valor)
        {
            string sms = "Error en la Factura del Cliente: " + cliente + ", en el Campo: " + campo + ", con el Valor: " + valor;
            //Console.WriteLine(sms);
            return sms;
        }
        public string capturandoErroresGral(string archivo, string campo, string valor)
        {
            string sms = "Error en el Archivo Enviado: " + archivo + ", con el error: " + campo + ", con la descripcion: " + valor;
            //Console.WriteLine(sms);
            return sms;
        }
        //1:carnet, 5:nit
        public int tipoDocumento(string numero)
        {
            var tipo = 0;
            var tamaño = numero.ToCharArray();
            if (tamaño.Length>8)
            {
                tipo = 5;
            }
            if (tamaño.Length<8)
            {
                tipo = 1;
            }
          
            return tipo;
        }
        public string buscarParametro(string nombreParametro)
        {
            var codigo = "0";
            SisfelbdContext _context = new SisfelbdContext();
            ParametroController parametroController = new ParametroController(_context);
            var lista = parametroController.getListaUnidadesMedida();
            foreach (var item in lista)
            {
                if (item.Nombreparametro.Equals(nombreParametro.ToUpper()))
                {
                    codigo = item.Codigoparametro.ToString();
                }
            }
            return codigo;
        }

        public List<FacturaTelecomunicaciones> mapeo(List<FacturaTxt> facturaTxts)
        {
            List<FacturaTelecomunicaciones> lista = new List<FacturaTelecomunicaciones>();
            
            foreach (var facturas in facturaTxts)
            {
                FacturaTelecomunicaciones factura = new FacturaTelecomunicaciones();
                List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
                factura.nitEmisor = facturas.nitEmisor;
                factura.municipio = facturas.municipio;
                factura.telefono = facturas.telefono;
                factura.nitConjunto = facturas.nitConjunto;
                factura.numeroFactura = facturas.numeroFactura;
                factura.cuf = facturas.cuf;
                factura.cufd = facturas.cufd;
                factura.codigoSucursal = facturas.codigoSucursal;
                factura.direccion = facturas.direccion;
                factura.codigoPuntoVenta = facturas.codigoPuntoVenta;
                factura.fechaEmision = facturas.fechaEmision;
                factura.nombreRazonSocial = facturas.nombreRazonSocial;
                factura.codigoTipoDocumentoIdentidad = facturas.codigoTipoDocumentoIdentidad;
                factura.numeroDocumento = facturas.numeroDocumento;
                factura.complemento = facturas.complemento;
                factura.codigoCliente = facturas.codigoCliente;
                factura.codigoMetodoPago = facturas.codigoMetodoPago;
                factura.montoTotal = facturas.montoTotal;
                factura.montoTotalSujetoIva = facturas.montoTotalSujetoIva;
                factura.leyenda = facturas.leyenda;
                factura.usuario = facturas.usuario;
                factura.codigoDocumentoSector = facturas.codigoDocumentoSector;
                factura.cafc = facturas.cafc;
                factura.codigoMoneda = facturas.codigoMoneda;
                factura.tipoCambio = facturas.tipoCambio;
                factura.montoTotalMoneda = facturas.montoTotalMoneda;
                factura.razonSocialEmisor = facturas.razonSocialEmisor;
                foreach (var detalles in facturas.detalles)
                {
                    FacturaDetalleGral detalle = new FacturaDetalleGral();
                    detalle.codigoProducto = detalles.codigoProducto;
                    detalle.actividadEconomica = detalles.actividadEconomica;
                    detalle.codigoProductoSin = detalles.codigoProductoSin;
                    detalle.descripcion = detalles.descripcion;
                    detalle.cantidad = detalles.cantidad;
                    detalle.unidadMedida = detalles.unidadMedida;
                    detalle.precioUnitario = detalles.precioUnitario;
                    detalle.montoDescuento = detalles.montoDescuento;
                    detalle.subTotal = detalles.subTotal;
                    listaDetalles.Add(detalle);
                }
                factura.detalles= listaDetalles;
                lista.Add(factura);
            }
            return lista;
        }
    
        public bool verificarTamanioTxt(int tipo, string[] obj)
        {
            var estado = false;
            if (tipo==1)
            {
                //validacion para el tamaño del obj cabecera
                if (obj.Length != 14)
                {
                    var archivo = "ARCHIVO CABECERA";
                    var campo = "TAMAÑO DEL ARCHIVO";
                    var error = capturandoErroresGral(archivo, campo, "tamaño: " + obj.Length.ToString());
                    erroresCabeceras.Add(error);
                    estado = true;
                }
                //
            }
            if (tipo==2)
            {
                //validacion para el tamaño del obj detalle
                if (obj.Length != 11)
                {
                    var archivo = "ARCHIVO DETALLE";
                    var campo = "TAMAÑO DEL ARCHIVO";
                    var error = capturandoErroresGral(archivo, campo, "tamaño: " + obj.Length.ToString());
                    erroresDetalles.Add(error);
                    estado= true;
                }
            }
            return estado;
        }
    }
}
