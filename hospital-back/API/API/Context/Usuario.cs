using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdTrabajador { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual Trabajador IdTrabajadorNavigation { get; set; } = null!;

    public virtual ICollection<RecuperacionContrasena> RecuperacionContrasenas { get; set; } = new List<RecuperacionContrasena>();
}
