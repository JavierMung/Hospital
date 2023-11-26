using System;
using System.Collections.Generic;

namespace API.Context;

public partial class RecuperacionContrasena
{
    public int IdRecuperacionContrasena { get; set; }

    public int IdUsuario { get; set; }

    public string TokenRecuperacion { get; set; } = null!;

    public DateTime FechaExpiracion { get; set; }

    public bool Utilizado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
