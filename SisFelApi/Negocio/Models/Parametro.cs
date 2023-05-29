using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Parametro
{
    public int Codigoparametro { get; set; }

    public string? Nombreparametro { get; set; }

    public string Nombregrupo { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Evento> Eventos { get; } = new List<Evento>();

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
