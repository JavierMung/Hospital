using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_API_HOSPITAL.Interfaces;
using REST_API_HOSPITAL.ViewModels;

namespace REST_API_HOSPITAL.Controllers
{

	[Route("[controller]")]
	[ApiController]
	public class MedicosController : ControllerBase
	{
		private readonly IMedicosServices _medicosServices;

		public MedicosController(IMedicosServices medicosServices)
		{
			_medicosServices = medicosServices;
		}

		[HttpGet("obtenerMedico/{id}")]
		public async Task<ActionResult<ViewMedicos>> GetMedico(int id)
		{
			if (id == 0)
			{
				return StatusCode(StatusCodes.Status400BadRequest, "El ID es incorrecto");
			}
			try
			{
				var res = await _medicosServices.GetMedico(id);
				if (res == null)
				{
					return StatusCode(StatusCodes.Status204NoContent, "No existen medicos con ese ID. intentelo con otro ID por favor");
				}
				return res;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status501NotImplemented, "Error en el servidor: "+ex.Message+"Intentelo mas tarde");
			}
		}
	}
}
