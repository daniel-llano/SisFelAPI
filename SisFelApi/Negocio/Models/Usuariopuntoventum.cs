using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Usuariopuntoventum
{
    public string Nombreusuario { get; set; } = null!;

    public int Codigopuntoventa { get; set; }

    public bool? Activo { get; set; }

    public virtual Puntoventum CodigopuntoventaNavigation { get; set; } = null!;

    public virtual Usuario NombreusuarioNavigation { get; set; } = null!;
}
