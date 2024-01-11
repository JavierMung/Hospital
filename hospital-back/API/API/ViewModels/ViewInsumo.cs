namespace API.ViewModels
{
	public record ViewInsumoTicket(int IdInsumo, int Cantidad);
	public record ViewInsumoResponse(int IdInsumo, string Nombre, double Costo ,int Cantidad, int Tipo, string NombreTipo, double PreTotal);
}
