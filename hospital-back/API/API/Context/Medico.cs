using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Medico
{
    public int IdMedico { get; set; }

    public int IdTrabajador { get; set; }

    public string? Consultorio { get; set; }

    public string Especialidad { get; set; } = null!;

    public bool Consulta { get; set; }

    public string? Cedula { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual Trabajador IdTrabajadorNavigation { get; set; } = null!;
}
