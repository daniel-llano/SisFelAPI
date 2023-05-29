namespace SisFelApi.Negocio.DTOs
{
    public class ProductoDto
    {
        public string Codigoproducto { get; set; } = null!;

        public string Nombreproducto { get; set; } = null!;

        public char Tipoproducto { get; set; }

        public double Precio { get; set; }

        public int Codigounidadmedida { get; set; }

        public int Codigocategoria { get; set; }

        public bool? Activo { get; set; }
    }
}
