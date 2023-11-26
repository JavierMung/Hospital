using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public string Servicio1 { get; set; } = null!;

    public double Costo { get; set; }

    public virtual ICollection<Cita> Cita { get; } = new List<Cita>();

    public virtual ICollection<ServiciosTicket> ServiciosTickets { get; } = new List<ServiciosTicket>();

    public virtual ICollection<TrabajadorServicio> TrabajadorServicios { get; } = new List<TrabajadorServicio>();
}
