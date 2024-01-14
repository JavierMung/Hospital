using API.ViewModels;

namespace API.Interfaces
{
	public interface IPersonaServices
	{
		public Task<Result<ViewPersona>> UpdatePersona(ViewPersona persona);
		public Task<Result<ViewPersona>> DeletePersona(ViewPersona persona);
		public Task<Result<ViewPersona>> GetPersonaById(int id);
		public Task<Result<ViewPersona>> CreatePersona(ViewPersona persona);

	}
}
