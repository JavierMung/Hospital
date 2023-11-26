using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Horario
{
    public int IdHorario { get; set; }

    public string Turno { get; set; } = null!;

    public DateTime HoraInicio { get; set; }

    public DateTime HoraFin { get; set; }

    public virtual ICollection<Trabajador> Trabajadors { get; } = new List<Trabajador>();
}
