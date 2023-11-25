using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class RecetaMedicamento
{
    public int IdRecetaMedicamento { get; set; }

    public int IdInsumo { get; set; }

    public int IdRecetaMedica { get; set; }

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual RecetaMedica IdRecetaMedicaNavigation { get; set; } = null!;
}
