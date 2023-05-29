using SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos;

public class FacturaCompraVenta
    {
        public long nitEmisor;
        public string razonSocialEmisor;
        public string municipio;
        public string? telefono;
        public long numeroFactura;
        public string cuf;
        public string cufd;
        public int codigoSucursal;
        public string direccion;
        public int? codigoPuntoVenta;
        public string fechaEmision;
        public string? nombreRazonSocial;
        public int codigoTipoDocumentoIdentidad;
        public string numeroDocumento;
        public string? complemento;
        public string codigoCliente;
        public int codigoMetodoPago;
        //public int numeroTarjeta;
        public float montoTotal;
        public float montoTotalSujetoIva;
        public int codigoMoneda;
        public float tipoCambio;
        public float montoTotalMoneda;
        //public float montoGiftCard;
        public float descuentoAdicional;
        public int? codigoExcepcion;
        public string? cafc;
        public string leyenda;
        public string usuario;
        public int codigoDocumentoSector;
        public List<FacturaDetalleGral> detalles { get; set; } = new List<FacturaDetalleGral>();
}