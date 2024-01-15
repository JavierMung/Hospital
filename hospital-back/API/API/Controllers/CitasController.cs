using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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

		public async Task<ActionResult<Result<ViewCita>>> CrearCita(ViewCitaAdd cita)
		{
			if (cita.IdMedico <= 0)
			{
				return BadRequest(new Result<ViewCita> { Message = "El ID del medico es incorrecto.", Status = StatusCodes.Status400BadRequest });
			}

			if (cita.paciente.CURP == null)
			{

				return BadRequest(new Result<ViewCita> { Message = "El CURP no puede ser NULL", Status = StatusCodes.Status400BadRequest });

			}

			if (cita.FechaCita.AddMonths(-3) > DateTime.Now)
			{
				return BadRequest(new Result<ViewCita> { Message = "La cita tiene debe ser menor a 3 meses.", Status = StatusCodes.Status400BadRequest });

			}

			if (DateTime.Now.AddHours(1) > cita.FechaCita)
			{

				return BadRequest(new Result<ViewCita> { Message = "La cita solo se puede crear con minimo una hora de anticipación.", Status = StatusCodes.Status400BadRequest });

			}
			var a = cita.paciente.CURP.Trim().Length;
			var b = Regex.IsMatch(cita.paciente.CURP.Trim().ToUpper(), "^[a-zA-Z0-9]*$");
			if (cita.paciente.CURP.Trim().Length != 18 || !Regex.IsMatch(cita.paciente.CURP.Trim().ToUpper(), "^[a-zA-Z0-9]*$"))
			{

				return BadRequest(new Result<ViewCita> { Message = "El CURP debe tener 18 caracteres alfanumericos", Status = StatusCodes.Status400BadRequest });

			}


			return await ExecuteOperation(async () => await _citas.CreateCita(cita));
		}
		[HttpGet("obtenerCitasByMedicoId/{id}")]

		public async Task<ActionResult<Result<List<ViewCita>>>> GetCitasByMedicoId(int id)
		{
			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _citas.GetCitasByMedicoId(id));
		}

		[HttpGet("obtenerCitasByCURP")]

		public async Task<ActionResult<Result<List<ViewCita>>>> GetCitaByCURP([FromQuery] string CURP)
		{
			if (CURP == null)
			{
				return BadRequest(new Result<ViewCita> { Message = "El CURP no puede ser NULL", Status = StatusCodes.Status400BadRequest });
			}
			if (CURP.Trim().Length != 18 || !Regex.IsMatch(CURP.Trim().ToUpper(), "^[a-zA-Z0-9]*$"))
			{
				return BadRequest(new Result<ViewCita> { Message = "El CURP debe tener 18 caracteres alfanumericos", Status = StatusCodes.Status400BadRequest });

			}
			return await ExecuteOperation(async () => await _citas.GetCitasByCURP(CURP.ToUpper()));
		}

		[HttpPut("actualizarCita")]

		public async Task<ActionResult<Result<ViewCita>>> UpdateCita(ViewCitaAdd citaP)
		{
			if (citaP.id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			var cita = await _citas.GetCitaById(citaP.id);

			if (cita?.Model?.FechaCita != citaP.FechaCita && DateTime.Now.AddDays(3) < citaP.FechaCita)
			{

				return BadRequest(new Result<ViewCita> { Message = "La cita solo se puede actualizar con 3 días de anticipación.", Status = StatusCodes.Status400BadRequest });

			}


			return await ExecuteOperation(async () => await _citas.UpdateCita(citaP));
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
