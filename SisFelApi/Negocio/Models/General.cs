using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class General
{
    public int Codigopuntoventa { get; set; }

    public string? Codigosistema { get; set; }

    public string? Cuis { get; set; }

    public DateOnly? Fechavigenciacuis { get; set; }

    public string? Cufd { get; set; }

    public DateOnly? Fechavigenciacufd { get; set; }

    public string? Codigocontrol { get; set; }

    public string? Nit { get; set; }

    public string? Nombreempresa { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Ciudad { get; set; }

    public string? Codigoautorizacion { get; set; }
}
