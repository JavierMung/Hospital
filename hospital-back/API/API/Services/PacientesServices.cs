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
					throw new Exception();
				}

				ViewPaciente res = new ViewPaciente(
					Id: pas.IdPaciente,
					Nombre: pas.Nombre,
					Apellido_Materno: pas.ApellidoMaterno,
					Apellido_Paterno: pas.ApellidoPaterno,
					Edad: pas.Edad,
					CURP: pas.Curp
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

		public async Task<Result<ViewPaciente>> GetPacienteByCURP(string CURP)
		{
			try
			{

				var paciente = await _context.Pacientes.Where(pac => pac.Curp == CURP).FirstOrDefaultAsync();
				if (paciente == null)
				{
					return new Result<ViewPaciente>
					{
						Model = null,
						Message = "El paciente no existe.",
						Status = StatusCodes.Status200OK

					};
				}

				return new Result<ViewPaciente>
				{
					Model = new ViewPaciente(paciente.IdPaciente, paciente.Nombre, paciente.ApellidoPaterno, paciente.ApellidoMaterno, paciente.Edad, paciente.Curp),
					Message = "Paciente recuperado con exito.",
					Status = StatusCodes.Status200OK

				};
			}
			catch (Exception)
			{
				return new Result<ViewPaciente>
				{
					Model = null,
					Message = "Error al crear el paciente",
					Status = StatusCodes.Status500InternalServerError

				};
			}
		}

		public Task<Result<List<ViewPaciente>>> GetPacientes()
		{
			throw new NotImplementedException();
		}

		public async Task<Result<ViewPaciente>> UpdatePacienteByCURP(ViewPaciente paciente)
		{
			using var transaction = _context.Database.BeginTransaction();
			try
			{
				var pacienteParaActualizar = await _context.Pacientes.Where(pac => pac.Curp == paciente.CURP).FirstOrDefaultAsync();
				if (pacienteParaActualizar == null)
				{
					return new Result<ViewPaciente>
					{
						Model = null,
						Message = "El paciente no existe.",
						Status = StatusCodes.Status200OK

					};
				}

				pacienteParaActualizar.Edad = paciente.Edad;
				pacienteParaActualizar.ApellidoMaterno = paciente.Apellido_Materno;
				pacienteParaActualizar.ApellidoPaterno = paciente.Apellido_Paterno;

				await _context.SaveChangesAsync();
				transaction.Commit();

				return new Result<ViewPaciente>
				{
					Model = paciente,
					Message = "Paciente actualizado con exito.",
					Status = StatusCodes.Status200OK

				};

			}
			catch (Exception)
			{
				transaction.Rollback();
				return new Result<ViewPaciente>
				{
					Model = null,
					Message = "Error al actualizar el paciente",
					Status = StatusCodes.Status500InternalServerError

				};
			}
		}
	}
}
