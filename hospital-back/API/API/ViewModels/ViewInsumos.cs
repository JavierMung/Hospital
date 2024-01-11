namespace API.ViewModels
{
	public record ViewInsumoAdd(string Nombre, double Precio, int Tipo, int Stock);
	public record ViewInsumoUpdate(int IdInsumo, int IdTipo ,string Nombre, double Precio,  int Stock);
	public record ViewInsumo(int IdInsumo,string Nombre, double Precio,string TipoNombre ,int Tipo, int Stock);
}
