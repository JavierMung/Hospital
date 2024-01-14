using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class ServiciosServices : IServiciosServices
	{
		private readonly HospitalContext _context;
		public ServiciosServices(HospitalContext context)
		{
			_context = context;
		}

		public async Task<Result<List<ViewServicio>>> GetAllServicios()
		{

			try
			{
				var consulta = await _context.Servicios
							.ToListAsync();

				if (consulta == null)
				{
					return new Result<List<ViewServicio>> { Model = null, Message = "No hay servicios para mostrar.", Status = StatusCodes.Status204NoContent };
				}


				List<ViewServicio> listServicios = new();

				foreach (var cons in consulta)
				{
					listServicios.Add(new ViewServicio(cons.IdServicio, cons.Servicio1, cons.Costo));
				}
				return new Result<List<ViewServicio>>
				{
					Model = listServicios,
					Message = "Servicios recuperados con exito.",
					Status = StatusCodes.Status200OK
				};
			}
			catch (Exception)
			{
				return new Result<List<ViewServicio>>
				{
					Model = null,
					Message = "Error al recuperar los servicios.",
					Status = StatusCodes.Status500InternalServerError
				};
			}
		}

		public async Task<Result<ViewServicio>> GetServicio(int idServicio)
		{
			var consulta = await _context.Servicios.FindAsync(idServicio);
			if (consulta == null)
			{
				return new Result<ViewServicio> { Model = null, Message = "No se encontro servicio cn ese ID.", Status = StatusCodes.Status204NoContent };

			}
			ViewServicio servicio = new ViewServicio(consulta.IdServicio, consulta.Servicio1, consulta.Costo);

			return new Result<ViewServicio>
			{
				Model = servicio,
				Message = "Servicio recuperado con exito.",
				Status = StatusCodes.Status200OK
			};
		}

		public async Task<Result<int>> DeleteServicio(int idServicio)
		{
			try
			{
				var servicio = await _context.Servicios.FindAsync(idServicio);
				if (servicio == null)
				{
					return new Result<int>
					{
						Model = 0,
						Message = "No existe el servicio con ese ID.",
						Status = StatusCodes.Status204NoContent
					};

				}

				_context.Servicios.Remove(servicio);
				await _context.SaveChangesAsync();

				return new Result<int>
				{
					Model = 1,
					Message = "Servicio borrado con exito.",
					Status = StatusCodes.Status200OK
				};
			}
			catch (Exception e)
			{
				return new Result<int>
				{
					Model = 0,
					Message = "Error al borrar el servicio.",
					Status = StatusCodes.Status500InternalServerError
				};
			}
		}

		public async Task<Result<ViewServicio>> AddServicio(ViewServicioAdd model)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var nuevoServicio = new Servicio
					{
						Servicio1 = model.servicio,
						Costo = model.costo
					};
					_context.Servicios.Add(nuevoServicio);
					await _context.SaveChangesAsync();

					var servicioIdServicioCreado = await _context.Servicios.OrderByDescending(p => p.IdServicio).FirstOrDefaultAsync();

					var id = servicioIdServicioCreado?.IdServicio ?? -1;

					if (id == -1)
					{
						throw new Exception("Error al crear el servicio.");
					}

					var respuesta = await GetServicio(id) ?? throw new Exception("Error al crear el servicio.");

					await transaction.CommitAsync();

					return new Result<ViewServicio>
					{
						Model = respuesta.Model,
						Message = "Servicio creado con exito.",
						Status = StatusCodes.Status200OK
					};
				}
				catch (Exception e)
				{
					await transaction.RollbackAsync();
					return new Result<ViewServicio>
					{
						Model = null,
						Message = "Error al crear el servicio.",
						Status = StatusCodes.Status500InternalServerError
					};
				}
			}
		}

		public async Task<Result<ViewServicio>> UpdateServicio(ViewServicio model)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var servicio = await _context.Servicios.FindAsync(model.IdServicio);
					if (servicio == null)
					{
						return new Result<ViewServicio>
						{
							Model = null,
							Message = "No se encontro servicio con ese ID.",
							Status = StatusCodes.Status500InternalServerError
						};
					}

					servicio.Servicio1 = model.Servicio;
					servicio.Costo = model.Costo;

					_context.Servicios.Update(servicio);
					await _context.SaveChangesAsync();


					await transaction.CommitAsync();

					return new Result<ViewServicio>
					{
						Model = model,
						Message = "Servicio actualizado con exito.",
						Status = StatusCodes.Status200OK
					}; ;
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					return new Result<ViewServicio>
					{
						Model = null,
						Message = "Error al actualizar el servicio.",
						Status = StatusCodes.Status500InternalServerError
					};
				}
			}

		}


	}
}
