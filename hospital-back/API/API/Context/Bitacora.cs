using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Bitacora
{
    public int IdBitacora { get; set; }

    public int IdRecetaMedica { get; set; }

    public int IdCita { get; set; }

    public DateTime Fecha { get; set; }

    public string Diagnostico { get; set; } = null!;

    public virtual Cita IdCitaNavigation { get; set; } = null!;

    public virtual RecetaMedica IdRecetaMedicaNavigation { get; set; } = null!;
}
