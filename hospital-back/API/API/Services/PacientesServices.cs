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
		public async Task<ViewPaciente?> CreatePaciente(ViewPaciente paciente)
		{

			try
			{
				Paciente paciente1 = new Paciente();
				paciente1.ApellidoMaterno = paciente.Apellido_Materno;
				paciente1.ApellidoPaterno = paciente.Apellido_Paterno;
				paciente1.Nombre = paciente.Nombre;
				paciente1.Edad = paciente.Edad;

				await _context.Pacientes.AddAsync(paciente1);
				await _context.SaveChangesAsync();


				var pas = await _context.Pacientes.OrderByDescending(p => p.IdPaciente).FirstOrDefaultAsync();

				if (pas == null || pas.IdPaciente <= 0)
				{
					return null;
				}

				ViewPaciente res = new ViewPaciente(
					Id: pas.IdPaciente,
					Nombre: pas.Nombre,
					Apellido_Materno: pas.ApellidoMaterno,
					Apellido_Paterno: pas.ApellidoPaterno,
					Edad: pas.Edad
				);

				return res;
			}
			catch (Exception ex)
			{

				throw new Exception("Error en la creacion del Paciente: " + ex.Message.ToString());
			}

		}

		public Task<ViewPaciente?> DeletePaciente(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ViewPaciente?> GetPacienteById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<ViewPaciente?>?> GetPacientes()
		{
			throw new NotImplementedException();
		}

		public Task<ViewPaciente?> UpdateCita(ViewPaciente paciente)
		{
			throw new NotImplementedException();
		}
	}
}
