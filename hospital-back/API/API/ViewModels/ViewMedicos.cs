namespace API.ViewModels
{

	public record ViewMedicos(int IdMedico,int IdTrabajador ,string Nombre, string Especialidad, string Consultorio, string Cedula ,string Status, bool Consulta ,TimeSpan HorarInicio, TimeSpan HoraFin);
	public record ViewMedicosUpdate(int IdMedico, string Nombre, string Especialidad, string Consultorio, string Status, bool Consulta);
	public record ViewMedicoAdd(int IdTrabajador, string Consultorio, string Especialidad, bool Consulta, string Cedula, string Status);
}
