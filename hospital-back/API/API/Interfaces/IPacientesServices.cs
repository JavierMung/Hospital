using API.ViewModels;

namespace API.Interfaces
{
	public interface IPacientesServices
	{
		public Task<List<ViewPaciente?>?> GetPacientes();
		public Task<ViewPaciente?> UpdateCita(ViewPaciente paciente);
		public Task<ViewPaciente?> DeletePaciente(int id);
		public Task<ViewPaciente?> GetPacienteById(int id);
		public Task<ViewPaciente?> CreatePaciente(ViewPaciente paciente);
	}
}
