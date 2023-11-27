using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Insumo
{
    public int IdInsumo { get; set; }

    public int IdTipoInsumo { get; set; }

    public int? Stock { get; set; }

    public double Precio { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual TipoInsumo IdTipoInsumoNavigation { get; set; } = null!;

    public virtual ICollection<RecetaMedicamento> RecetaMedicamentos { get; set; } = new List<RecetaMedicamento>();

    public virtual ICollection<TicketsInsumo> TicketsInsumos { get; set; } = new List<TicketsInsumo>();
}
