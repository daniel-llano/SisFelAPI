namespace SisFelApi.Negocio.DTOs
{
    public class UsuarioDto
    {
        public string Nombreusuario { get; set; } = null!;

        public string Ci { get; set; } = null!;

        public string Nombres { get; set; } = null!;

        public string? Ap { get; set; }

        public string? Am { get; set; }

        public string? Telefono { get; set; }

        public string Clave { get; set; } = null!;

        public int? Codigorol { get; set; }

        public bool? Activo { get; set; }
    }
}