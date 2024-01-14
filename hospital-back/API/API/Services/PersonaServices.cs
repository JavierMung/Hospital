using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Services
{
	public class PersonaServices : IPersonaServices
	{
		private readonly HospitalContext _context;
		public PersonaServices(HospitalContext context)
		{
			_context = context;
		}

		public async Task<Result<ViewPersona>> CreatePersona(ViewPersona persona)
		{

			Persona persona1 = new()
			{
				Nombre = persona.Nombre,
				ApellidoPaterno = persona.Apellido_Paterno,
				ApellidoMaterno = persona.Apellido_Materno,
				FechaNacimiento = persona.Fecha_Nacimiento,
				Calle = persona.Calle,
				Colonia = persona.Colonia,
				Municipio = persona.Municipio,
				Estado = persona.Estado,
				Cp = persona.CP,
				Telefono = persona.Telefono
			};

			try
			{
				await _context.Personas.AddAsync(persona1);

				_context.SaveChanges();

				var id = await _context.Personas.OrderByDescending(p => p.IdPersona).Select(pe => pe.IdPersona).FirstOrDefaultAsync();

				var respuesta = await GetPersonaById(id);

				if (respuesta.Model == null)
				{
					return new Result<ViewPersona> { Model = respuesta.Model, Message = "Trabajador creado con exito.", Status = StatusCodes.Status200OK };
				}

				return new Result<ViewPersona> { Model = respuesta.Model, Message = "Trabajador creado con exito.", Status = StatusCodes.Status200OK };
			}
			catch (Exception ex)
			{
				throw new Exception("Error al crear la persona: " + ex);
			}

		}

		public Task<Result<ViewPersona>> DeletePersona(ViewPersona persona)
		{
			throw new NotImplementedException();
		}

		public async Task<Result<ViewPersona>> GetPersonaById(int id)
		{
			var persona = await _context.Personas.Where(p => p.IdPersona == id).FirstOrDefaultAsync();

			if (persona == null)
			{
				return new Result<ViewPersona> { Model = null, Message = "No existe persona con ese ID.", Status = StatusCodes.Status204NoContent };

			}

			ViewPersona respuesta = new
				(
				IdPersona: persona.IdPersona,
				Nombre: persona.Nombre,
				Apellido_Paterno: persona.ApellidoPaterno,
				Apellido_Materno: persona.ApellidoMaterno ?? "",
				Fecha_Nacimiento: persona.FechaNacimiento,
				Calle: persona.Calle ?? "",
				Colonia: persona.Colonia ?? "",
				Municipio: persona.Municipio ?? "",
				Estado: persona.Estado ?? "",
				CP: persona.Cp ?? "",
				Telefono: persona.Telefono ?? ""
				);

			return new Result<ViewPersona> { Model = respuesta, Message = "Persona encontrada con exito.", Status = StatusCodes.Status200OK };
		}

		public async Task<Result<ViewPersona>> UpdatePersona(ViewPersona persona)
		{
			var pe = await _context.Personas.FindAsync(persona.IdPersona);
			if (pe == null)
			{
				return new Result<ViewPersona> { Model = null, Message = "La persona no existente con ese ID", Status = StatusCodes.Status204NoContent };
			}
			pe.Nombre = persona.Nombre;
			pe.ApellidoPaterno = persona.Apellido_Paterno;
			pe.ApellidoMaterno = persona.Apellido_Materno;
			pe.Calle = persona.Calle;
			pe.Colonia = persona.Colonia;
			pe.Municipio = persona.Municipio;
			pe.Estado = persona.Estado;
			pe.Cp = persona.CP;
			pe.Telefono = persona.Telefono;
			await _context.SaveChangesAsync();

			var respuesta = await GetPersonaById(persona.IdPersona);

			return respuesta;
		}
	}
}
