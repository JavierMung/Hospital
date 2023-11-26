using API.ViewModels;

namespace API.Interfaces
{
	public interface IMedicosServices
	{
		public Task<ViewMedicos?> GetMedico(int id);
	}
}
