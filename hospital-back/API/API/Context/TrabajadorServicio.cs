using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class TrabajadorServicio
{
    public int IdTrabajadorServicio { get; set; }

    public int IdTrabajador { get; set; }

    public int IdServicio { get; set; }

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual Trabajador IdTrabajadorNavigation { get; set; } = null!;
}
