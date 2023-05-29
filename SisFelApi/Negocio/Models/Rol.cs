using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Rol
{
    public int Codigorol { get; set; }

    public string Nombrerol { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Permiso> Permisos { get; } = new List<Permiso>();

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
