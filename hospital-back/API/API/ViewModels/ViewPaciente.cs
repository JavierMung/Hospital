namespace API.ViewModels
{
	public record ViewPaciente (int Id, string Nombre,  string Apellido_Paterno, string? Apellido_Materno,int Edad, string CURP);
}
