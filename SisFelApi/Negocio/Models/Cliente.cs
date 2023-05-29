using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Cliente
{
    public string Codigocliente { get; set; } = null!;

    public string Ci { get; set; }

    public string? Tipopersona { get; set; }

    public string? Datoscliente { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Historialtelefonico> Historialtelefonicos { get; } = new List<Historialtelefonico>();

    public virtual ICollection<Telefonocliente> Telefonoclientes { get; } = new List<Telefonocliente>();
}
