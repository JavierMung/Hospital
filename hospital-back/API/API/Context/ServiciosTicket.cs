using System;
using System.Collections.Generic;

namespace API.Context;

public partial class ServiciosTicket
{
    public int IdServicioTicket { get; set; }

    public int IdServicio { get; set; }

    public int IdTicket { get; set; }

    public int Cantidad { get; set; }

    public double PreTotal { get; set; }

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual Ticket IdTicketNavigation { get; set; } = null!;
}
