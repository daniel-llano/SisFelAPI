using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Sucursal
{
    public int Codigosucursal { get; set; }

    public string Nombresucursal { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Barrio { get; set; } = null!;

    public string Municipio { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Factura> Facturas { get; } = new List<Factura>();

    public virtual ICollection<Puntoventum> Puntoventa { get; } = new List<Puntoventum>();
}
