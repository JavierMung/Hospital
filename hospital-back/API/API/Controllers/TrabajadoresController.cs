using API.Interfaces;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class TrabajadoresController : ControllerBase
	{
		private readonly ITrabajadoresServices _trabajadorServices;
		//	private readonly IConfiguration _configuration;

		public TrabajadoresController(ITrabajadoresServices TrabajadoresServices)
		{
			_trabajadorServices = TrabajadoresServices;
		}
		[HttpGet("obtenerTrabajador/{id}")]
		public async Task<ActionResult<Result<ViewTrabajador>>> GetTrabajador(int id)
		{
			if (id <= 0)
			{
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});
			}

			return await ExecuteOperation(async () => await _trabajadorServices.GetTrabajador(id));

		}

		[HttpGet("obtenerTrabajadores")]
		public async Task<ActionResult<Result<List<ViewTrabajador>>>> GetTrabajadores()
		{

			return await ExecuteOperation(async () => await _trabajadorServices.GetTrabajadores());

		}

		[HttpDelete("eliminarTrabajador/{id}")]
		public async Task<ActionResult<Result<ViewTrabajador>>> DeleteTrabajador(int id)
		{

			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _trabajadorServices.DeleteTrabajador(id));

		}

		[HttpPost("agregarTrabajador")]
		public async Task<ActionResult<Result<ViewTrabajador>>> AddTrabajador(ViewAddTrabajador trabajadorRequest)
		{

			return await ExecuteOperation(async () => await _trabajadorServices.AddTrabajador(trabajadorRequest));

		}

		[HttpPut("actualizarTrabajador")]
		public async Task<ActionResult<Result<ViewTrabajador>>> UpdateTrabajador(ViewTrabajador model)
		{
			if (model.idTrabajador <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _trabajadorServices.UpdateTrabajador(model));

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

				return StatusCode(StatusCodes.Status200OK, result);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error en el servidor: {ex.Message}. Inténtelo más tarde.");
			}
		}

	}
}
