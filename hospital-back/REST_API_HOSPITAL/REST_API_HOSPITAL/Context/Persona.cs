using System;
using System.Collections.Generic;

namespace REST_API_HOSPITAL.Context;

public partial class Persona
{
    public int IdPersona { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Calle { get; set; } = null!;

    public string Colonia { get; set; } = null!;

    public string Municipio { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public int Cp { get; set; }

    public int? Telefono { get; set; }

    public virtual ICollection<Trabajador> Trabajadors { get; } = new List<Trabajador>();
}
