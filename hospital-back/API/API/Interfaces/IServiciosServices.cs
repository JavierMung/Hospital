using API.ViewModels;

namespace API.Interfaces
{
	public interface IServiciosServices
	{
		public Task<ViewServicio?> GetServicio(int idServicio);
		public Task<List<ViewServicio>?> GetAllServicios();
        public Task<int> DeleteServicio(int idServicio);
        public Task<ViewServicio?> AddServicio(ViewServicio model);
        public Task<ViewServicio?> UpdateServicio(ViewServicio model);


    }
}
