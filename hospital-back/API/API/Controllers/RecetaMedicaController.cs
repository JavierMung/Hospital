using API.Context;
using API.Interfaces;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class RecetaMedicaController : ControllerBase
	{
		private readonly IRecetaMedica _recetaMedica;

		public RecetaMedicaController(IRecetaMedica recetaMedica)
		{
			_recetaMedica = recetaMedica;
		}

		[HttpGet("obtenerRecetaMedicaByIdCita/{id}")]
		public async Task<ActionResult<Result<ViewRecetaMedica>>> GetRecetaMedicaByIdCita(int id)
		{

			return await ExecuteOperation(async () => await _recetaMedica.GetRecetaByIdCita(id));

		}

		[HttpPost("agregarRecetaMedica")]
		public async Task<ActionResult<Result<ViewRecetaMedica>>> AddRecetaMedica(ViewRecetaMedicaAdd receta)
		{

			return await ExecuteOperation(async () => await _recetaMedica.AddReceta(receta));

		}

		[HttpPost("actualizarRecetaMedica")]
		public async Task<ActionResult<Result<ViewRecetaMedica>>> UpdateRecetaMedica(ViewRecetaMedicaUpdate receta)
		{

			return await ExecuteOperation(async () => await _recetaMedica.UpdateReceta(receta));

		}

		public async Task<ActionResult<Result<T>>> ExecuteOperation<T>(Func<Task<Result<T>>> operation)
		{
			try
			{
				var result = await operation();


				if (result.Status == 500)
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
