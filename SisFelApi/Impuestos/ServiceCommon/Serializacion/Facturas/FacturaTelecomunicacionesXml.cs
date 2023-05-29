using System.Xml.Serialization;
using System.Xml.Schema;
    [XmlRoot("facturaElectronicaTelecomunicacion")]
    [XmlType("NewTypeName")]
    public class FacturaTelecomunicacionesXml
    {
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string attr = "facturaElectronicaTelecomunicacion.xsd";
        public CabeceraFacturaTelecomunicaciones cabecera;
    }