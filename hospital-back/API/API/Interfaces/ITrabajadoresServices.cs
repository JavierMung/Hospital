using API.ViewModels;
using System.Data;

namespace API.Interfaces
{
	public interface ITrabajadoresServices
	{
		public Task<Result<ViewTrabajador>> GetTrabajador(int id_Trabajador);

		public Task<Result<List<ViewTrabajador>>> GetTrabajadores();

		public Task<Result<ViewTrabajador>> DeleteTrabajador(int idTrabajador);

		public Task<Result<ViewTrabajador>> AddTrabajador(ViewAddTrabajador trabajadorRequest);

		public Task<Result<ViewTrabajador>> UpdateTrabajador(ViewTrabajador trabajador);
	}
}
