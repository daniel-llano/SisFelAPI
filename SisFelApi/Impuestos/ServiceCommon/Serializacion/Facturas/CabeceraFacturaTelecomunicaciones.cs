using System.Xml.Serialization;
 public  class CabeceraFacturaTelecomunicaciones
    {
        //(comentarios para el mapeo de objetos del txt)
        public long nitEmisor; //NITEMI
        public string razonSocialEmisor; //COOPERATIVA DE SERVICIOS PUBLICOS DE TELECOMUNICACIONES DE TARIJA R.L. COSETT R.
        public string municipio; //Tarija
        [XmlElement(IsNullable = true)]
        public string? telefono; //2846005
        [XmlElement(IsNullable = true)]
        public string? nitConjunto;//null
        public long numeroFactura; //NROFAC
        public string cuf; //""
        public string cufd; //""
        public int codigoSucursal; //0
        public string direccion; //AV.JORGE LOPEZ #123
        [XmlElement(IsNullable = true)]
        public int? codigoPuntoVenta; //null
        public string fechaEmision; //FECEMI
        public string nombreRazonSocial; //NOMCLI
        public int codigoTipoDocumentoIdentidad; //1
        public string numeroDocumento; //NITCLI
        [XmlElement(IsNullable = true)]
        public string? complemento; //null
        public string codigoCliente; //TELCOD
        public int codigoMetodoPago; //1
        [XmlElement(IsNullable = true)]
        public int? numeroTarjeta; //null-
        public float montoTotal; //TOTFAC
        public float montoTotalSujetoIva; //TASA
        public int codigoMoneda;//1
        public int tipoCambio;//1
        public float montoTotalMoneda; //TOTFAC
        [XmlElement(IsNullable = true)]
        public float? montoGiftCard; //null-
        [XmlElement(IsNullable = true)]
        public float? descuentoAdicional; //null-
        [XmlElement(IsNullable = true)]
        public int? codigoExcepcion; //null-
        [XmlElement(IsNullable = true)]
        public string? cafc; //null
        public string leyenda; //""
        public string usuario; //""
        public int codigoDocumentoSector; //22
    }