namespace SisFelApi.Negocio.DTOs
{
    public class UsuarioPuntoVentaDto
    {
        public string Nombreusuario { get; set; } = null!;

        public int Codigopuntoventa { get; set; }

        public bool? Activo { get; set; }
    }
}