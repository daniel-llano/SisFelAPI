namespace SisFelApi.Negocio.DTOs
{
    public class ClienteDto
    {
        public string Codigocliente { get; set; } = null!;

        public string Ci { get; set; }

        public string? Tipopersona { get; set; }

        public string? Datoscliente { get; set; }

        public bool? Activo { get; set; }
    }
}
