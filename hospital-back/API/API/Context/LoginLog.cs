using System;
using System.Collections.Generic;

namespace API.Context;

public partial class LoginLog
{
    public int IdUsuario { get; set; }

    public string? Mensaje { get; set; }

    public DateTime Fecha { get; set; }
}
