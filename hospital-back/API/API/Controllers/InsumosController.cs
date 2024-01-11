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
	public class InsumosController : ControllerBase
	{
		private readonly IInsumosServices _insumosServices;

		public InsumosController(IInsumosServices insumosController)
		{
			_insumosServices = insumosController;
		}

		[HttpGet("obtenerInsumoById/{id}")]
		public async Task<ActionResult<Result<ViewInsumo>>> GetInsumo(int id)
		{
			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _insumosServices.GetInsumoById(id));
		}

		[HttpPut("actualizarInsumo")]
		public async Task<ActionResult<Result<ViewInsumo>>> UpdateInsumo (ViewInsumoUpdate insumo)
		{
			if (insumo == null || insumo.IdTipo <= 0 || insumo.IdInsumo <= 0 || insumo.Precio <= 0 || insumo.Stock <= 0)
				return BadRequest(new Result<ViewInsumo>()
				{
					Model = null,
					Message = "Error en la petición. Revise los datos.",
					Status = StatusCodes.Status400BadRequest
				});
			return await ExecuteOperation(async () => await _insumosServices.UpdateInsumo(insumo));
		}

		[HttpGet("obtenerInsumos")]
		public async Task<ActionResult<Result<List<ViewInsumo>>>> GetInsumos()
		{
			return await ExecuteOperation(async () => await _insumosServices.GetInsumos());
		}
		
		[HttpPost("agregarInsumo")]
		public async Task<ActionResult<Result<ViewInsumo>>> AddInsumo([FromBody] ViewInsumoAdd Insumo)
		{
			if (Insumo == null || Insumo.Tipo <= 0 || Insumo.Stock <= 0 || Insumo.Precio <= 0)
				return BadRequest(new Result<ViewInsumo>()
				{
					Model = null,
					Message = "Error en la petición. Revise los datos.",
					Status = StatusCodes.Status400BadRequest
				});
			return await ExecuteOperation(async () => await _insumosServices.AddInsumo(Insumo));
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
