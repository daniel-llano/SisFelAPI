using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Puntoventum
{
    public int Codigopuntoventa { get; set; }

    public string Nombrepuntoventa { get; set; } = null!;

    public string Tipopuntoventa { get; set; } = null!;

    public int Codigosucursal { get; set; }

    public bool? Activo { get; set; }

    public virtual Sucursal CodigosucursalNavigation { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; } = new List<Factura>();

    public virtual ICollection<Usuariopuntoventum> Usuariopuntoventa { get; } = new List<Usuariopuntoventum>();
}
