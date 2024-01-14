using System;
using System.Collections.Generic;

namespace API.Context;

public partial class TrabajadorServicio1
{
    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string Servicio { get; set; } = null!;
}
