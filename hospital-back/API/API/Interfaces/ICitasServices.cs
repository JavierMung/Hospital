using API.ViewModels;

namespace API.Interfaces
{
	public interface ICitasServices
	{
		//public Task<List<ViewCita?>?> GetCitas();
		public Task<Result<ViewCita>> UpdateCita(ViewCitaAdd cita);
		public Task<Result<ViewCita>> DeleteCita(int id);
		public Task<Result<ViewCita>> GetCitaById(int id);
		public Task<Result<ViewCita>> CreateCita(ViewCitaAdd cita);
		public Task<Result<List<ViewCita>>> GetCitasByCURP(string CURP);
		public Task<Result<List<ViewCita>>> GetCitasByMedicoId(int Id);

	}
}
