using API.ViewModels;

namespace API.Interfaces
{
	public interface IServiciosServices
	{
		public Task<ViewServicio?> GetServicio(int idServicio);
		public Task<List<ViewServicio>?> GetAllServicios();
        public Task<int> DeleteServicio(int idServicio);
        Task<bool> AddServicio(string nombreServicio, double costo);
        Task<bool> UpdateServicio(int idServicio, string servicio, double costo);


    }
}
