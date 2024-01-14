using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class PacientesServices : IPacientesServices
	{
		private readonly HospitalContext _context;
		public PacientesServices(HospitalContext context)
		{
			_context = context;
		}
		public async Task<Result<ViewPaciente>> CreatePaciente(ViewPaciente paciente)
		{

			try
			{
				Paciente paciente1 = new Paciente();
				paciente1.ApellidoMaterno = paciente.Apellido_Materno;
				paciente1.ApellidoPaterno = paciente.Apellido_Paterno;
				paciente1.Nombre = paciente.Nombre;
				paciente1.Edad = paciente.Edad;
				paciente1.Curp = paciente.CURP;
				await _context.Pacientes.AddAsync(paciente1);
				await _context.SaveChangesAsync();


				var pas = await _context.Pacientes.OrderByDescending(p => p.IdPaciente).FirstOrDefaultAsync();

				if (pas == null || pas.IdPaciente <= 0)
				{
					return new Result<ViewPaciente>
					{
						Model = null,
						Message = "Error al crear el paciente",
						Status = StatusCodes.Status500InternalServerError

					};
				}

				ViewPaciente res = new ViewPaciente(
					Id: pas.IdPaciente,
					Nombre: pas.Nombre,
					Apellido_Materno: pas.ApellidoMaterno,
					Apellido_Paterno: pas.ApellidoPaterno,
					Edad: pas.Edad,
					CURP:pas.Curp
				);

				 return new Result<ViewPaciente>
				{
					Model = res,
					Message = "Paciente creado con exito",
					Status = StatusCodes.Status200OK

				};
			}
			catch (Exception ex)
			{
				return new Result<ViewPaciente>
				{
					Model = null,
					Message = "Error al crear el paciente",
					Status = StatusCodes.Status500InternalServerError

				};
			}

		}

		public Task<Result<ViewPaciente>> DeletePaciente(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Result<ViewPaciente>> GetPacienteById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Result<List<ViewPaciente>>> GetPacientes()
		{
			throw new NotImplementedException();
		}

		public Task<Result<ViewPaciente>> UpdateCita(ViewPaciente paciente)
		{
			throw new NotImplementedException();
		}
	}
}
