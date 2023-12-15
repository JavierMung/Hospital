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

    public int IdStatus { get; set; }

    public virtual ICollection<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();

    public virtual Medico IdMedicoNavigation { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual ICollection<RecetaMedica> RecetaMedicas { get; set; } = new List<RecetaMedica>();
}
