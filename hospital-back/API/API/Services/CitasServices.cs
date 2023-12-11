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
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{

					PacientesServices spacientes = new PacientesServices(_context);
					var paciente = await spacientes.CreatePaciente(cita.paciente);

					if (paciente == null)
					{
						throw new Exception("Error en la creacion del paciente");
					}


					Cita citaInsert = new Cita();
					citaInsert.IdMedico = cita.medico.idMedico;
					citaInsert.IdPaciente = paciente.Id;
					citaInsert.IdServicio = cita.idServicio;
					citaInsert.Fecha = cita.FechaCita;
					citaInsert.IdStatus = 1;

					await _context.Citas.AddAsync(citaInsert);
					await _context.SaveChangesAsync();

					var citaIdCitaCreada = await _context.Citas.OrderByDescending(p => p.IdCita).FirstOrDefaultAsync();

					var id = citaIdCitaCreada?.IdCita ?? -1;

					if (id == -1)
					{
						throw new Exception("Error al recuperar la cita");
					}

					var respuesta = await GetCitaById(id);
					await transaction.CommitAsync();

					return respuesta;
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					throw new Exception("Error al crear la cita: " + ex.Message.ToString()); ;
				}
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

				ViewCita citaRespuesta = new ViewCita
				(
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

				List<ViewCita?> listaDeCitasRespuesta = new List<ViewCita?>();

				foreach (var cita in listaCita)
				{
					ViewCita citaRespuesta = new ViewCita
					(
						id: cita.IdCita,
						idServicio: cita.IdServicio,
						FechaAlta: DateTime.Now,
						FechaCita: cita.Fecha,
						Status: cita.IdStatusNavigation.Status1,
						costo: cita.IdServicioNavigation.Costo,
						paciente: new ViewPaciente
						(
							Id = cita.IdPacienteNavigation.IdPaciente,
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

		public Task<List<ViewCita?>?> GetCitasByName(ViewCita cita)
		{
			throw new NotImplementedException();
		}

		public async Task<ViewCita?> UpdateCita(ViewCita cita)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{

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


					if (citaParaActualizar == null)
						throw new Exception("No se encontro el id de la cita ");

					citaParaActualizar.IdPacienteNavigation.Edad = cita.paciente.Edad;
					citaParaActualizar.IdPacienteNavigation.Nombre = cita.paciente.Nombre;
					citaParaActualizar.IdPacienteNavigation.ApellidoPaterno = cita.paciente.Apellido_Paterno;
					citaParaActualizar.IdPacienteNavigation.ApellidoMaterno = cita.paciente.Apellido_Materno ?? "";
					citaParaActualizar.Fecha = cita.FechaCita;

					switch (cita.Status)
					{

						case "En espera": citaParaActualizar.IdStatus = 1; break;
						case "Aprobada": citaParaActualizar.IdStatus = 2; break;
						case "Cancelada": citaParaActualizar.IdStatus = 3; break;
						case "Concluida": citaParaActualizar.IdStatus = 4; break;
						case "Reprograma": citaParaActualizar.IdStatus = 5; break;
						default: throw new Exception("Error al actualizar es Status");
					}


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
}
