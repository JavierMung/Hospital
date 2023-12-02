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
