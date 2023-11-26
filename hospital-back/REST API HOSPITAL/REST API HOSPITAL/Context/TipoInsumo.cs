using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class TipoInsumo
{
    public int IdTipoInsumo { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Insumo> Insumos { get; } = new List<Insumo>();
}
