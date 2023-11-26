using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class Trabajador
{
    public int IdTrabajador { get; set; }

    public int IdRol { get; set; }

    public int IdHorario { get; set; }

    public int IdPersona { get; set; }

    public DateTime FechaInicio { get; set; }

    public double Salario { get; set; }

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Medico> Medicos { get; } = new List<Medico>();

    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();

    public virtual ICollection<TrabajadorServicio> TrabajadorServicios { get; } = new List<TrabajadorServicio>();

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
