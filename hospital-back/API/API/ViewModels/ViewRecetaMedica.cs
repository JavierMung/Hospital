namespace API.ViewModels
{
	public record ViewRecetaMedica(int IdRecetaMedica, string Posologia, ViewCita Cita);
	public record ViewRecetaMedicaAdd(int IdCita, string Posologia);
	public record ViewRecetaMedicaUpdate(int IdReceta, string Posologia);
}
