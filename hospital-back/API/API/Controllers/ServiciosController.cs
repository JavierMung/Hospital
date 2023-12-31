using API.Context;
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
		public async Task<ActionResult<Result<ViewServicio>>> GetServicio(int id)
		{
			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _servicioServices.GetServicio(id));
		}

		[HttpGet("obtenerServicios")]
		public async Task<ActionResult<Result<List<ViewServicio>>>> GetALLServicios()
		{
			return await ExecuteOperation(async () => await _servicioServices.GetAllServicios());
		}


		[HttpDelete("eliminarServicio/{id}")]
		public async Task<ActionResult<Result<int>>> DeleteServicio(int id)
		{
			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _servicioServices.DeleteServicio(id));
		}

		[HttpPost("agregarServicio")]
		public async Task<ActionResult<Result<ViewServicio>>> AddServicio([FromBody] ViewServicioAdd model)
		{

			return await ExecuteOperation(async () => await _servicioServices.AddServicio(model));
		}

		[HttpPut("actualizarServicio")]
		public async Task<ActionResult<Result<ViewServicio>>> UpdateServicio([FromBody] ViewServicio model)
		{
			if (model.idServicio <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _servicioServices.UpdateServicio(model));
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
				}else if (result.Status == 400)
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
