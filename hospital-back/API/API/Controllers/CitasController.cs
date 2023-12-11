using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CitasController : ControllerBase
	{
		private readonly ICitasServices _citas;
		public CitasController(ICitasServices citas)
		{
			_citas = citas;
		}

		[HttpPost("crearCita")]

		public async Task<ActionResult<ViewCita>> CrearCita(ViewCita cita)
		{
			try
			{
				var respuesta = await _citas.CreateCita(cita);
				if (respuesta == null)
				{
					return StatusCode(StatusCodes.Status404NotFound, "Error al crear la cita, revise los datos por favor");
				}

				return respuesta;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
			}
		}
		[HttpGet("obtenerCitasByMedicoId/{id}")]

		public async Task<ActionResult<List<ViewCita?>>> GetCitasByMedicoId(int id)
		{
			try
			{
				var citas = await _citas.GetCitasByMedicoId(id);

				if (citas == null)
				{
					return StatusCode(StatusCodes.Status404NotFound, "Error al recuperar la cita debido al id del Medico, revise los datos por favor");
				}
				return citas;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
			}
		}

		[HttpGet("obtenerCitasByCitaId/{id}")]

		public async Task<ActionResult<ViewCita?>> GetCitaById(int id)
		{
			try
			{
				var cita = await _citas.GetCitaById(id);

				if (cita == null)
				{
					return StatusCode(StatusCodes.Status404NotFound, "Error al recuperar la cita debido al id de la cita, revise los datos por favor");
				}
				return cita;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
			}
		}
	}
}
