namespace SisFelApi.Impuestos.ServiceCommon.Armado_de_Objetos.modelos
{
    public class DetalleTxt
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
        public string Cuenta { get; set; }
        public virtual FacturaTxt factura { get; set; } = null!;
    }
}
