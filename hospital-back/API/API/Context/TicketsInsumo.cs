using System;
using System.Collections.Generic;

namespace API.Context;

public partial class TicketsInsumo
{
    public int IdTicketInsumo { get; set; }

    public int IdTicket { get; set; }

    public int IdInsumo { get; set; }

    public int Cantidad { get; set; }

    public double PreTotal { get; set; }

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual Ticket IdTicketNavigation { get; set; } = null!;
}
