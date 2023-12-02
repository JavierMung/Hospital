using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Horario
{
    public int IdHorario { get; set; }

    public string Turno { get; set; } = null!;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public virtual ICollection<Trabajador> Trabajadors { get; set; } = new List<Trabajador>();
}
