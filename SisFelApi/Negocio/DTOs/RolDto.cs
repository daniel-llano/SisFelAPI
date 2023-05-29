namespace SisFelApi.Negocio.DTOs
{
    public class RolDto
    {
        public int Codigorol { get; set; }

        public string Nombrerol { get; set; } = null!;

        public string? Descripcion { get; set; }

        public bool? Activo { get; set; }
    }
}