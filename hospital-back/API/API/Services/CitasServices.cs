using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class CitasServices : ICitasServices
	{
		private readonly HospitalContext _context;
		public CitasServices(HospitalContext context)
		{
			_context = context;
		}
		public async Task<ViewCita?> CreateCita(ViewCita cita)
		{
			using var transaction = _context.Database.BeginTransaction();
			
			try
			{

				PacientesServices spacientes = new(_context);
				var paciente = await spacientes.CreatePaciente(cita.paciente);

				if (paciente == null) return null;


				Cita citaInsert = new()
				{
					IdMedico = cita.medico.idMedico,
					IdPaciente = paciente.Id,
					IdServicio = cita.idServicio,
					Fecha = cita.FechaCita,
					IdStatus = 1
				};

				await _context.Citas.AddAsync(citaInsert);
				await _context.SaveChangesAsync();

				var citaIdCitaCreada = await _context.Citas.OrderByDescending(p => p.IdCita).FirstOrDefaultAsync();

				if (citaIdCitaCreada == null) return null;

				var id = citaIdCitaCreada.IdCita;

				var respuesta = await GetCitaById(id);

				if (respuesta == null) return null;

				await transaction.CommitAsync();

				return respuesta;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw new Exception("Error al crear la cita: " + ex.Message.ToString()); ;
			}
		}

		public Task<ViewCita?> DeleteCita(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ViewCita?> GetCitaById(int id)
		{
			try
			{

				var cita = await _context.Citas
					.Include(c => c.IdPacienteNavigation)
					.Include(c => c.IdMedicoNavigation)
					.ThenInclude(p => p.IdTrabajadorNavigation)
					.ThenInclude(t => t.IdPersonaNavigation)
					.Include(t => t.IdStatusNavigation)
					.Include(c => c.IdServicioNavigation)

					.Where(p => p.IdCita == id)
					.FirstOrDefaultAsync();

				if (cita == null) return null;

				ViewCita citaRespuesta = new(
					id: cita.IdCita,
					idServicio: cita.IdServicio,
					Status: cita.IdStatusNavigation.Status1,
					FechaAlta: DateTime.Now,
					FechaCita: cita.Fecha,
					costo: cita.IdServicioNavigation.Costo,
					paciente: new ViewPaciente
					(
						id = cita.IdPacienteNavigation.IdPaciente,
						Nombre: cita.IdPacienteNavigation.Nombre,
						Apellido_Materno: cita.IdPacienteNavigation.ApellidoMaterno,
						Apellido_Paterno: cita.IdPacienteNavigation.ApellidoPaterno,
						Edad: cita.IdPacienteNavigation.Edad
					),
					medico: new ViewMedicos
					(
						idMedico: cita.IdMedicoNavigation.IdMedico,
						Nombre: cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.Nombre
						+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno
						+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno,
						Especialidad: cita.IdMedicoNavigation.Especialidad
					)

				);
				return citaRespuesta;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al recuperark la cita: " + ex.Message.ToString()); ;
			}
		}


		public async Task<List<ViewCita?>?> GetCitasByMedicoId(int Id)
		{
			try
			{
				var listaCita = await _context.Citas
					.Include(c => c.IdPacienteNavigation)
					.Include(c => c.IdMedicoNavigation)
						.ThenInclude(p => p.IdTrabajadorNavigation)
						.ThenInclude(t => t.IdPersonaNavigation)
					.Include(c => c.IdStatusNavigation)
					.Include(c => c.IdServicioNavigation)
					.Where(p => p.IdMedico == Id)
					.ToListAsync();


				if (listaCita == null) return null;

				List<ViewCita?> listaDeCitasRespuesta = new();

				foreach (var cita in listaCita)
				{
					ViewCita citaRespuesta = new
						(
						id: cita.IdCita,
						idServicio: cita.IdServicio,
						FechaAlta: DateTime.Now,
						FechaCita: cita.Fecha,
						Status: cita.IdStatusNavigation.Status1,
						costo: cita.IdServicioNavigation.Costo,
						paciente: new ViewPaciente
						(
							Id: cita.IdPacienteNavigation.IdPaciente,
							Nombre: cita.IdPacienteNavigation.Nombre,
							Apellido_Materno: cita.IdPacienteNavigation.ApellidoMaterno,
							Apellido_Paterno: cita.IdPacienteNavigation.ApellidoPaterno,
							Edad: cita.IdPacienteNavigation.Edad
						),
						medico: new ViewMedicos
						(
							idMedico: cita.IdMedicoNavigation.IdMedico,
							Nombre: cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.Nombre
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno,
							Especialidad: cita.IdMedicoNavigation.Especialidad
						)

					);

					listaDeCitasRespuesta.Add(citaRespuesta);
				}
				return listaDeCitasRespuesta;

			}
			catch (Exception ex)
			{
				throw new Exception("Error al recuperar las citas: " + ex.Message.ToString());
			}
		}

		public async Task<List<ViewCita?>?> GetCitasByName(ViewCita citaP)
		{
			try
			{
				var listaCita = await _context.Citas
					.Include(c => c.IdPacienteNavigation)
					.Include(c => c.IdMedicoNavigation)
						.ThenInclude(p => p.IdTrabajadorNavigation)
						.ThenInclude(t => t.IdPersonaNavigation)
					.Include(c => c.IdStatusNavigation)
					.Include(c => c.IdServicioNavigation)
					.Where(c => (c.IdPacienteNavigation.Nombre + c.IdPacienteNavigation.ApellidoPaterno + c.IdPacienteNavigation.ApellidoMaterno) == (citaP.paciente.Nombre + citaP.paciente.Apellido_Paterno + citaP.paciente.Apellido_Paterno))
					.ToListAsync();


				if (listaCita == null) return null;

				List<ViewCita?> listaDeCitasRespuesta = new();

				foreach (var cita in listaCita)
				{
					ViewCita citaRespuesta = new
						(
						id: cita.IdCita,
						idServicio: cita.IdServicio,
						FechaAlta: DateTime.Now,
						FechaCita: cita.Fecha,
						Status: cita.IdStatusNavigation.Status1,
						costo: cita.IdServicioNavigation.Costo,
						paciente: new ViewPaciente
						(
							Id: cita.IdPacienteNavigation.IdPaciente,
							Nombre: cita.IdPacienteNavigation.Nombre,
							Apellido_Materno: cita.IdPacienteNavigation.ApellidoMaterno,
							Apellido_Paterno: cita.IdPacienteNavigation.ApellidoPaterno,
							Edad: cita.IdPacienteNavigation.Edad
						),
						medico: new ViewMedicos
						(
							idMedico: cita.IdMedicoNavigation.IdMedico,
							Nombre: cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.Nombre
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno,
							Especialidad: cita.IdMedicoNavigation.Especialidad
						)

					);

					listaDeCitasRespuesta.Add(citaRespuesta);
				}
				return listaDeCitasRespuesta;

			}
			catch (Exception ex)
			{
				throw new Exception("Error al recuperar las citas: " + ex.Message.ToString());
			}

		}

		public async Task<ViewCita?> UpdateCita(ViewCita cita)
		{
			using var transaction = _context.Database.BeginTransaction();

			try
			{
				var citaParaActualizar = await _context.Citas
						.Include(c => c.IdPacienteNavigation)
						.Include(c => c.IdMedicoNavigation)
						.ThenInclude(p => p.IdTrabajadorNavigation)
						.ThenInclude(t => t.IdPersonaNavigation)
						.Include(t => t.IdStatusNavigation)
						.Include(c => c.IdServicioNavigation)

						.Where(p => p.IdCita == cita.id)
						.FirstOrDefaultAsync();


				if (citaParaActualizar == null) return null;

				citaParaActualizar.IdPacienteNavigation.Edad = cita.paciente.Edad;
				citaParaActualizar.IdPacienteNavigation.Nombre = cita.paciente.Nombre;
				citaParaActualizar.IdPacienteNavigation.ApellidoPaterno = cita.paciente.Apellido_Paterno;
				citaParaActualizar.IdPacienteNavigation.ApellidoMaterno = cita.paciente.Apellido_Materno ?? "";
				citaParaActualizar.Fecha = cita.FechaCita;

				citaParaActualizar.IdStatus = cita.Status switch
				{
					"En espera" => 1,
					"Aprobada" => 2,
					"Cancelada" => 3,
					"Concluida" => 4,
					"Reprograma" => 5,
					_ => throw new Exception("Error al actualizar es Status"),
				};

				var respuesta = await GetCitaById(cita.id);
				await transaction.CommitAsync();

				return respuesta;

			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_context.ChangeTracker.Clear();
				throw new Exception("Error al actualizar la cita: " + ex.Message.ToString());

			}

		}
	}
}
