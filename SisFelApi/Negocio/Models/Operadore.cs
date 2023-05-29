using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Operadore
{
    public long Nit { get; set; }

    public string Nombre { get; set; } = null!;

    public string Sucursal { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Barriozona { get; set; }

    public string Telefono { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public bool? Activo { get; set; }
}
