using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.ViewModels;
using API.Context;

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

		[HttpGet("obtenerMedicoById/{id}")]
		public async Task<ActionResult<Result<ViewMedicos>>> GetMedico(int id)
		{
			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _medicosServices.GetMedico(id));
		}
		[HttpGet("obtenerCitasByTrabajadorId/{id}")]

		public async Task<ActionResult<Result<ViewMedicos>>> GetMedicoByTrabajadorId(int id)
		{
			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _medicosServices.GetMedicoByIdTrabajador(id));
		}

		[HttpGet("obtenerMedicos")]
		public async Task<ActionResult<Result<ViewListMedicos>>> GetMedicos()
		{
			return await ExecuteOperation(async () => await _medicosServices.GetMedicos());
		}

		[HttpPut("actualizarMedico")]
		public async Task<ActionResult<Result<ViewMedicos>>> UpdateMedico([FromBody] ViewMedicosUpdate medico)
		{
			return await ExecuteOperation(async () => await _medicosServices.UpdateMedico(medico));
		}

		[HttpPost("agregarMedico")]
		public async Task<ActionResult<Result<ViewMedicos>>> AddMedico([FromBody] ViewMedicoAdd medico)
		{
			if (medico.IdTrabajador <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID del trabajador es incorrecto.",
					Status = 400
				});
			return await ExecuteOperation(async () => await _medicosServices.AddMedico(medico));
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
