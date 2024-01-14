using API.ViewModels;

namespace API.Interfaces
{
	public interface ITicketServices
	{
		public Task<Result<ViewTicketResponse>> AddTicket (ViewTicketAdd ticket);
		public Task<Result<ViewTicket>> GetTicket (int IdTicket);

	}
}
