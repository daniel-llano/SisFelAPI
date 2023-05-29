using System.Xml.Serialization;
using System.Xml.Schema;
    [XmlRoot("facturaElectronicaCompraVenta")]
    [XmlType("NewTypeName")]
    public class FacturaCompraVentaXml
    {
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string attr = "facturaElectronicaCompraVenta.xsd";
        public CabeceraFacturaCompraVenta cabecera;
    }