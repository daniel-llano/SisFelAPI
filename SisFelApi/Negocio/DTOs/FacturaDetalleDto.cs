namespace SisFelApi.Negocio.DTOs
{
    public class FacturaDetalleDto
    {
        public int Codigofacturadetalle { get; set; }

        public int Codigofactura { get; set; }

        public string Codigoproducto { get; set; } = null!;

        public string Actividadeconomica { get; set; } = null!;

        public int Codigoproductosin { get; set; }

        public string Descripcion { get; set; } = null!;

        public decimal Cantidad { get; set; }

        public int Unidadmedida { get; set; }

        public decimal Preciounitario { get; set; }

        public decimal Montodescuento { get; set; }

        public decimal Subtotal { get; set; }

        public string Cuenta { get; set; } = null!;

        public string? Numeroserie { get; set; }

        public string? Numeroimei { get; set; }

        public string? Codigogrupo { get; set; }
    }
}
