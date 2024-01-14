using System;
using System.Collections.Generic;

namespace API.Context;

public partial class TrabajadorRol
{
    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string Rol { get; set; } = null!;
}
