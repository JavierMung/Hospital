using System;
using System.Collections.Generic;

namespace API.Context;

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

    public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<TrabajadorServicio> TrabajadorServicios { get; set; } = new List<TrabajadorServicio>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
