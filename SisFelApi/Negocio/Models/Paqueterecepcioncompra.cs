using System;
using System.Collections.Generic;

namespace SisFelApi.Negocio.Models;

public partial class Paqueterecepcioncompra
{
    public int Codigopaqueterecepcioncompra { get; set; }

    public string? Codigorecepcionpaquete { get; set; }

    public int? Cantidadfacturas { get; set; }

    public int Numerofacturainicio { get; set; }

    public int Numerofacturafin { get; set; }

    public DateTime? Fechaenvio { get; set; }

    public short? Gestion { get; set; }

    public short? Periodo { get; set; }

    public short? Estado { get; set; }

    public virtual ICollection<Registrocompra> Registrocompras { get; } = new List<Registrocompra>();
}
