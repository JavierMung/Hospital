using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;

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
			using var transaction = _context.Database.BeginTransaction();
			try
			{

				Ticket nuevoTicket = new()
				{
					IdTrabajador = ticket.IdTrabajador,
					Fecha = DateTime.Now,
				};

				List<TicketsInsumo> ticketsInsumos = new List<TicketsInsumo>();
				List<ServiciosTicket> ticketsServicios = new List<ServiciosTicket>();

				var costo = 0.0;

				foreach (var insumo in ticket.Insumos)
				{
					var insumo_ = await _context.Insumos.FindAsync(insumo.IdInsumo);
					if (insumo_ == null)
					{
						return new Result<ViewTicketResponse> { Message = $"El ID = {insumo.IdInsumo} del insumo no existe.", Status = StatusCodes.Status400BadRequest };
					}



					if (insumo_.Stock < insumo.Cantidad || insumo_.Stock <= 0)
					{
						return new Result<ViewTicketResponse> { Message = $"No es posible crear el ticket. El insumo {insumo_.Nombre} solo cuenta con {insumo_.Stock} unidades.", Status = StatusCodes.Status400BadRequest };
					}
					costo += insumo.Cantidad * insumo_.Precio;

					var t = new TicketsInsumo
					{
						IdInsumo = insumo_.IdInsumo,
						Cantidad = insumo.Cantidad,
						PreTotal = insumo.Cantidad * insumo_.Precio
					};

					insumo_.Stock -= insumo.Cantidad;
					ticketsInsumos.Add(t);
				}

				foreach (var servicio in ticket.Servicios)
				{
					var servicio_ = await _context.Servicios.FindAsync(servicio.IdServicio);
					if (servicio_ == null)
					{
						return new Result<ViewTicketResponse> { Message = $"El ID = {servicio.IdServicio} del servicio no existe", Status = StatusCodes.Status400BadRequest };
					}

					costo += servicio.Cantidad * servicio_.Costo;

					var t = new ServiciosTicket
					{
						IdServicio = servicio_.IdServicio,
						Cantidad = servicio.Cantidad,
						PreTotal = servicio.Cantidad * servicio_.Costo

					};
					ticketsServicios.Add(t);
				}

				nuevoTicket.Total = costo;


				var sticket = await _context.Tickets.AddAsync(nuevoTicket);

				await _context.SaveChangesAsync();

				var id = nuevoTicket.IdTicket;

				foreach (var t in ticketsServicios) { t.IdTicket = id; }
				foreach (var t in ticketsInsumos) { t.IdTicket = id; }

				_context.TicketsInsumos.AddRange(ticketsInsumos);
				_context.ServiciosTickets.AddRange(ticketsServicios);



				await _context.SaveChangesAsync();

				transaction.Commit();

				return new Result<ViewTicketResponse>()
				{
					Model = new ViewTicketResponse(ticket, costo, id),
					Message = "Ticket creado con exito.",
					Status = StatusCodes.Status200OK

				};
			}
			catch (Exception)
			{
				transaction.Rollback();
				return new Result<ViewTicketResponse>()
				{
					Model = null,
					Message = "Hubo un error al crear el ticket.",
					Status = StatusCodes.Status500InternalServerError

				};
			}
		}

		public async Task<Result<ViewTicket>> GetTicket(int IdTicket)
		{
			try
			{

				var ticket = await _context.FindAsync<Ticket>(IdTicket);

				if (ticket == null) return new Result<ViewTicket>() { Message = "No existe ticket con ese ID.", Status = StatusCodes.Status400BadRequest };


				var ticketConDetalles = _context.Tickets
						.Where(t => t.IdTicket == IdTicket)
						.Select(t => new ViewTicket
						(
							t.IdTicket,
							t.IdTrabajador,
							t.Total,
							t.TicketsInsumos.Select(t =>
							new ViewInsumoResponse(t.IdInsumo, t.IdInsumoNavigation.Nombre, t.IdInsumoNavigation.Precio ,t.Cantidad, t.IdInsumoNavigation.IdTipoInsumo, t.IdInsumoNavigation.IdTipoInsumoNavigation.Tipo, t.PreTotal)).ToList(),
							t.ServiciosTickets.Select(t =>
							new ViewServicioTicket(t.IdTicket, t.IdServicioNavigation.Servicio1, t.IdServicioNavigation.Costo, t.Cantidad, t.PreTotal)).ToList()
						))
						.FirstOrDefault();

				return new Result<ViewTicket>()
				{
					Model = ticketConDetalles,
					Message = "Ticket encontrado con exito.",
					Status = StatusCodes.Status200OK
				};
			}
			catch (Exception)
			{
				return new Result<ViewTicket>()
				{
					Model = null,
					Message = "Error interno al buscar el ticket. Intentelo más tarde.",
					Status = StatusCodes.Status500InternalServerError
				};
			}

		}
	}
}
