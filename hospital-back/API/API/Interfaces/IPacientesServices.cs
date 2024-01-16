using API.ViewModels;

namespace API.Interfaces
{
	public interface IPacientesServices
	{
		public Task<Result<List<ViewPaciente>>> GetPacientes();
		public Task<Result<ViewPaciente>> UpdatePacienteByCURP(ViewPaciente paciente);
		public Task<Result<ViewPaciente>> DeletePaciente(int id);
		public Task<Result<ViewPaciente>> GetPacienteByCURP(string CURP);
		public Task<Result<ViewPaciente>> CreatePaciente(ViewPaciente paciente);
	}
}
