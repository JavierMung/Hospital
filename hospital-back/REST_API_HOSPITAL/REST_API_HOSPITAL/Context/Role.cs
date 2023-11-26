using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class Role
{
    public int IdRol { get; set; }

    public string Rol { get; set; } = null!;

    public virtual ICollection<Trabajador> Trabajadors { get; } = new List<Trabajador>();
}
