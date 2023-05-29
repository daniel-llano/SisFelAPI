namespace SisFelApi.Negocio.DTOs
{
    public class PuntoVentaDto
    {
        public int Codigopuntoventa { get; set; }

        public string Nombrepuntoventa { get; set; } = null!;

        public string Tipopuntoventa { get; set; } = null!;

        public int Codigosucursal { get; set; }

        public bool? Activo { get; set; }
    }
}
