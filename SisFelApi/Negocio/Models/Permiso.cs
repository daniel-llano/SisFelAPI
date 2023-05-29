using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Permiso
{
    public int Codigorol { get; set; }

    public int Codigoenlace { get; set; }

    public bool? Activo { get; set; }

    public virtual Enlace CodigoenlaceNavigation { get; set; } = null!;

    public virtual Rol CodigorolNavigation { get; set; } = null!;
}
