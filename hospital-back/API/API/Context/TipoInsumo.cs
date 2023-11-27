using System;
using System.Collections.Generic;

namespace API.Context;

public partial class TipoInsumo
{
    public int IdTipoInsumo { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();
}
