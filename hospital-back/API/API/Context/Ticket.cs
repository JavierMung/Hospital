using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Ticket
{
    public int IdTicket { get; set; }

    public int IdTrabajador { get; set; }

    public double Total { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Trabajador IdTrabajadorNavigation { get; set; } = null!;

    public virtual ICollection<ServiciosTicket> ServiciosTickets { get; } = new List<ServiciosTicket>();

    public virtual ICollection<TicketsInsumo> TicketsInsumos { get; } = new List<TicketsInsumo>();
}
