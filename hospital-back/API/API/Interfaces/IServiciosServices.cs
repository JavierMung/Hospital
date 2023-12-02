using API.ViewModels;

namespace API.Interfaces
{
	public interface IServiciosServices
	{
		public Task<ViewServicio?> GetServicio(int idServicio);
		public Task<List<ViewServicio>?> GetAllServicios();

	}
}
