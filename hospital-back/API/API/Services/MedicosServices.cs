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



		public async Task<ViewMedicos?> GetMedico(int id)
		{
			try
			{
				var medico = await _context.Medicos
				.Include(m => m.IdTrabajadorNavigation)
				.ThenInclude(t => t.IdPersonaNavigation)
				.Where(m => m.IdMedico == id).FirstOrDefaultAsync();

				if (medico == null) return null;
				ViewMedicos respuesta = new(medico.IdMedico, medico.IdTrabajadorNavigation.IdPersonaNavigation.Nombre + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno + " " + medico.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoMaterno, medico.Especialidad);
				return respuesta;
			}
			catch (Exception e)
			{
				return null; // new Exception("Error");
							 //Console.WriteLine(e.ToString());
			}
		}

		public async Task<ViewListMedicos?> GetMedicos()
		{
			try
			{
				var consulta = await _context.Medicos
					.Include(m => m.IdTrabajadorNavigation)
					.ThenInclude(m => m.IdPersonaNavigation)
					.ToListAsync();

				if (!consulta.Any()) return null;

				List<ViewMedicos> Medicos = new List<ViewMedicos>();

				foreach (var cons in consulta)
				{
					Medicos.Add(new ViewMedicos(cons.IdMedico, cons.IdTrabajadorNavigation.IdPersonaNavigation.Nombre + " " + cons.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno + " " + cons.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoMaterno, cons.Especialidad));
				}

				ViewListMedicos listaMedicos = new ViewListMedicos(Medicos);

				return listaMedicos;
			}
			catch (Exception e)
			{
				return null;
			}
		}

	}
}
