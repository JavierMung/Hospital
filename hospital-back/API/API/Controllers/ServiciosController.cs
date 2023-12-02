using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ServiciosController : ControllerBase
	{
		private readonly IServiciosServices _servicioServices;

		public ServiciosController(IServiciosServices servicioServices)
		{
			_servicioServices = servicioServices;
		}
		[HttpGet("obtenerServicio/{id}")]
		public async Task<ActionResult<ViewServicio>> GetServicio(int id)
		{
			try
			{
				var res = await _servicioServices.GetServicio(id);
				if (res == null) return StatusCode(StatusCodes.Status204NoContent, "No se encontraron sevicios con ese ID.");
				return res;
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Intentelo más tarde. ");
			}

		}

		[HttpGet("obtenerServicios")]
		public async Task<ActionResult<List<ViewServicio>?>> GetALLServicios()
		{
			try
			{
				var res = await _servicioServices.GetAllServicios();
				if (res == null) return StatusCode(StatusCodes.Status204NoContent, "No se encontraron sevicios con ese ID.");
				return res;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Intentelo más tarde. ");
			}

		}
	}
}
