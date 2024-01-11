using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class TicketController : ControllerBase
	{
		private readonly ITicketServices _ticket;
		public TicketController(ITicketServices ticket)
		{
			_ticket = ticket;
		}

		[HttpPost("crearTicket")]
		public async Task<ActionResult<Result<ViewTicketResponse>>> AddTicket(ViewTicketAdd ticket)
		{
			if (ticket == null)
			{
				return BadRequest(new Result<ViewTicketResponse> { Message = "No se permiten nulos.", Status = StatusCodes.Status400BadRequest });
			}
			if (ticket.Insumos.Count <= 0 && ticket.Servicios.Count <= 0)
			{
				return BadRequest(new Result<ViewTicketResponse> { Message = "No hay insumos o servicios por cobrar.", Status = StatusCodes.Status400BadRequest });
			}
			foreach (var insumo in ticket.Insumos)
			{
				if (insumo.Cantidad <= 0 || insumo.IdInsumo <= 0)
				{
					return BadRequest(new Result<ViewTicketResponse> { Message = "Error en los parametros cantidad o ID del insumo.", Status = StatusCodes.Status400BadRequest });
				}
			}
			foreach (var insumo in ticket.Servicios)
			{
				if (insumo.Cantidad <= 0 || insumo.IdServicio <= 0)
				{
					return BadRequest(new Result<ViewTicketResponse> { Message = "Error en los parametros cantidad o ID del servicio.", Status = StatusCodes.Status400BadRequest });
				}
			}
			return await ExecuteOperation(async () => await _ticket.AddTicket(ticket));
		}
		[HttpGet("obtenerTicket/{id}")]
		public async Task<ActionResult<Result<ViewTicket>>> GetTicket(int id)
		{
			if (id <= 0)
			{
				return BadRequest(new Result<ViewTicketResponse> { Message = "ID inscorrecto.", Status = StatusCodes.Status400BadRequest });
			}
			
			return await ExecuteOperation(async () => await _ticket.GetTicket(id));
		}

		public async Task<ActionResult<Result<T>>> ExecuteOperation<T>(Func<Task<Result<T>>> operation)
		{
			try
			{
				var result = await operation();

				if (result.Status == 204)
				{
					return StatusCode(StatusCodes.Status204NoContent, result);
				}
				else if (result.Status == 500)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, result);
				}
				else if (result.Status == 400)
				{
					return StatusCode(StatusCodes.Status400BadRequest, result);
				}

				return StatusCode(StatusCodes.Status200OK, result);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error en el servidor: {ex.Message}. Inténtelo más tarde.");
			}
		}

	}
}
