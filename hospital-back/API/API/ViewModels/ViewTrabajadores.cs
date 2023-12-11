namespace API.ViewModels
{
    public record ViewTrabajadores(int idTrabajador, string Nombre, string Rol, double Salario);
    public record ViewTrabajador( int idTrabajador, int idRol, int IdHorario, int IdPersona,  DateTime FechaInicio, double Salario); 
    
}
