using System.Xml.Serialization;
public  class DetalleFacturaCompraVenta
    {
        //DETALLE
        public string actividadEconomica;
        public int codigoProductoSin;
        public string codigoProducto;
        public string descripcion;
        public decimal cantidad;
        public int unidadMedida;
        public float precioUnitario;
        [XmlElement(IsNullable = true)]
        public float? montoDescuento;
        public float subTotal;
        [XmlElement(IsNullable = true)]
        public string? numeroSerie;
        [XmlElement(IsNullable = true)]
        public string? numeroImei;
    }