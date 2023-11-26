﻿using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class ServiciosTicket
{
    public int IdServicioTicket { get; set; }

    public int IdServicio { get; set; }

    public int IdTicket { get; set; }

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual Ticket IdTicketNavigation { get; set; } = null!;
}
