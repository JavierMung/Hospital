using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Cita
{
    public int IdCita { get; set; }

    public int IdMedico { get; set; }

    public int IdPaciente { get; set; }

    public int IdServicio { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<Bitacora> Bitacoras { get; } = new List<Bitacora>();

    public virtual Medico IdMedicoNavigation { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual ICollection<RecetaMedica> RecetaMedicas { get; } = new List<RecetaMedica>();
}
