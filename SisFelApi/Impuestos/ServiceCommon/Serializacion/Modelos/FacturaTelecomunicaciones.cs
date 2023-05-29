using SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos;
public class FacturaTelecomunicaciones
{
    public long nitEmisor { get; set; }
    public string razonSocialEmisor { get; set; }
    public string municipio { get; set; }
    public string? telefono { get; set; }
    public string nitConjunto { get; set; }
    public long numeroFactura { get; set; }
    public string cuf { get; set; }
    public string cufd { get; set; }
    public int codigoSucursal { get; set; }
    public string direccion { get; set; }
    public int? codigoPuntoVenta { get; set; }
    public string fechaEmision { get; set; }
    public string? nombreRazonSocial { get; set; }
    public int codigoTipoDocumentoIdentidad { get; set; }
    public string numeroDocumento { get; set; }
    public string? complemento { get; set; }
    public string codigoCliente { get; set; }
    public int codigoMetodoPago { get; set; }
    //public int numeroTarjeta;
    public float montoTotal { get; set; }
    public float montoTotalSujetoIva { get; set; }
    public int codigoMoneda { get; set; }
    public int tipoCambio { get; set; }
    public float montoTotalMoneda { get; set; }
    //public float montoGiftCard;
    public float descuentoAdicional;
    //public int codigoExcepcion;
    public string? cafc { get; set; }

    public string leyenda { get; set; }
    public string usuario { get; set; }
    public int codigoDocumentoSector { get; set; }
    public List<FacturaDetalleGral> detalles { get; set; } = new List<FacturaDetalleGral>();
}