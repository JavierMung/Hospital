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
	public class PacientesController : ControllerBase
	{
		private readonly IPacientesServices _pacientes;

		public PacientesController(IPacientesServices pacientes)
		{
			_pacientes = pacientes;
		}

		[HttpGet("obtenerPacienteByCURP")]
		public async Task<ActionResult<Result<ViewPaciente>>> GetPacienteByCURP(string CURP)
		{
			var a = CURP.Trim().Length;
			var b = Regex.IsMatch(CURP.Trim().ToUpper(), "^[a-zA-Z0-9]*$");
			if (CURP.Trim().Length != 18 || !Regex.IsMatch(CURP.Trim().ToUpper(), "^[a-zA-Z0-9]*$"))
			{

				return BadRequest(new Result<ViewCita> { Message = "El CURP debe tener 18 caracteres alfanumericos", Status = StatusCodes.Status400BadRequest });

			}

			return await ExecuteOperation(async () => await _pacientes.GetPacienteByCURP(CURP));
		}

		[HttpPost("actualizarPaciente")]

		public async Task<ActionResult<Result<ViewPaciente>>> UpdatePaciente(ViewPaciente Paciente)
		{
			var a = Paciente.CURP.Trim().Length;
			var b = Regex.IsMatch(Paciente.CURP.Trim().ToUpper(), "^[a-zA-Z0-9]*$");
			if (Paciente.CURP.Trim().Length != 18 || !Regex.IsMatch(Paciente.CURP.Trim().ToUpper(), "^[a-zA-Z0-9]*$"))
			{

				return BadRequest(new Result<ViewCita> { Message = "El CURP debe tener 18 caracteres alfanumericos", Status = StatusCodes.Status400BadRequest });

			}

			return await ExecuteOperation(async () => await _pacientes.GetPacienteByCURP(Paciente.ToString()));
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
