using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Factura
{
    public int Codigofactura { get; set; }

    public string? Codigorecepcion { get; set; }

    public long? Nitemisor { get; set; }

    public string? Municipio { get; set; }

    public int? Telefonoemisor { get; set; }

    public string? Nitconjunto { get; set; }

    public long Numerofactura { get; set; }

    public string Cuf { get; set; } = null!;

    public string Cufd { get; set; } = null!;

    public int Codigosucursal { get; set; }

    public string Direccion { get; set; } = null!;

    public int Codigopuntoventa { get; set; }

    public string? Cafc { get; set; }

    public DateTime Fechaemision { get; set; }

    public string Nombrerazonsocial { get; set; } = null!;

    public int Codigotipodocumentoidentidad { get; set; }

    public string Numerodocumento { get; set; } = null!;

    public string? Complemento { get; set; }

    public string Codigotelefonocliente { get; set; }

    public int Codigometodopago { get; set; }

    public long? Nrotarjeta { get; set; }

    public decimal Montototal { get; set; }

    public decimal Montototalsujetoiva { get; set; }

    public int Codigomoneda { get; set; }

    public string Leyenda { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public int Codigodocumentosector { get; set; }

    public string EstadoFactura { get; set; } = null!;

    public decimal Descuentoadicional { get; set; }

    public virtual ICollection<Auditoriafactura> Auditoriafacturas { get; set; } = new List<Auditoriafactura>();

    public virtual Puntoventum CodigopuntoventaNavigation { get; set; } = null!;

    public virtual Sucursal CodigosucursalNavigation { get; set; } = null!;

    public virtual Telefonocliente CodigotelefonoclienteNavigation { get; set; } = null!;

    public virtual ICollection<Facturadetalle> Facturadetalles { get; set; } = new List<Facturadetalle>();

    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
