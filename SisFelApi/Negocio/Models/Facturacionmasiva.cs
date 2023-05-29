using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Facturacionmasiva
{
    public int Codigofacturacionmasiva { get; set; }

    public string Cufdmasivo { get; set; } = null!;

    public DateTime Fechainicio { get; set; }

    public DateTime Fechafin { get; set; }

    public int Numerofacturasenviadas { get; set; }

    public int Numerofacturainicio { get; set; }

    public int Numerofacturafin { get; set; }

    public int Estado { get; set; }

    public string? Codigorecepcion { get; set; }
}
