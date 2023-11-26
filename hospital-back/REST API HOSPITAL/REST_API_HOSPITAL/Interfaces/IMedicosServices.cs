using REST_API_HOSPITAL.ViewModels;

namespace REST_API_HOSPITAL.Interfaces
{
	public interface IMedicosServices
	{
		public Task<ViewMedicos?> GetMedico(int id);
	}
}
