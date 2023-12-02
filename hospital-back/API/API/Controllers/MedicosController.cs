using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.ViewModels;

namespace API.Controllers
{

	[Route("[controller]")]
	[ApiController]
	public class MedicosController : ControllerBase
	{
		private readonly IMedicosServices _medicosServices;
		private readonly IConfiguration _configuration;

		public MedicosController(IMedicosServices medicosServices, IConfiguration configuration)
		{
			_medicosServices = medicosServices;
			_configuration = configuration;
		}

		[HttpGet("obtenerMedico/{id}")]
		public async Task<ActionResult<ViewMedicos>> GetMedico(int id)
		{
			if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest, "El ID es incorrecto.");

			try
			{
				var res = await _medicosServices.GetMedico(id);
				if (res == null)
				{
					var mensajeStatus204 = _configuration.GetSection("MensajesDeEstatus:Status204:mensaje").Value ?? "xd";

					return StatusCode(StatusCodes.Status204NoContent, mensajeStatus204);
				}
				return StatusCode(StatusCodes.Status200OK,res);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status501NotImplemented, _configuration.GetSection("MensajesDeEstatus").GetValue("Status500", "Se ha producido un error interno en el servidor."));
			}
		}

		[HttpGet("obtenerMedicos")]
		public async Task<ActionResult<ViewListMedicos>> GetMedicos()
		{
			//if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest, "El ID es incorrecto.");

			try
			{
				var res = await _medicosServices.GetMedicos();
				if (res == null)
				{
					return StatusCode(StatusCodes.Status204NoContent, "No existen medicos con ese ID. intentelo con otro ID por favor.");
				}
				return res;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status501NotImplemented, "Error en el servidor: " + ex.Message + ". Intentelo mas tarde.");
			}
		}

	}
}
