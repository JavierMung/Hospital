using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Status
{
    public int IdStatus { get; set; }

    public string Status1 { get; set; } = null!;

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
