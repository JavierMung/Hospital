namespace API.ViewModels
{
	public record ViewInsumoTicket(int IdInsumo, int Cantidad);
	public record ViewInsumoResponse(int IdInsumo, string Nombre, int Cantidad, int Tipo, string NombreTipo);
}
