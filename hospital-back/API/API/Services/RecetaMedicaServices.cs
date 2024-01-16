using API.Context;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http;

using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class RecetaMedicaServices : IRecetaMedica
	{
		private readonly HospitalContext _context;
		public RecetaMedicaServices(HospitalContext context)
		{
			_context = context;
		}
		public async Task<Result<ViewRecetaMedica>> AddReceta(ViewRecetaMedicaAdd receta)
		{
			using var transaction = _context.Database.BeginTransaction();
			try
			{
				var cita = await _context.Citas.FindAsync(receta.IdCita);
				if (cita == null)
				{
					return new Result<ViewRecetaMedica>()
					{
						Model = null,
						Message = "No existe la cita medica. Revise los datos.",
						Status = StatusCodes.Status400BadRequest
					};
				}


				RecetaMedica nuevaReceta = new RecetaMedica()
				{
					IdCita = receta.IdCita,
					Posologia = receta.Posologia,
				};

				_ = await _context.RecetaMedicas.AddAsync(nuevaReceta);

				await _context.SaveChangesAsync();
				
				transaction.Commit();
				
				var res = await GetRecetaByIdCita(receta.IdCita);
				
				return new Result<ViewRecetaMedica>()
				{
					Model = res.Model,
					Message = "Receta agregada con exito.",
					Status = StatusCodes.Status200OK
				};

			}
			catch (Exception e)
			{
				transaction.Rollback();
				return new Result<ViewRecetaMedica>()
				{
					Model = null,
					Message = "Ocurrio un error al crear la receta."+e,
					Status = StatusCodes.Status500InternalServerError
				};
			}

		}

		public async Task<Result<ViewRecetaMedica>> GetRecetaByIdCita(int id)
		{
			try
			{
				var receta = await _context.RecetaMedicas.Where(cita => cita.IdCita == id).FirstOrDefaultAsync();

				if (receta == null)
				{
					return new Result<ViewRecetaMedica>()
					{
						Model = null,
						Message = "No existe receta medica.",
						Status = 204
					};
				}


				var contextCitas = new CitasServices(_context);

				var cita = await contextCitas.GetCitaById(receta.IdCita);

				if (cita.Model == null)
				{
					return new Result<ViewRecetaMedica>()
					{
						Model = null,
						Message = "No existe cita medica. Revise los datos.",
						Status = StatusCodes.Status400BadRequest
					};
				}

				ViewRecetaMedica recetaRespuesta = new(id, receta.Posologia ?? "", cita.Model);

				return new Result<ViewRecetaMedica>()
				{
					Model = recetaRespuesta,
					Message = "Receta medica encontrada con exito.",
					Status = StatusCodes.Status200OK
				};

			}
			catch (Exception)
			{
				return new Result<ViewRecetaMedica>()
				{
					Model = null,
					Message = "Ocurrio un error al obtener la receta.",
					Status = StatusCodes.Status500InternalServerError
				};
			}
			throw new NotImplementedException();
		 }

		public async Task<Result<ViewRecetaMedica>> UpdateReceta(ViewRecetaMedicaUpdate receta)
		{
			using var transaction = _context.Database.BeginTransaction();
			try
			{
			
				var recetaMedica = await _context.RecetaMedicas.FindAsync(receta.IdReceta);
				if (recetaMedica == null)
				{
					return new Result<ViewRecetaMedica>()
					{
						Model = null,
						Message = "No existe la cita medica. Revise los datos.",
						Status = StatusCodes.Status400BadRequest
					};
				}
				recetaMedica.Posologia = receta.Posologia;

				await _context.SaveChangesAsync();

				transaction.Commit();

				var res = await GetRecetaByIdCita(recetaMedica.IdCita);

				return new Result<ViewRecetaMedica>()
				{
					Model = res.Model,
					Message = "Receta agregada con exito.",
					Status = StatusCodes.Status200OK
				};

			}
			catch (Exception)
			{
				transaction.Rollback();
				return new Result<ViewRecetaMedica>()
				{
					Model = null,
					Message = "Ocurrio un error al crear la receta.",
					Status = StatusCodes.Status500InternalServerError
				};
			}
		}
	}
}
