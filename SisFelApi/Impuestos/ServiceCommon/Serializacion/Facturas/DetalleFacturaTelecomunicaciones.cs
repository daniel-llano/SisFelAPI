using System.Xml.Serialization;
public  class DetalleFacturaTelecomunicaciones
    {
        //DETALLE (comentarios para el mapeo de objetos del txt)
        public string actividadEconomica; //pruebas 610000
        public int codigoProductoSin;// pruebas 84120
        public string codigoProducto;// CODSER
        public string descripcion;//DESCRIP     
        public decimal cantidad; //CANTIDAD 
        public int unidadMedida; //UNIDAD
        public float precioUnitario;//PREUNI
        [XmlElement(IsNullable = true)]
        public float? montoDescuento; //null
        public float subTotal;//PRETOT
        [XmlElement(IsNullable = true)]
        public string? numeroSerie; //null
        [XmlElement(IsNullable = true)]
        public string? numeroImei; //null
    }