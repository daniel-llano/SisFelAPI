using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Enlace
{
    public int Codigoenlace { get; set; }

    public string Nombreenlace { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Permiso> Permisos { get; } = new List<Permiso>();
}
