using API.ViewModels;
using System.Data;

namespace API.Interfaces
{
    public interface ITrabajadoresServices
    {
        public Task<ViewTrabajadores?> GetTrabajador(int id_Trabajador);

        public Task<ViewListTrabajadores?> GetTrabajadores();

        Task<Result<ViewTrabajador>> DeleteTrabajador(int idTrabajador);

        Task<Result<ViewTrabajador>> AddTrabajador(ViewAddTrabajador trabajadorRequest);

        Task<Result<ViewTrabajador>> UpdateTrabajador(int id_trabajador, ViewTrabajador trabajador);
    }
}
