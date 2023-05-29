namespace SisFelApi.Negocio.DTOs
{
    public class EnlaceDto
    {
        public int Codigoenlace { get; set; }

        public string Nombreenlace { get; set; } = null!;

        public string Ruta { get; set; } = null!;

        public bool? Activo { get; set; }
    }
}