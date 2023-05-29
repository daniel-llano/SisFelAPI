namespace SisFelApi.Negocio.DTOs
{
    public class TelefonoClienteDto
    {
        public string Codigotelefonocliente { get; set; }

        public string Codigocliente { get; set; } = null!;

        public int Codigotipodocumentoidentidad { get; set; }

        public string Nit { get; set; }

        public string Ci { get; set; }

        public string? Complemento { get; set; }

        public string Razonsocial { get; set; } = null!;

        public string? Email { get; set; }

        public int Telefono { get; set; }

        public bool? Activo { get; set; }
    }
}
