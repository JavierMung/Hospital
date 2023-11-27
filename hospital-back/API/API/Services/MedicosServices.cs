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
			}catch(Exception e) 
			{ 
				Console.WriteLine(e.ToString());
				return null; 
			}
		}

	}
}
