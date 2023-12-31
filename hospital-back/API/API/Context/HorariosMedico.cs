using System;
using System.Collections.Generic;

namespace API.Context;

public partial class HorariosMedico
{
    public int IdMedico { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string Turno { get; set; } = null!;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }
}
