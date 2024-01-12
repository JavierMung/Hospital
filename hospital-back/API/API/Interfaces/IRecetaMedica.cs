using API.ViewModels;

namespace API.Interfaces
{
	public interface IRecetaMedica
	{
		public Task<Result<ViewRecetaMedica>> GetRecetaByIdCita(int id);
		public Task<Result<ViewRecetaMedica>> AddReceta(ViewRecetaMedicaAdd receta);
		public Task<Result<ViewRecetaMedica>> UpdateReceta(ViewRecetaMedicaUpdate receta);
	}
}
