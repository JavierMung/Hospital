using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Paciente
{
    public int IdPaciente { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public int Edad { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
