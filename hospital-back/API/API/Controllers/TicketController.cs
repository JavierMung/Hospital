using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return await ExecuteOperation(async () => await _ticket.AddTicket(ticket));
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
