namespace API.ViewModels
{
	public class Result<T>
	{
		public T? Model { get; set; }
		public string? Message { get; set; }
		public int Status { get; set; }
	}
	public record ViewTrabajador(int idTrabajador, int idRol, int IdHorario, int IdPersona, DateTime FechaInicio, double Salario, ViewPersona Persona);
	public record ViewAddTrabajador(int idRol, int IdHorario, int IdPersona, DateTime FechaInicio, double Salario, ViewPersona Persona);

}
