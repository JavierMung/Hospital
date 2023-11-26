using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class LoginLog
{
    public int IdUsuario { get; set; }

    public string? Mensaje { get; set; }

    public DateTime Fecha { get; set; }
}
