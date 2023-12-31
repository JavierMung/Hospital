using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Interfaces;
using API.ViewModels;
using System;

namespace API.Services
{
	public class TrabajadoresServices : ITrabajadoresServices
	{
		public readonly HospitalContext _context;

		public TrabajadoresServices(HospitalContext context)
		{
			_context = context;
		}

		public async Task<Result<ViewTrabajador>> GetTrabajador(int id)
		{
			try
			{
				var trabajador = await _context.Trabajadors
					.Include(t => t.IdPersonaNavigation) // Incluir detalles de la persona
					.Include(t => t.IdRolNavigation) // Incluir detalles del rol
					.Where(t => t.IdTrabajador == id) // Filtrar por ID del trabajador
					.FirstOrDefaultAsync();

				if (trabajador == null)
				{
					return new Result<ViewTrabajador> { Model = null, Message = "Trabajador no existente con ese ID.", Status = StatusCodes.Status204NoContent };
				}


				// Crear un nuevo objeto ViewTrabajador con la información requerida
				ViewTrabajador respuesta = new ViewTrabajador(
					trabajador.IdTrabajador,
					trabajador.IdRol,
					trabajador.IdHorario,
					trabajador.IdPersona,
					trabajador.FechaInicio,
					trabajador.Salario,
					new ViewPersona
					(
						IdPersona: trabajador.IdPersona,
						Nombre: trabajador.IdPersonaNavigation.Nombre,
						Apellido_Paterno: trabajador.IdPersonaNavigation.ApellidoPaterno,
						Apellido_Materno: trabajador.IdPersonaNavigation.ApellidoMaterno ?? "",
						Fecha_Nacimiento: trabajador.IdPersonaNavigation.FechaNacimiento,
						Calle: trabajador.IdPersonaNavigation.Calle ?? "",
						Colonia: trabajador.IdPersonaNavigation.Colonia ?? "",
						Municipio: trabajador.IdPersonaNavigation.Municipio ?? "",
						Estado: trabajador.IdPersonaNavigation.Estado ?? "",
						CP: trabajador.IdPersonaNavigation.Cp ?? "",
						Telefono: trabajador.IdPersonaNavigation.Telefono ?? ""
					)

				);

				return new Result<ViewTrabajador> { Model = respuesta, Message = "Trabajador encontrado con éxito.", Status = StatusCodes.Status200OK };
			}
			catch (Exception e)
			{
				return new Result<ViewTrabajador> { Model = null, Message = "Error en el servidor al recuperar el trabajdor.", Status = StatusCodes.Status500InternalServerError };

			}
		}

		public async Task<Result<List<ViewTrabajador>>> GetTrabajadores()
		{
			try
			{
				var consulta = await _context.Trabajadors
					.Include(t => t.IdPersonaNavigation) // Incluir detalles de la persona
					.Include(t => t.IdRolNavigation) // Incluir detalles del rol
					.ToListAsync();

				if (!consulta.Any())
				{
					return new Result<List<ViewTrabajador>> { Model = null, Message = "No hay trabajadores para mostrar.", Status = StatusCodes.Status204NoContent };

				}

				List<ViewTrabajador> Trabajadores = new List<ViewTrabajador>();

				foreach (var trabajador in consulta)
				{
					Trabajadores.Add(
					new ViewTrabajador(
					trabajador.IdTrabajador,
					trabajador.IdRol,
					trabajador.IdHorario,
					trabajador.IdPersona,
					trabajador.FechaInicio,
					trabajador.Salario,
					new ViewPersona
					(
						IdPersona: trabajador.IdPersona,
						Nombre: trabajador.IdPersonaNavigation.Nombre,
						Apellido_Paterno: trabajador.IdPersonaNavigation.ApellidoPaterno,
						Apellido_Materno: trabajador.IdPersonaNavigation.ApellidoMaterno ?? "",
						Fecha_Nacimiento: trabajador.IdPersonaNavigation.FechaNacimiento,
						Calle: trabajador.IdPersonaNavigation.Calle ?? "",
						Colonia: trabajador.IdPersonaNavigation.Colonia ?? "",
						Municipio: trabajador.IdPersonaNavigation.Municipio ?? "",
						Estado: trabajador.IdPersonaNavigation.Estado ?? "",
						CP: trabajador.IdPersonaNavigation.Cp ?? "",
						Telefono: trabajador.IdPersonaNavigation.Telefono ?? ""
					)

				));
				}

				return new Result<List<ViewTrabajador>> { Model = Trabajadores, Message = "Trabajadores encontrados cone exito.", Status = StatusCodes.Status200OK }; ;
			}
			catch (Exception ex)
			{
				return new Result<List<ViewTrabajador>> { Model = null, Message = "Error en el servidor al recuperar los trabajadores.", Status = StatusCodes.Status500InternalServerError };

			}
		}
		public async Task<Result<ViewTrabajador>> DeleteTrabajador(int idTrabajador)
		{
			try
			{
				var trabajador = await _context.Trabajadors.FindAsync(idTrabajador);

				if (trabajador == null)
				{
					return new Result<ViewTrabajador> { Model = null, Message = "Trabajador no encontrado.", Status = StatusCodes.Status204NoContent };
				}

				// Verificar si hay médicos asociados al trabajador
				if (_context.Medicos.Any(m => m.IdTrabajador == idTrabajador))
				{
					return new Result<ViewTrabajador> { Message = "No se puede eliminar el Trabajador debido a las dependencias existentes." };
				}
				var personaServices = new PersonaServices(_context);

				var person = await personaServices.GetPersonaById(trabajador.IdPersona);

				if (person.Model == null)
				{
					return new Result<ViewTrabajador> { Message = person.Message , Status = StatusCodes.Status204NoContent };
				}

				_context.Trabajadors.Remove(trabajador);
				await _context.SaveChangesAsync();


				return new Result<ViewTrabajador> { Model = new ViewTrabajador(trabajador.IdTrabajador, trabajador.IdRol, trabajador.IdHorario, trabajador.IdPersona, trabajador.FechaInicio, trabajador.Salario, person.Model), Message = "Trabajador eliminado con éxito." };
			}
			catch (DbUpdateException)
			{
				return new Result<ViewTrabajador> { Message = "Error al eliminar el Trabajador." };
			}
			catch (Exception)
			{
				return new Result<ViewTrabajador> { Message = "Error durante la eliminación." };
			}
		}


		public async Task<Result<ViewTrabajador>> AddTrabajador(ViewAddTrabajador trabajadorRequest)
		{
			using (var transaction = _context.Database.BeginTransaction())

				try
				{
					var personaServices = new PersonaServices(_context);

					var person = await personaServices.CreatePersona(trabajadorRequest.Persona);

					if (person.Model == null)
					{
						return new Result<ViewTrabajador> { Message = person.Message };

					}


					Trabajador nuevoTrabajador = new Trabajador
					{
						IdRol = trabajadorRequest.idRol,
						IdHorario = trabajadorRequest.IdHorario,
						IdPersona = person.Model.IdPersona,
						FechaInicio = trabajadorRequest.FechaInicio,
						Salario = trabajadorRequest.Salario
					};

					_context.Trabajadors.Add(nuevoTrabajador);
					await _context.SaveChangesAsync();
					transaction.Commit();



					return new Result<ViewTrabajador> { Model = new ViewTrabajador(nuevoTrabajador.IdTrabajador, nuevoTrabajador.IdRol, nuevoTrabajador.IdHorario, nuevoTrabajador.IdPersona, nuevoTrabajador.FechaInicio, nuevoTrabajador.Salario, person.Model), Message = "Trabajador agregado con éxito." };
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					return new Result<ViewTrabajador> { Message = "Error al crear al trabajador." };
				}
		}


		public async Task<Result<ViewTrabajador>> UpdateTrabajador(ViewTrabajador trabajador)
		{
			using (var transaction = _context.Database.BeginTransaction())
				try
				{
					var trabajadorEntity = await _context.Trabajadors
						.Include(t => t.IdPersonaNavigation)
						.Where(t => t.IdTrabajador == trabajador.idTrabajador)
						.FirstOrDefaultAsync();

					if (trabajadorEntity == null)
					{

						return new Result<ViewTrabajador> { Model = null, Message = "No se encotró el trabajador.", Status = 204 };

					}

					trabajadorEntity.IdRol = trabajador.idRol;
					trabajadorEntity.IdHorario = trabajador.IdHorario;
					trabajadorEntity.FechaInicio = trabajador.FechaInicio;
					trabajadorEntity.Salario = trabajador.Salario;


					_context.Trabajadors.Update(trabajadorEntity);

					await _context.SaveChangesAsync();

					var personaServices = new PersonaServices(_context);

					var pe = await personaServices.UpdatePersona(trabajador.Persona);

					if (pe.Model == null)
					{
						return new Result<ViewTrabajador> { Model = null, Message = "No se encotró a la persona.", Status = 204 };

					}


					var updatedModel = new ViewTrabajador(
						trabajadorEntity.IdTrabajador,
						trabajadorEntity.IdRol,
						trabajadorEntity.IdHorario,
						trabajadorEntity.IdPersona,
						trabajadorEntity.FechaInicio,
						trabajadorEntity.Salario,
						pe.Model

					);

					transaction.Commit();
					return new Result<ViewTrabajador> { Model = updatedModel, Message = "Trabajador actualizado con éxito." };
				}
				catch (Exception)
				{
					transaction.Rollback();
					return new Result<ViewTrabajador> { Message = "Error al actualizar el Trabajador." };
				}
		}

	}
}
