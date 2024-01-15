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
		public async Task<Result<ViewCita>> CreateCita(ViewCitaAdd cita)
		{

			var servicioMedicos = new MedicosServices(_context);

			var medico = await servicioMedicos.GetMedico(cita.IdMedico);

			if (medico.Model == null)
			{
				return new Result<ViewCita> { Model = null, Message = medico.Message, Status = medico.Status };

			}

			if (cita.FechaCita.Hour < medico.Model.HorarInicio.Hours || cita.FechaCita.Hour >= medico.Model.HoraFin.Hours)
			{
				return new Result<ViewCita> { Model = null, Message = "La cita no es posible en ese horario. Elige otro.", Status = 400 };
			}


			if (!await ValidarFecha(cita.FechaCita, cita.IdMedico))
			{
				return new Result<ViewCita> { Model = null, Message = "La cita no es posible en ese horario. Elige otro.", Status = 400 };
			}

			using var transaction = _context.Database.BeginTransaction();

			try
			{

				PacientesServices spacientes = new(_context);
				var paciente = await spacientes.CreatePaciente(cita.paciente);

				if (paciente.Model == null)
				{
					return new Result<ViewCita>
					{
						Model = null,
						Message = paciente.Message,
						Status = paciente.Status

					};
				}

				DateTime nuevaFechaHora = new DateTime(cita.FechaCita.Year, cita.FechaCita.Month, cita.FechaCita.Day, cita.FechaCita.Hour, 0, 0);

				Cita citaInsert = new()
				{
					IdMedico = cita.IdMedico,
					IdPaciente = paciente.Model.Id,
					IdServicio = cita.idServicio,
					Fecha = nuevaFechaHora,
					IdStatus = 1
				};

				await _context.Citas.AddAsync(citaInsert);
				await _context.SaveChangesAsync();


				var respuesta = await GetCitaById(citaInsert.IdCita);

				if (respuesta == null)
				{
					throw new Exception();
				}


				await transaction.CommitAsync();

				return new Result<ViewCita>
				{
					Model = respuesta.Model,
					Message = "Cita creada con exito.",
					Status = 200

				}; ;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				return new Result<ViewCita>
				{
					Model = null,
					Message = "Error al crear la cita.",
					Status = 500

				};
			}
		}

		public Task<Result<ViewCita>> DeleteCita(int id)
		{
			throw new NotImplementedException();
		}


		public async Task<Result<ViewCita>> GetCitaById(int id)
		{
			try
			{

				var cita = await _context.Citas
					.Include(c => c.IdPacienteNavigation)
					.Include(c => c.IdMedicoNavigation)
					.ThenInclude(p => p.IdTrabajadorNavigation)
					.ThenInclude(t => t.IdPersonaNavigation)
					.Include(c => c.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation)
					.Include(t => t.IdStatusNavigation)
					.Include(c => c.IdServicioNavigation)
					.Include(c => c.IdServicioNavigation)

					.Where(p => p.IdCita == id)
					.FirstOrDefaultAsync();

				if (cita == null)
				{
					return new Result<ViewCita>
					{
						Model = null,
						Message = "No se pudo recuperar la cita, verifique ID o busque por CURP del paciente.",
						Status = 204
					};
				};

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
						Edad: cita.IdPacienteNavigation.Edad,
						CURP: cita.IdPacienteNavigation.Curp
					),
					medico: new ViewMedicos
					(
						IdMedico: cita.IdMedicoNavigation.IdMedico,
							IdTrabajador: cita.IdMedicoNavigation.IdTrabajador,
							Nombre: cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.Nombre
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno,
							Especialidad: cita.IdMedicoNavigation.Especialidad,
							Consultorio: cita.IdMedicoNavigation.Consultorio ?? "",
							Cedula: cita.IdMedicoNavigation.Cedula ?? "",
							Status: cita.IdMedicoNavigation.Status ?? "",
							Consulta: cita.IdMedicoNavigation.Consulta,
							cita.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation.HoraInicio,
							cita.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation.HoraFin
					)

				);
				return new Result<ViewCita>
				{
					Model = citaRespuesta,
					Message = "Cita recuperada con exito",
					Status = 200

				};
			}
			catch (Exception ex)
			{
				return new Result<ViewCita>
				{
					Model = null,
					Message = "Error al recuperar la cita.",
					Status = 500

				};
			}
		}


		public async Task<Result<List<ViewCita>>> GetCitasByMedicoId(int Id)
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
					.Include(c => c.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation)
					.Where(p => p.IdMedico == Id)
					.ToListAsync();


				if (listaCita == null || listaCita.Count <= 0)
				{
					return new Result<List<ViewCita>>
					{
						Model = null,
						Message = $"No exiten citas para el medico con ID : {Id}.",
						Status = 204

					};
				};

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
							Edad: cita.IdPacienteNavigation.Edad,
							CURP: cita.IdPacienteNavigation.Curp
						),
						medico: new ViewMedicos
						(
							IdMedico: cita.IdMedicoNavigation.IdMedico,
							IdTrabajador: cita.IdMedicoNavigation.IdTrabajador,
							Nombre: cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.Nombre
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno,
							Especialidad: cita.IdMedicoNavigation.Especialidad,
							Consultorio: cita.IdMedicoNavigation.Consultorio ?? "",
							Cedula: cita.IdMedicoNavigation.Cedula ?? "",
							Status: cita.IdMedicoNavigation.Status ?? "",
							Consulta: cita.IdMedicoNavigation.Consulta,
							cita.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation.HoraInicio,
							cita.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation.HoraFin
						)

					);

					listaDeCitasRespuesta.Add(citaRespuesta);
				}
				return new Result<List<ViewCita>>
				{
					Model = listaDeCitasRespuesta,
					Message = "Citas recuperada con exito",
					Status = 200

				};

			}
			catch (Exception ex)
			{
				return new Result<List<ViewCita>>
				{
					Model = null,
					Message = "Error al recuperar las citas.",
					Status = 500

				};

			}
		}

		public async Task<Result<List<ViewCita>>> GetCitasByCURP(string CURP)
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
					.Include(c => c.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation)
					.Where(c => c.IdPacienteNavigation.Curp == CURP)
					.ToListAsync();


				if (listaCita == null || listaCita.Count <= 0)
				{
					return new Result<List<ViewCita>>
					{
						Model = null,
						Message = $"No exiten citas para el medico para el paciente con CURP : {CURP}.",
						Status = 204

					};
				};
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
							Edad: cita.IdPacienteNavigation.Edad,
							CURP: cita.IdPacienteNavigation.Curp

						),
						medico: new ViewMedicos
						(
							IdMedico: cita.IdMedicoNavigation.IdMedico,
							IdTrabajador: cita.IdMedicoNavigation.IdTrabajador,
							Nombre: cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.Nombre
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno
							+ " " + cita.IdMedicoNavigation.IdTrabajadorNavigation.IdPersonaNavigation.ApellidoPaterno,
							Especialidad: cita.IdMedicoNavigation.Especialidad,
							Consultorio: cita.IdMedicoNavigation.Consultorio ?? "",
							Cedula: cita.IdMedicoNavigation.Cedula ?? "",
							Status: cita.IdMedicoNavigation.Status ?? "",
							Consulta: cita.IdMedicoNavigation.Consulta,
							cita.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation.HoraInicio,
							cita.IdMedicoNavigation.IdTrabajadorNavigation.IdHorarioNavigation.HoraFin
						)

					);

					listaDeCitasRespuesta.Add(citaRespuesta);
				}

				return new Result<List<ViewCita>>
				{
					Model = listaDeCitasRespuesta,
					Message = "Citas recuperadas con exito",
					Status = 200

				};

			}
			catch (Exception ex)
			{
				return new Result<List<ViewCita>>
				{
					Model = null,
					Message = "Error al recuperar las citas.",
					Status = 500

				}; ;
			}

		}

		public async Task<Result<ViewCita>> UpdateCita(ViewCitaAdd cita)
		{
			var servicioMedicos = new MedicosServices(_context);

			var medico = await servicioMedicos.GetMedico(cita.IdMedico);

			if (medico.Model == null)
			{
				return new Result<ViewCita> { Model = null, Message = medico.Message, Status = medico.Status };

			}

			if (cita.FechaCita.Hour < medico.Model.HorarInicio.Hours || cita.FechaCita.Hour >= medico.Model.HoraFin.Hours)
			{
				return new Result<ViewCita> { Model = null, Message = "La cita no es posible en ese horario. Elige otro.", Status = 400 };
			}


			if (!await ValidarFecha(cita.FechaCita, cita.IdMedico))
			{
				return new Result<ViewCita> { Model = null, Message = "La cita no es posible en ese horario. Elige otro.", Status = 400 };
			}

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

				if (citaParaActualizar == null)
				{
					return new Result<ViewCita>
					{
						Model = null,
						Message = "No existe cita con ese ID.",
						Status = 400

					};
				}

				if (!await ValidarFecha(cita.FechaCita, cita.IdMedico))
				{
					return new Result<ViewCita> { Model = null, Message = "La cita no es posible en ese horario. Elige otro.", Status = 400 };
				}

				if (cita.Status == "Cancelada" && citaParaActualizar.Fecha.Date == DateTime.Now.Date && DateTime.Now.Hour > citaParaActualizar.Fecha.AddHours(-1).Hour)
				{
					return new Result<ViewCita>
					{
						Model = null,
						Message = "Para cancelar deberá ser al menos 24 horas antes de la cita.",
						Status = 400

					};
				}



				citaParaActualizar.IdPacienteNavigation.Edad = cita.paciente.Edad;
				citaParaActualizar.IdPacienteNavigation.Nombre = cita.paciente.Nombre;
				citaParaActualizar.IdPacienteNavigation.ApellidoPaterno = cita.paciente.Apellido_Paterno;
				citaParaActualizar.IdPacienteNavigation.ApellidoMaterno = cita.paciente.Apellido_Materno ?? "";
				DateTime nuevaFechaHora = new(cita.FechaCita.Year, cita.FechaCita.Month, cita.FechaCita.Day, cita.FechaCita.Hour, 0, 0);

				citaParaActualizar.Fecha = nuevaFechaHora;

				citaParaActualizar.IdStatus = cita.Status switch
				{
					"En espera" => 1,
					"Aprobada" => 2,
					"Cancelada" => 3,
					"Concluida" => 4,
					"Reprograma" => 5,
					_ => throw new Exception("Error al actualizar el Status."),
				};

				await _context.SaveChangesAsync();

				var respuesta = await GetCitaById(cita.id);
				await transaction.CommitAsync();

				return new Result<ViewCita>
				{
					Model = respuesta.Model,
					Message = "Cita actualizada con exito.",
					Status = 200

				}; ;

			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				_context.ChangeTracker.Clear();

				return new Result<ViewCita>
				{
					Model = null,
					Message = "Error al actualizar la cita.",
					Status = 505

				};
			}

		}

		private async Task<bool> ValidarFecha(DateTime fecha, int IdMedico)
		{


			var citas = await GetCitasByMedicoId(IdMedico);

			if (citas.Model != null)
			{

				foreach (var cta in citas.Model)
				{
					if ((cta.FechaCita.Date == fecha.Date && cta.FechaCita.Hour == fecha.Hour) && (cta.Status == "Aprobada" || cta.Status == "En espera"))
					{
						return false;
					}
				}
			}

			return true;
		}

		/*private async List<DateTime> HorariosDisponibles()
		{
			var list = new List<DateTime>();

			var servicioMedicos = new MedicosServices(_context);

			//await servicioMedicos.GetMedicoDisponibilidad();

			return list;
		}*/
	}
}
