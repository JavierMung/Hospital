namespace API.ViewModels
{
    public class Result<T>
    {
        public T Model { get; set; }
        public string Message { get; set; }
    }
    public record ViewTrabajadores(int idTrabajador, string Nombre, string Rol, double Salario);
    public record ViewTrabajador( int idTrabajador, int idRol, int IdHorario, int IdPersona,  DateTime FechaInicio, double Salario);
    public record ViewAddTrabajador(int idRol, int IdHorario, int IdPersona, DateTime FechaInicio, double Salario);

}
