using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces
{
	public interface IMedicosServices
	{
		public Task<ViewMedicos?> GetMedico(int id);
		public Task<ViewListMedicos?> GetMedicos();
	}
}

