namespace API.ViewModels
{
	public record ViewServicio (int idServicio, string servicio, double costo );
	public record ViewServicioAdd(string servicio, double costo);
	public record ViewServicioTicket(string IdServicio, int Cantidad);

}
