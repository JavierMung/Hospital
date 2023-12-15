using API.ViewModels;

namespace API.Interfaces
{
	public interface ICitasServices
	{
		//public Task<List<ViewCita?>?> GetCitas();
		public Task<ViewCita?> UpdateCita(ViewCita cita);
		public Task<ViewCita?> DeleteCita(int id);
		public Task<ViewCita?> GetCitaById(int id);
		public Task<ViewCita?> CreateCita(ViewCita cita);
		public Task<List<ViewCita?>?> GetCitasByName(ViewCita cita);
		public Task<List<ViewCita?>?> GetCitasByMedicoId(int Id);

	}
}
