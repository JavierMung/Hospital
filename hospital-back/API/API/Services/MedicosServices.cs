using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;

namespace API.Services
{
	public class MedicosServices : IMedicosServices
	{
		public readonly HospitalContext _context;

		public MedicosServices(HospitalContext context)
		{
			_context = context;
		}

		public async Task<Result<ViewMedicos>> AddMedico(ViewMedicoAdd medicosAdd)
		{
			using var transaction = _context.Database.BeginTransaction();
			try
			{
				var trabajador = await _context.Medicos.Where(m => m.IdTrabajador == medicosAdd.IdTrabajador || m.Cedula == medicosAdd.Cedula || m.Consultorio == medicosAdd.Consultorio).FirstOrDefaultAsync();
				if (trabajador != null)
				{
					if (trabajador.IdTrabajador == medicosAdd?.IdTrabajador)
						return new Result<ViewMedicos> { Model = null, Message = "Ya existe un medico relacionado a ese trabajador.", Status = StatusCodes.Status400BadRequest };
					if (trabajador.Cedula == medicosAdd?.Cedula)
						return new Result<ViewMedicos> { Model = null, Message = "Ya existe un medico relacionado a esa cedula.", Status = StatusCodes.Status400BadRequest };
					if (trabajador.Consultorio == medicosAdd?.Consultorio)
						return new Result<ViewMedicos> { Model = null, Message = "Ya existe un medico relacionado a ese consultorio.", Status = StatusCodes.Status400BadRequest };
					else throw new Exception();
				}

				var nuevoMedico = new Medico()
				{
					IdTrabajador = medicosAdd.IdTrabajador,
					Consultorio = medicosAdd.Consultorio,
					Consulta = medicosAdd.Consulta,
					Especialidad = medicosAdd.Especialidad,
					Cedula = medicosAdd.Cedula,
					Status = "ACTIVO"
				};

				await _context.Medicos.AddAsync(nuevoMedico);

				await _context.SaveChangesAsync();


				transaction.Commit();

				var medico = await GetMedico(nuevoMedico.IdMedico);

				return new Result<ViewMedicos> { Model = medico.Model, Message = "Medico agregado con exito.", Status = StatusCodes.Status200OK };
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				return new Result<ViewMedicos> { Model = null, Message = "Error al agregar el medico.", Status = StatusCodes.Status500InternalServerError };
			}
			throw new NotImplementedException();
		}

		public async Task<Result<ViewMedicos>> GetMedico(int id)
		{
			try
			{
				var medico = await _context.Medicos
			   .Include(m => m.IdTrabajadorNavigation)
			   .ThenInclude(t => t.IdPersonaNavigation)
			   .Include(m => m.IdTrabajadorNavigation.IdHorarioNavigation)
			   .Where(m => m.IdMedico == id).FirstOrDefaultAsync();

				if (medico == null)
				{
					return new Result<ViewMedicos> { Model = null, Message = "No se encontro el medico con ese ID.", Status = StatusCodes.Status204NoContent };

				}
				ViewMedicos respuesta = new(
					medico.IdMedico,
					medico.IdTrabajador,
					medico.IdTrabajadorNavigation.IdPersonaNavigation.Nombre + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoMaterno,
					medico.Especialidad,
					medico.Consultorio ?? "",
					medico.Consultorio ?? "",
					medico.Status ?? "",
					medico.Consulta,
					medico.IdTrabajadorNavigation.IdHorarioNavigation.HoraInicio,
					medico.IdTrabajadorNavigation.IdHorarioNavigation.HoraFin
					);


				return new Result<ViewMedicos> { Model = respuesta, Message = "Medico encontrado con exito.", Status = StatusCodes.Status200OK };
			}
			catch (Exception e)
			{

				return new Result<ViewMedicos> { Model = null, Message = "Error al buscar el medico.", Status = StatusCodes.Status500InternalServerError };

			}
		}

		public async Task<Result<ViewMedicos>> GetMedicoByIdTrabajador(int id)
		{
			try
			{
				var medico = await _context.Medicos
			   .Include(m => m.IdTrabajadorNavigation)
			   .ThenInclude(t => t.IdPersonaNavigation)
			   .Include(m => m.IdTrabajadorNavigation.IdHorarioNavigation)
			   .Where(m => m.IdTrabajador == id).FirstOrDefaultAsync();

				if (medico == null)
				{
					return new Result<ViewMedicos> { Model = null, Message = "No se encontro el medico con ese ID de trabajdor.", Status = StatusCodes.Status204NoContent };

				}
				ViewMedicos respuesta = new(
					medico.IdMedico,
					medico.IdTrabajador,
					medico.IdTrabajadorNavigation.IdPersonaNavigation.Nombre + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoMaterno,
					medico.Especialidad,
					medico.Consultorio ?? "",
					medico.Consultorio ?? "",
					medico.Status ?? "",
					medico.Consulta,
					medico.IdTrabajadorNavigation.IdHorarioNavigation.HoraInicio,
					medico.IdTrabajadorNavigation.IdHorarioNavigation.HoraFin
					);


				return new Result<ViewMedicos> { Model = respuesta, Message = "Medico encontrado con exito.", Status = StatusCodes.Status200OK };
			}
			catch (Exception e)
			{

				return new Result<ViewMedicos> { Model = null, Message = "Error al buscar el medico.", Status = StatusCodes.Status500InternalServerError };

			}
		}

		public async Task<Result<ViewListMedicos>> GetMedicos()
		{
			try
			{
				var consulta = await _context.Medicos
					.Include(m => m.IdTrabajadorNavigation)
					.ThenInclude(m => m.IdPersonaNavigation)
					.Include(m => m.IdTrabajadorNavigation.IdHorarioNavigation)
					.ToListAsync();

				if (!consulta.Any())
				{
					return new Result<ViewListMedicos> { Model = null, Message = "No existen medicos", Status = StatusCodes.Status204NoContent };
				}

				List<ViewMedicos> Medicos = new();

				foreach (var cons in consulta)
				{
					Medicos.Add(new ViewMedicos(
						cons.IdMedico,
						cons.IdTrabajador,
						cons.IdTrabajadorNavigation.IdPersonaNavigation.Nombre + " " + cons.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno + " " + cons.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoMaterno,
						cons.Especialidad,
						cons.Consultorio ?? "",
						cons.Cedula ?? "",
						cons.Status ?? "",
						cons.Consulta,
						cons.IdTrabajadorNavigation.IdHorarioNavigation.HoraInicio,
						cons.IdTrabajadorNavigation.IdHorarioNavigation.HoraFin
						));
				}
				ViewListMedicos listaMedicos = new(Medicos);

				return new Result<ViewListMedicos> { Model = listaMedicos, Message = "Medicos encontrados con exito.", Status = StatusCodes.Status200OK };
			}
			catch (Exception)
			{
				return new Result<ViewListMedicos> { Model = null, Message = "Error al recuperar medicos.", Status = StatusCodes.Status500InternalServerError };
			}
		}


		public async Task<Result<ViewMedicos>> UpdateMedico(ViewMedicosUpdate medicoAdd)
		{
			using var transaction = _context.Database.BeginTransaction();
			try
			{
				var medico = await _context.Medicos.FindAsync(medicoAdd.IdMedico);

				if (medico == null)
				{
					return new Result<ViewMedicos> { Model = null, Message = "No existe medico con ese ID.", Status = StatusCodes.Status400BadRequest };
				}

				var citasPorTener = await _context.Citas.Where(cita => cita.IdMedico == medicoAdd.IdMedico && (cita.IdStatus == 1 || cita.IdStatus == 2)).ToListAsync();

				if (medico.Status == "INACTIVO" && citasPorTener != null)
				{
					return new Result<ViewMedicos> { Model = null, Message = "No se puede actualizar a Inactivo a un medico que tenga citas En espera o Aprobadas.", Status = StatusCodes.Status400BadRequest };
				}

				medico.Consultorio = medicoAdd.Consultorio;
				medico.Especialidad = medicoAdd.Especialidad;
				medico.Status = medicoAdd.Status;
				medico.Consulta = medicoAdd.Consulta;
				_context.SaveChanges();
				transaction.Commit();
				var med = await GetMedico(medicoAdd.IdMedico);

				return new Result<ViewMedicos> { Model = med.Model, Message = "Medico actualizado con exito.", Status = StatusCodes.Status200OK };

			}
			catch (Exception)
			{
				transaction.Rollback();
				return new Result<ViewMedicos> { Model = null, Message = "Error al actualizar el medico.", Status = StatusCodes.Status500InternalServerError };

			}
		}


	}
}
