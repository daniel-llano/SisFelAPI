namespace SisFelApi.Negocio.DTOs
{
    public class SucursalDto
    {
        public int Codigosucursal { get; set; }

        public string Nombresucursal { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Barrio { get; set; } = null!;

        public string Municipio { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        public bool? Activo { get; set; }
    }
}
