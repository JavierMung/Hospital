namespace API.ViewModels
{

		public record ViewMedicos (int idMedico, string Nombre, string Especialidad, string Consultorio ,string Status, TimeSpan HorarInicio, TimeSpan HoraFin  );
		public record ViewMedicosAdd (int idMedico, string Nombre, string Especialidad, string Consultorio, string Status);
		public record ViewMedicoId(int idMedico );
}
