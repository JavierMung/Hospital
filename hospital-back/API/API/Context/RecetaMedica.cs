using System;
using System.Collections.Generic;

namespace API.Context;

public partial class RecetaMedica
{
    public int IdRecetaMedica { get; set; }

    public int IdCita { get; set; }

    public string? Posologia { get; set; }

    public virtual ICollection<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();

    public virtual Cita IdCitaNavigation { get; set; } = null!;

    public virtual ICollection<RecetaMedicamento> RecetaMedicamentos { get; set; } = new List<RecetaMedicamento>();
}
