using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Role
{
    public int IdRol { get; set; }

    public string Rol { get; set; } = null!;

    public virtual ICollection<Trabajador> Trabajadors { get; set; } = new List<Trabajador>();
}
