using SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos;
using System.Xml.Serialization;
using SisFelApi.Negocio.Models;
using System.Xml.Linq;
using SisFelApi.Impuestos.ServiceCommon.Serializacion.Facturas;
using System.Xml;

public class Serializacion
{
    public void GenerarFacturasXml(){
        GenerarFacturaTelecomunicacionesXml(LlenarFacturaTelecomunicaciones(),"Impuestos/XML/facturaElectronicaTelecomunicacion.xml");
        GenerarFacturaCompraVentaXml(LlenarFacturaCompraVenta(),"Impuestos/XML/facturaElectronicaCompraVenta.xml");
    }
     //serialización del objeto enviado y almacenado en una dirección proveniente del proyecto segun el tipo que se envie; 1 es de telecomunicaciones y 2 de compra y venta
    public void SerializarObjeto(Object objet, int tipo, string pathArchivo, List<DetalleFacturaGralXml> detalles)
    {
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        XmlSerializer x = new XmlSerializer(objet.GetType());

        if(tipo==1){
            FileStream fileStream1 = File.Open(pathArchivo, FileMode.Create,FileAccess.Write);
            x.Serialize(fileStream1,objet);
            fileStream1.Close();
            XElement newDocXML = XElement.Load(pathArchivo);
            foreach (var item in detalles)
            {

                XElement newElement = new XElement("detalle",
                new XElement("actividadEconomica", item.actividadEconomica),
                new XElement("codigoProductoSin", item.codigoProductoSin),
                new XElement("codigoProducto", item.codigoProducto),
                new XElement("descripcion", item.descripcion),
                new XElement("cantidad", item.cantidad),
                new XElement("unidadMedida", item.unidadMedida),
                new XElement("precioUnitario", item.precioUnitario),
                new XElement("montoDescuento", item.montoDescuento),
                new XElement("subTotal", item.subTotal),
                new XElement("numeroSerie", new XAttribute(xsi + "nil", true)),
                new XElement("numeroImei", new XAttribute(xsi + "nil", true)));
                newDocXML.Add(newElement);
            }
            newDocXML.Save(pathArchivo);
        }
        else if(tipo==2){
            FileStream fileStream2 = File.Open(pathArchivo, FileMode.Create,FileAccess.Write);
            x.Serialize(fileStream2,objet);
            fileStream2.Close();
            XElement newDocXML = XElement.Load(pathArchivo);
            foreach (var item in detalles)
            {
                XElement newElement = new XElement("detalle",
                new XElement("actividadEconomica", item.actividadEconomica),
                new XElement("codigoProductoSin", item.codigoProductoSin),
                new XElement("codigoProducto", item.codigoProducto),
                new XElement("descripcion", item.descripcion),
                new XElement("cantidad", item.cantidad),
                new XElement("unidadMedida", item.unidadMedida),
                new XElement("precioUnitario", item.precioUnitario),
                new XElement("montoDescuento", item.montoDescuento),
                new XElement("subTotal", item.subTotal),
                new XElement("numeroSerie", new XAttribute(xsi + "nil", true)),
                new XElement("numeroImei", new XAttribute(xsi + "nil", true)));
                newDocXML.Add(newElement);
            }
            newDocXML.Save(pathArchivo);
        }else if(tipo==3){
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            FileStream fileStream = File.Open(pathArchivo, FileMode.Create,FileAccess.Write);
            x.Serialize(fileStream,objet, namespaces);
            fileStream.Close();
        }        
    }
    //llenado de datos ficticios en las facturas de telecom y com-ven
    public FacturaTelecomunicaciones LlenarFacturaTelecomunicaciones()
    {
        //lista de detalles factura
        List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
        FacturaDetalleGral facturaDetalleGral = new FacturaDetalleGral();
        facturaDetalleGral.actividadEconomica = "451010";
        facturaDetalleGral.codigoProductoSin = 49711;
        facturaDetalleGral.codigoProducto = "POST - 137231";
        facturaDetalleGral.descripcion = "PAGO DE MES DE ABRIL DE FIBRA OPTICA";
        facturaDetalleGral.cantidad = 1;
        facturaDetalleGral.unidadMedida = 58;
        facturaDetalleGral.precioUnitario = 150;
        facturaDetalleGral.montoDescuento = null;
        facturaDetalleGral.subTotal = 150;
        listaDetalles.Add(facturaDetalleGral);
        //
        FacturaDetalleGral facturaDetalleGral1 = new FacturaDetalleGral();
        facturaDetalleGral1.actividadEconomica = "451010";
        facturaDetalleGral1.codigoProductoSin = 49111;
        facturaDetalleGral1.codigoProducto = "POST - 131231";
        facturaDetalleGral1.descripcion = "PAGO DE MES DE ABRIL DE POSTPAGO";
        facturaDetalleGral1.cantidad = 1;
        facturaDetalleGral1.unidadMedida = 58;
        facturaDetalleGral1.precioUnitario = 150;
        facturaDetalleGral1.montoDescuento = null;
        facturaDetalleGral1.subTotal = 150;
        listaDetalles.Add(facturaDetalleGral1);
        //
        FacturaTelecomunicaciones obj = new FacturaTelecomunicaciones();
        obj.nitEmisor = 1003579028;
        obj.razonSocialEmisor = "Carlos Loza";
        obj.municipio = "La Paz";
        obj.telefono = "2846005";
        obj.nitConjunto =null;
        obj.numeroFactura = 1;
        obj.cuf = "44AAEC00DBD34C81DFBDAB7C52067B9DA7A5E686A267A75AC82F24C74";
        obj.cufd = "BQUE+QytqQUDBKVUFOSVRPQkxVRFZNVFVJBMDAwMDAwM";
        obj.codigoSucursal = 0;
        obj.direccion = "AV.JORGE LOPEZ #123";
        obj.codigoPuntoVenta = 0;
        obj.fechaEmision = "2021-10-07T09:55:46.414";
        obj.nombreRazonSocial = "Mi razon social";
        obj.codigoTipoDocumentoIdentidad = 1;
        obj.numeroDocumento = "5115889";
        obj.complemento = null;
        obj.codigoCliente = "51158891";
        obj.codigoMetodoPago = 1;
        obj.montoTotal = 150;
        obj.montoTotalSujetoIva = 150;
        obj.codigoMoneda = 1;
        obj.tipoCambio = 1;
        obj.montoTotalMoneda = 150;
        obj.leyenda = "Ley N° 453: Tienes derecho a recibir información sobre las características y contenidos de los servicios que utilices.";
        obj.usuario = "Ricardo Mendez";
        obj.codigoDocumentoSector = 22;
        obj.detalles = listaDetalles;
        return obj;
    }
    public FacturaCompraVenta LlenarFacturaCompraVenta()
    {
        //lista de detalles factura
        List<FacturaDetalleGral> listaDetalles = new List<FacturaDetalleGral>();
        FacturaDetalleGral facturaDetalleGral = new FacturaDetalleGral();
        facturaDetalleGral.actividadEconomica = "451010";
        facturaDetalleGral.codigoProductoSin = 49111;
        facturaDetalleGral.codigoProducto = "JN-131231";
        facturaDetalleGral.descripcion = "JUGO DE NARANJA EN VASO";
        facturaDetalleGral.cantidad = 1;
        facturaDetalleGral.unidadMedida = 1;
        facturaDetalleGral.precioUnitario = 100;
        facturaDetalleGral.montoDescuento = null;
        facturaDetalleGral.subTotal = 100;
        listaDetalles.Add(facturaDetalleGral);
        //
        FacturaDetalleGral facturaDetalleGral1 = new FacturaDetalleGral();
        facturaDetalleGral1.actividadEconomica = "12345";
        facturaDetalleGral1.codigoProductoSin = 49856;
        facturaDetalleGral1.codigoProducto = "JN-131313";
        facturaDetalleGral1.descripcion = "JUGO DE NARANJA EN CAJA";
        facturaDetalleGral1.cantidad = 1;
        facturaDetalleGral1.unidadMedida = 1;
        facturaDetalleGral1.precioUnitario = 100;
        facturaDetalleGral1.montoDescuento = 20;
        facturaDetalleGral1.subTotal = 100;
        listaDetalles.Add(facturaDetalleGral1);
        //
        FacturaCompraVenta obj = new FacturaCompraVenta();
        obj.nitEmisor = 1003579028;
        obj.razonSocialEmisor = "Carlos Loza";
        obj.municipio = "La Paz";
        obj.telefono = "2846005";
        obj.numeroFactura = 1;
        obj.cuf = "44AAEC00DBD34C81DFBDAB7C52067B9DA7A5E686A267A75AC82F24C74";
        obj.cufd = "BQUE+QytqQUDBKVUFOSVRPQkxVRFZNVFVJBMDAwMDAwM";
        obj.codigoSucursal = 0;
        obj.direccion = "AV.JORGE LOPEZ #123";
        obj.codigoPuntoVenta = 0;
        obj.fechaEmision = "2021-10-07T09:55:46.414";
        obj.nombreRazonSocial = "Mi razon social";
        obj.codigoTipoDocumentoIdentidad = 1;
        obj.numeroDocumento = "5115889";
        obj.complemento = null;
        obj.codigoCliente = "51158891";
        obj.codigoMetodoPago = 1;
        obj.montoTotal = 99;
        obj.montoTotalSujetoIva = 99;
        obj.codigoMoneda = 1;
        obj.tipoCambio = 1;
        obj.montoTotalMoneda = 99;
        obj.cafc = null;
        obj.leyenda = "Ley N° 453: Tienes derecho a recibir información sobre las características y contenidos de los servicios que utilices.";
        obj.usuario = "pperez";
        obj.codigoDocumentoSector = 22;
        obj.detalles = listaDetalles;
        return obj;
    }
    //llenado de datos ficticios en las cabeceras de del xml de telecom y com-ven
    public CabeceraFacturaTelecomunicaciones LlenarCabeceraFacturaTelecomunicacionesXml (FacturaTelecomunicaciones factura)
    {
        CabeceraFacturaTelecomunicaciones cabecera = new CabeceraFacturaTelecomunicaciones();
        cabecera.nitEmisor = factura.nitEmisor;
        cabecera.razonSocialEmisor = factura.razonSocialEmisor;
        cabecera.municipio = factura.municipio;
        cabecera.telefono = factura.telefono;
        cabecera.nitConjunto = factura.nitConjunto;
        cabecera.numeroFactura = factura.numeroFactura;
        cabecera.cuf = factura.cuf;
        cabecera.cufd = factura.cufd;
        cabecera.codigoSucursal = factura.codigoSucursal;
        cabecera.direccion = factura.direccion;
        cabecera.codigoPuntoVenta = factura.codigoPuntoVenta;
        cabecera.fechaEmision = factura.fechaEmision;
        cabecera.nombreRazonSocial = factura.nombreRazonSocial;
        cabecera.codigoTipoDocumentoIdentidad = factura.codigoTipoDocumentoIdentidad;
        cabecera.numeroDocumento = factura.numeroDocumento;
        cabecera.complemento = factura.complemento;
        cabecera.codigoCliente = factura.codigoCliente;
        cabecera.codigoMetodoPago = factura.codigoMetodoPago;
        cabecera.numeroTarjeta = null;
        cabecera.montoTotal = factura.montoTotal;
        cabecera.montoTotalSujetoIva = factura.montoTotalSujetoIva;
        cabecera.codigoMoneda = factura.codigoMoneda;
        cabecera.tipoCambio = factura.tipoCambio;
        cabecera.montoTotalMoneda = factura.montoTotalMoneda;
        cabecera.montoGiftCard = null;
        cabecera.descuentoAdicional = factura.descuentoAdicional;
        cabecera.codigoExcepcion = null;
        cabecera.cafc = factura.cafc;
        cabecera.leyenda = factura.leyenda;
        cabecera.usuario = factura.usuario;
        cabecera.codigoDocumentoSector = factura.codigoDocumentoSector;
        return cabecera;
    }
    public CabeceraFacturaCompraVenta LlenarCabeceraFacturaCompraVentaXml(FacturaCompraVenta factura)
    {
        CabeceraFacturaCompraVenta cabecera = new CabeceraFacturaCompraVenta();
        cabecera.nitEmisor = factura.nitEmisor;
        cabecera.razonSocialEmisor = factura.razonSocialEmisor;
        cabecera.municipio = factura.municipio;
        cabecera.telefono = factura.telefono;
        cabecera.numeroFactura = factura.numeroFactura;
        cabecera.cuf = factura.cuf;
        cabecera.cufd = factura.cufd;
        cabecera.codigoSucursal = factura.codigoSucursal;
        cabecera.direccion = factura.direccion;
        cabecera.codigoPuntoVenta = factura.codigoPuntoVenta;
        cabecera.fechaEmision = factura.fechaEmision;
        cabecera.nombreRazonSocial = factura.nombreRazonSocial;
        cabecera.codigoTipoDocumentoIdentidad = factura.codigoTipoDocumentoIdentidad;
        cabecera.numeroDocumento = factura.numeroDocumento;
        cabecera.complemento = factura.complemento;
        cabecera.codigoCliente = factura.codigoCliente;
        cabecera.codigoMetodoPago = factura.codigoMetodoPago;
        cabecera.numeroTarjeta = null;
        cabecera.montoTotal = factura.montoTotal;
        cabecera.montoTotalSujetoIva = factura.montoTotalSujetoIva;
        cabecera.codigoMoneda = factura.codigoMoneda;
        cabecera.tipoCambio = factura.tipoCambio;
        cabecera.montoTotalMoneda = factura.montoTotalMoneda;
        cabecera.montoGiftCard = null;
        cabecera.descuentoAdicional = factura.descuentoAdicional;
        cabecera.codigoExcepcion = null;
        cabecera.cafc = factura.cafc;
        cabecera.leyenda = factura.leyenda;
        cabecera.usuario = factura.usuario;
        cabecera.codigoDocumentoSector = factura.codigoDocumentoSector;
        return cabecera;
    }
    //llenado de datos ficticios en los detalles de del xml de telecom y com-ven
    public List<DetalleFacturaGralXml> LlenarDetalleFacturaTelecomunicacionesXml(List<FacturaDetalleGral> detalles)
    {
        List<DetalleFacturaGralXml> listaDetalles = new List<DetalleFacturaGralXml>();
        foreach (var factura in detalles)
        {
            if (factura.montoDescuento == null)
            {
                factura.montoDescuento = 0;
            }
            DetalleFacturaGralXml detalle = new DetalleFacturaGralXml();
            detalle.actividadEconomica = factura.actividadEconomica;
            detalle.codigoProductoSin = factura.codigoProductoSin;
            detalle.codigoProducto = factura.codigoProducto;
            detalle.descripcion = factura.descripcion;
            detalle.cantidad = factura.cantidad;
            detalle.unidadMedida = factura.unidadMedida;
            detalle.precioUnitario = factura.precioUnitario;
            detalle.montoDescuento = factura.montoDescuento;
            detalle.subTotal = factura.subTotal;
            detalle.numeroSerie = null;
            detalle.numeroImei = null;
            listaDetalles.Add(detalle);
        }
        return listaDetalles;
    }
    public List<DetalleFacturaGralXml> LlenarDetalleFacturaCompraVentaXml(List<FacturaDetalleGral> detalles)
    {
        List<DetalleFacturaGralXml> listaDetalles = new List<DetalleFacturaGralXml>();
        foreach (var factura in detalles)
        {
            if (factura.montoDescuento == null)
            {
                factura.montoDescuento = 0;
            }
            DetalleFacturaGralXml detalle = new DetalleFacturaGralXml();
            detalle.actividadEconomica = factura.actividadEconomica;
            detalle.codigoProductoSin = factura.codigoProductoSin;
            detalle.codigoProducto = factura.codigoProducto;
            detalle.descripcion = factura.descripcion;
            detalle.cantidad = factura.cantidad;
            detalle.unidadMedida = factura.unidadMedida;
            detalle.precioUnitario = factura.precioUnitario;
            detalle.montoDescuento = factura.montoDescuento;
            detalle.subTotal = factura.subTotal;
            detalle.numeroSerie = null;
            detalle.numeroImei = null;
            listaDetalles.Add(detalle);
        }
        
        return listaDetalles;
    }
    //generando las facturas en formato xml de telecom y com-ven
    public void GenerarFacturaTelecomunicacionesXml(FacturaTelecomunicaciones factura, string pathArchivo)
    {
        FacturaTelecomunicacionesXml obj=new FacturaTelecomunicacionesXml();
        List<DetalleFacturaGralXml> detalles = new List<DetalleFacturaGralXml>();
        obj.cabecera= LlenarCabeceraFacturaTelecomunicacionesXml(factura);
        detalles= LlenarDetalleFacturaTelecomunicacionesXml(factura.detalles);
        SerializarObjeto(obj,1,pathArchivo,detalles);
    }
    public void GenerarFacturaCompraVentaXml(FacturaCompraVenta factura, string pathArchivo)
    {
        FacturaCompraVentaXml obj = new FacturaCompraVentaXml();
        List<DetalleFacturaGralXml> detalles = new List<DetalleFacturaGralXml>();
        obj.cabecera = LlenarCabeceraFacturaCompraVentaXml(factura);
        detalles = LlenarDetalleFacturaCompraVentaXml(factura.detalles);
        SerializarObjeto(obj,2,pathArchivo,detalles);
    }

    public void GenerarRegistroCompraXml(RegistroCompra registroCompra, string pathArchivo)
    {
        RegistroCompraXml obj = new RegistroCompraXml();
        obj.codigoAutorizacion = registroCompra.codigoAutorizacion;
        obj.codigoControl = registroCompra.codigoControl;
        obj.creditoFiscal = registroCompra.creditoFiscal;
        obj.descuento = registroCompra.descuento;
        obj.fechaEmision = registroCompra.fechaEmision;
        obj.importeIce = registroCompra.importeIce;
        obj.importeIehd = registroCompra.importeIehd;
        obj.importeIpj = registroCompra.importeIpj;
        obj.importesExentos = registroCompra.importesExentos;
        obj.importeTasaCero = registroCompra.importeTasaCero;
        obj.montoGiftCard = registroCompra.montoGiftCard;
        obj.montoTotalCompra = registroCompra.montoTotalCompra;
        obj.montoTotalSujetoIva = registroCompra.montoTotalSujetoIva;
        obj.nitEmisor = registroCompra.nitEmisor;
        obj.nro = registroCompra.nro;
        obj.numeroDuiDim = registroCompra.numeroDuiDim;
        obj.numeroFactura = registroCompra.numeroFactura;
        obj.otroNoSujetoCredito = registroCompra.otroNoSujetoCredito;
        obj.razonSocialEmisor = registroCompra.razonSocialEmisor;
        obj.subTotal = registroCompra.subTotal;
        obj.tasas = registroCompra.tasas;
        obj.tipoCompra = registroCompra.tipoCompra;

        SerializarObjeto(obj,3,pathArchivo,null);
    }
}