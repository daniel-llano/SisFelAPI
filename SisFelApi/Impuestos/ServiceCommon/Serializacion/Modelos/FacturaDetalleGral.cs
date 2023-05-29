namespace SisFelApi.Impuestos.ServiceCommon.Serializacion.Modelos
{
    public class FacturaDetalleGral
    {
        public string actividadEconomica { get; set; }
        public int codigoProductoSin { get; set; }
        public string codigoProducto { get; set; }
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public int unidadMedida { get; set; }
        public float precioUnitario { get; set; }
        public float? montoDescuento { get; set; }
        public float subTotal { get; set; }
        public virtual FacturaTelecomunicaciones factura { get; set; } = null!;
    }
}
