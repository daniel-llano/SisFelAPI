using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Auditoriafactura
{
    public int Codigomovimiento { get; set; }

    public int Codigofactura { get; set; }

    public string Usuario { get; set; } = null!;

    public DateOnly? Fecha { get; set; }

    public virtual Factura CodigofacturaNavigation { get; set; } = null!;

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
