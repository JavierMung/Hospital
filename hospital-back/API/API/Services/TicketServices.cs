using API.Context;
using API.Interfaces;
using API.ViewModels;

namespace API.Services
{
	public class TicketServices : ITicketServices
	{
		private readonly HospitalContext _context;
		public TicketServices(HospitalContext context)
		{
			_context = context;
		}
		public async Task<Result<ViewTicketResponse>> AddTicket(ViewTicketAdd ticket)
		{
			Ticket nuevoTicket = new Ticket
			{
				IdTrabajador = ticket.IdTrabajador,
				Fecha = DateTime.Now,
			};

			var costo = 0.0;

			foreach (var insumo in ticket.Insumos)
			{
				var insumo_ = await _context.Insumos.FindAsync(insumo.IdInsumo);
				if (insumo_ == null)
				{
					return new Result<ViewTicketResponse> { Message = $"El ID = {insumo.IdInsumo} del insumo no existe", Status = StatusCodes.Status400BadRequest }
				}

				costo += insumo.Cantidad * insumo_.Precio;

			}

			foreach (var servicio in ticket.Servicios)
			{
				var insumo_ = await _context.Servicios.FindAsync(servicio.IdServicio);
				if (insumo_ == null)
				{
					return new Result<ViewTicketResponse> { Message = $"El ID = {servicio.IdServicio} del servicio no existe", Status = StatusCodes.Status400BadRequest }
				}

				costo = servicio.Cantidad * insumo_.Costo;

			}

			nuevoTicket.Total = costo;


			var sticket = await _context.Tickets.AddAsync(nuevoTicket);


			throw new NotImplementedException();
		}

		public Task<Result<ViewTicketResponse>> GetTicket(int IdTicket)
		{
			throw new NotImplementedException();
		}
	}
}
