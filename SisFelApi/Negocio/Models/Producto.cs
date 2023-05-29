using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Producto
{
    public string Codigoproducto { get; set; } = null!;

    public string Nombreproducto { get; set; } = null!;

    public char Tipoproducto { get; set; }

    public decimal Precio { get; set; }

    public int Codigounidadmedida { get; set; }

    public int Codigocategoria { get; set; }

    public bool? Activo { get; set; }

    public virtual Categorium CodigocategoriaNavigation { get; set; } = null!;
    
    public virtual Parametro CodigounidadmedidaNavigation { get; set; } = null!;

    public virtual ICollection<Facturadetalle> Facturadetalles { get; } = new List<Facturadetalle>();
}
