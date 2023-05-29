using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Categorium
{
    public int Codigocategoria { get; set; }

    public int Codigoactividad { get; set; }

    public int Codigoproductosin { get; set; }

    public string? Descripcionproducto { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
