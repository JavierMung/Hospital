
namespace API.ViewModels
{
	public record ViewCita (int id, DateTime FechaAlta, DateTime FechaCita, ViewPaciente paciente, ViewMedicos medico, Double costo, int idServicio, string Status);
}
