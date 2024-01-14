using System;
using System.Collections.Generic;

namespace API.Context;

public partial class MedicosEspecialidad
{
    public string Especialidad { get; set; } = null!;

    public int? NumeroMedicos { get; set; }
}
