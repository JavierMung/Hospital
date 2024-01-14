namespace API.ViewModels
{
	public record ViewServicio (int IdServicio, string Servicio, double Costo);
	public record ViewServicioAdd(string servicio, double costo);
	public record ViewServicioTicket(int IdServicio, string Servicio, double Costo ,int Cantidad ,  double PreTotal);
	public record ViewServicioTicketAdd(int IdServicio, int Cantidad);


}
