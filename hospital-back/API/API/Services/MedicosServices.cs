using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Interfaces;
using API.ViewModels;

namespace API.Services
{
	public class MedicosServices : IMedicosServices
	{
		public readonly HospitalContext _context;

		public MedicosServices(HospitalContext context)
		{
			_context = context;
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
					medico.IdTrabajadorNavigation.IdPersonaNavigation.Nombre + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoMaterno,
					medico.Especialidad,
					medico.Consultorio ?? "",
					medico.Status ?? "",
					medico.IdTrabajadorNavigation.IdHorarioNavigation.HoraInicio,
					medico.IdTrabajadorNavigation.IdHorarioNavigation.HoraFin
					);


				return new Result<ViewMedicos> { Model = respuesta, Message = "Medico encontrado con exito.", Status = StatusCodes.Status200OK };
			}
			catch (Exception)
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
						cons.IdTrabajadorNavigation.IdPersonaNavigation.Nombre + " " + cons.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno + " " + cons.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoMaterno,
						cons.Especialidad,
						cons.Consultorio ?? "",
						cons.Status ?? "",
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


		public async Task<Result<ViewMedicosAdd>> UpdateMedico(ViewMedicosAdd medicoAdd)
		{
			try
			{
				var medico = await _context.Medicos.FindAsync(medicoAdd.idMedico);

				if (medico == null)
				{
					return new Result<ViewMedicosAdd> { Model = null, Message = "No existe medico con ese ID.", Status = StatusCodes.Status204NoContent };
				}
				medico.Consultorio = medicoAdd.Consultorio;
				medico.Especialidad = medicoAdd.Especialidad;
				medico.Status = medicoAdd.Status;
				_context.SaveChanges();

				return new Result<ViewMedicosAdd> { Model = medicoAdd, Message = "Medico actualizado con exito.", Status = StatusCodes.Status200OK };

			}
			catch (Exception)
			{
				return new Result<ViewMedicosAdd> { Model = null, Message = "Error al actualizar el medico.", Status = StatusCodes.Status500InternalServerError };

			}
		}


	}
}
