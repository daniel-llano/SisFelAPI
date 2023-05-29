using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Evento
{
    public int Codigoevento { get; set; }

    public int Codigomotivoevento { get; set; }

    public long Codigorecepcionevento { get; set; }

    public int Codigopuntoventa { get; set; }

    public string? Cafccompraventa { get; set; }

    public string? Cafctelecom { get; set; }

    public string Cuis { get; set; } = null!;

    public string? Cufd { get; set; }

    public string Cufdevento { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime Fechahorainicioevento { get; set; }

    public DateTime Fechahorafinevento { get; set; }

    public bool? Activo { get; set; }

    public virtual Parametro CodigomotivoeventoNavigation { get; set; } 
}
