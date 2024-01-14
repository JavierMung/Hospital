using API.ViewModels;

namespace API.Interfaces
{
	public interface IServiciosServices
	{
		public Task<Result<ViewServicio>> GetServicio(int idServicio);
		public Task<Result<List<ViewServicio>>> GetAllServicios();
		public Task<Result<int>> DeleteServicio(int idServicio);
		public Task<Result<ViewServicio>> AddServicio(ViewServicioAdd model);
		public Task<Result<ViewServicio>> UpdateServicio(ViewServicio model);


	}
}
