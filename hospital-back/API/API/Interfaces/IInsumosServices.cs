using API.ViewModels;

namespace API.Interfaces
{
	public interface IInsumosServices
	{
		public Task<Result<ViewInsumo>> AddInsumo(ViewInsumoAdd Insumo);
		public Task<Result<ViewInsumo>> GetInsumoById(int Id);
		public Task<Result<List<ViewInsumo>>> GetInsumos();
		public Task<Result<ViewInsumo>> UpdateInsumo(ViewInsumoUpdate Insumo);
		public Task<Result<int>> DeleteInsumo(int Id);
	}
}
