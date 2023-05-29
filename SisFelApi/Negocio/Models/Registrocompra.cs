using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Registrocompra
{
    public int Codigocompra { get; set; }

    public int Codigopaqueterecepcioncompra { get; set; }

    public int Nrocompra { get; set; }

    public long? Nitemisor { get; set; }

    public string? Razonsocialemisor { get; set; }

    public string? Codigoautorizacion { get; set; }

    public string? Numerofactura { get; set; }

    public string? Numeroduidim { get; set; }

    public DateTime? Fechaemision { get; set; }

    public decimal? Montototalcompra { get; set; }

    public decimal? Importeice { get; set; }

    public decimal? Importeiehd { get; set; }

    public decimal? Importeipj { get; set; }

    public decimal? Tasas { get; set; }

    public decimal? Otronosujetocredito { get; set; }

    public decimal? Importesexentos { get; set; }

    public decimal? Importetasacero { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Descuento { get; set; }

    public decimal? Montogiftcard { get; set; }

    public decimal? Montototalsujetoiva { get; set; }

    public decimal? Creditofiscal { get; set; }

    public string? Tipocompra { get; set; }

    public string? Codigocontrol { get; set; }

    public int? Estado { get; set; }

    public virtual Paqueterecepcioncompra CodigopaqueterecepcioncompraNavigation { get; set; } = null!;
}
