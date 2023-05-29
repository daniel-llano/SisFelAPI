using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Usuario
{
    public string Nombreusuario { get; set; } = null!;

    public string Ci { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string? Ap { get; set; }

    public string? Am { get; set; }

    public string? Telefono { get; set; }

    public string Clave { get; set; } = null!;

    public int Codigorol { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Auditoriafactura> Auditoriafacturas { get; } = new List<Auditoriafactura>();

    public virtual Rol CodigorolNavigation { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; } = new List<Factura>();

    public virtual ICollection<Usuariopuntoventum> Usuariopuntoventa { get; } = new List<Usuariopuntoventum>();
}
