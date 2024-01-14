using API.ViewModels;

namespace API.Interfaces
{
	public interface IPacientesServices
	{
		public Task<Result<List<ViewPaciente>>> GetPacientes();
		public Task<Result<ViewPaciente>> UpdateCita(ViewPaciente paciente);
		public Task<Result<ViewPaciente>> DeletePaciente(int id);
		public Task<Result<ViewPaciente>> GetPacienteById(int id);
		public Task<Result<ViewPaciente>> CreatePaciente(ViewPaciente paciente);
	}
}
