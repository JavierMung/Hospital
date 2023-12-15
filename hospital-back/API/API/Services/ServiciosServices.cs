using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class ServiciosServices : IServiciosServices
	{
		private readonly HospitalContext _context;
		public ServiciosServices (HospitalContext context)
		{
			_context = context;
		}

		public async Task<List<ViewServicio>?> GetAllServicios()
		{
			var consulta = await _context.Servicios
							.ToListAsync();

			if (consulta == null) return null;

			List<ViewServicio> listServicios = new List<ViewServicio>();

			foreach (var cons in consulta)
			{
				listServicios.Add(new ViewServicio(cons.IdServicio, cons.Servicio1, cons.Costo));
			}
			return listServicios;
		}

		public async Task<ViewServicio?> GetServicio(int idServicio)
		{
			var consulta = await _context.Servicios.Where(ser => ser.IdServicio == idServicio).FirstOrDefaultAsync();
			if (consulta == null) return null;
			ViewServicio servicio = new ViewServicio(consulta.IdServicio, consulta.Servicio1, consulta.Costo);
			return servicio;	
		}

        public async Task<int> DeleteServicio(int idServicio)
        {
            try
            {
                var servicio = await _context.Servicios.FindAsync(idServicio);
                if (servicio == null)
                {
                    return 0;
                }
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception e)
            {
                return 2;
            }
        }

        public async Task<ViewServicio?> AddServicio(ViewServicio model)
        {
			using (var transaction = _context.Database.BeginTransaction())
			{ 
					try
				{
					//Creamos el objeto a crear
					var nuevoServicio = new Servicio
					{
						Servicio1 = model.servicio,
						Costo = model.costo
					};
					//insertamos el objeto en la db
					_context.Servicios.Add(nuevoServicio);
					await _context.SaveChangesAsync();

                    var servicioIdServicioCreado = await _context.Servicios.OrderByDescending(p => p.IdServicio).FirstOrDefaultAsync();

                    var id = servicioIdServicioCreado?.IdServicio ?? -1;

                    if (id == -1)
                    {
                        throw new Exception("Error al recuperar el servicio");
                    }

                    var respuesta = await GetServicio(id);

                    await transaction.CommitAsync();

					return respuesta;
                }
				catch (Exception e)
				{
                    await transaction.RollbackAsync();
                    throw new Exception("Error al crear la cita: " + e.Message.ToString()); ;
                }
			}
        }

		public async Task<ViewServicio?> UpdateServicio(ViewServicio model)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{ 
					try
				{
					var servicio = await _context.Servicios.FindAsync(model.idServicio);
					if (servicio == null)
					{
						throw new Exception("No se encontró el servicio con id:" + model.idServicio.ToString());
					}

					servicio.Servicio1 = model.servicio;
					servicio.Costo = model.costo;

					_context.Servicios.Update(servicio);
					await _context.SaveChangesAsync();


                    await transaction.CommitAsync();

                    return model;
				}
				catch (Exception e)
				{
                    await transaction.RollbackAsync();
                    throw new Exception("Error al actualizar el servicio: " + e.Message.ToString()); ;
                }
			}

        }

        public async Task<List<ViewServicio?>?> GetServicios(string servicio)
		{
			try
			{
				var consulta = await _context.Trabajadors
								.Include(t => t.IdPersonaNavigation) // Carga la propiedad de navegación Persona
								.Include(t => t.TrabajadorServicios) // Carga la propiedad de navegación TrabajadorServicios
									.ThenInclude(ts => ts.IdServicioNavigation) // Carga la propiedad de navegación Servicio dentro de TrabajadorServicios
								.Where(data => data.TrabajadorServicios.Any(ts => ts.IdServicioNavigation.Servicio1 == servicio))
								.Select(t => new
								{
									t.IdTrabajador,
									Persona = new
									{
										t.IdPersonaNavigation.Nombre,
										t.IdPersonaNavigation.ApellidoPaterno,
										t.IdPersonaNavigation.ApellidoMaterno,
									},
									Servicios = t.TrabajadorServicios
									.Where(ts => ts.IdServicioNavigation.Servicio1 == servicio)
									.Select(ts => new
									{
										ts.IdServicioNavigation.Servicio1,
										ts.IdServicioNavigation.Costo
									})
								}).ToListAsync();

				if (consulta.Count <= 0) return null;	

				List <ViewServicio> servicios = new List<ViewServicio>();


				Console.WriteLine(consulta.ToString());
				return null;
			}
			catch (Exception e)
			{
				return null; 
			}
		}

	}
}
