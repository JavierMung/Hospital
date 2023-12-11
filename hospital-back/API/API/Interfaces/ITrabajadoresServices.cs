using API.ViewModels;
using System.Data;

namespace API.Interfaces
{
    public interface ITrabajadoresServices
    {
        public Task<ViewTrabajadores?> GetTrabajador(int id_Trabajador);

        public Task<ViewListTrabajadores?> GetTrabajadores();

        public Task<int> DeleteTrabajador(int id_Trabajador);

        public Task<int> AddTrabajador(int id_Rol, int id_Horario, int id_Persona, DateTime Fecha_Inicio, double Salario);

        Task<bool> UpdateTrabajador(int id_trabajador,int id_Rol, int id_Horario, int id_Persona, DateTime Fecha_Inicio, double Salario);
    }
}
