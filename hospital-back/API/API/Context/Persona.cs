using System;
using System.Collections.Generic;

namespace API.Context;

public partial class Persona
{
    public int IdPersona { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string? Calle { get; set; }

    public string? Colonia { get; set; }

    public string? Municipio { get; set; }

    public string? Estado { get; set; }

    public string? Cp { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<Trabajador> Trabajadors { get; set; } = new List<Trabajador>();
}
