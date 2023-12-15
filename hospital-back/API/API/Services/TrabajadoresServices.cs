using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Interfaces;
using API.ViewModels;

namespace API.Services { 
public class TrabajadoresServices : ITrabajadoresServices
{
    public readonly HospitalContext _context;

    public TrabajadoresServices(HospitalContext context) 
    {
        _context = context;
    }



    public async Task<ViewTrabajadores?> GetTrabajador(int id)
    {
        try
        {
            var trabajador = await _context.Trabajadors
                .Include(t => t.IdPersonaNavigation) // Incluir detalles de la persona
                .Include(t => t.IdRolNavigation) // Incluir detalles del rol
                .Where(t => t.IdTrabajador == id) // Filtrar por ID del trabajador
                .FirstOrDefaultAsync();

            if (trabajador == null) return null;

            // Crear un nuevo objeto ViewTrabajador con la información requerida
            ViewTrabajadores respuesta = new ViewTrabajadores(
                trabajador.IdTrabajador,
                trabajador.IdPersonaNavigation.Nombre + " " +
                trabajador.IdPersonaNavigation.ApellidoPaterno + " " +
                trabajador.IdPersonaNavigation.ApellidoMaterno,
                trabajador.IdRolNavigation.Rol, // Asumiendo que tienes un campo 'NombreRol' en la entidad Role
                trabajador.Salario
            );

            return respuesta;
        }
        catch (Exception e)
        {
            // Manejo de errores, considera registrar el error
            return null;
        }
    }

        public async Task<ViewListTrabajadores?> GetTrabajadores()
        {
            try
            {
                var consulta = await _context.Trabajadors
                    .Include(t => t.IdPersonaNavigation) // Incluir detalles de la persona
                    .Include(t => t.IdRolNavigation) // Incluir detalles del rol
                    .ToListAsync();

                if (!consulta.Any()) return null;

                List<ViewTrabajadores> Trabajadores = new List<ViewTrabajadores>();

                foreach (var trabajador in consulta)
                {
                    Trabajadores.Add(new ViewTrabajadores(
                        trabajador.IdTrabajador,
                        trabajador.IdPersonaNavigation.Nombre + " " +
                        trabajador.IdPersonaNavigation.ApellidoPaterno + " " +
                        trabajador.IdPersonaNavigation.ApellidoMaterno,
                        trabajador.IdRolNavigation.Rol,
                        trabajador.Salario
                    ));
                }

                ViewListTrabajadores listaTrabajadores = new ViewListTrabajadores(Trabajadores);

                return listaTrabajadores;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*public async Task<int> DeleteTrabajador(int idTrabajador)
        {
            try
            {
                var trabajador = await _context.Trabajadors.FindAsync(idTrabajador);
                if (trabajador == null)
                {
                    return 0; // Trabajador no encontrado
                }

                // Verificar si hay médicos asociados al trabajador
                if (_context.Medicos.Any(m => m.IdTrabajador == idTrabajador))
                {
                    return 3; // Existen dependencias, no se puede eliminar
                }

                _context.Trabajadors.Remove(trabajador);
                await _context.SaveChangesAsync();
                return 1; // Trabajador eliminado con éxito
            }
            catch (Exception e)
            {
                return 2; // Error durante la eliminación
            }
        }*/
        public async Task<Result<ViewTrabajador>> DeleteTrabajador(int idTrabajador)
        {
            try
            {
                var trabajador = await _context.Trabajadors.FindAsync(idTrabajador);
                if (trabajador == null)
                {
                    return new Result<ViewTrabajador> { Message = "Trabajador no encontrado." };
                }

                // Verificar si hay médicos asociados al trabajador
                if (_context.Medicos.Any(m => m.IdTrabajador == idTrabajador))
                {
                    return new Result<ViewTrabajador> { Message = "No se puede eliminar el Trabajador debido a las dependencias existentes." };
                }

                _context.Trabajadors.Remove(trabajador);
                await _context.SaveChangesAsync();

                return new Result<ViewTrabajador> { Model = new ViewTrabajador(trabajador.IdTrabajador, trabajador.IdRol, trabajador.IdHorario, trabajador.IdPersona, trabajador.FechaInicio, trabajador.Salario), Message = "Trabajador eliminado con éxito." };
            }
            catch (DbUpdateException)
            {
                return new Result<ViewTrabajador> { Message = "Error al eliminar el Trabajador." };
            }
            catch (Exception)
            {
                return new Result<ViewTrabajador> { Message = "Error durante la eliminación." };
            }
        }


        public async Task<Result<ViewTrabajador>> AddTrabajador(ViewAddTrabajador trabajadorRequest)
        {
            try
            {
                var persona = await _context.Personas.FindAsync(trabajadorRequest.IdPersona);
                if (persona == null)
                {
                    return new Result<ViewTrabajador> { Message = "La persona con el ID especificado no existe." };
                }

                var nuevoTrabajador = new Trabajador
                {
                    IdRol = trabajadorRequest.idRol,
                    IdHorario = trabajadorRequest.IdHorario,
                    IdPersona = trabajadorRequest.IdPersona,
                    FechaInicio = trabajadorRequest.FechaInicio,
                    Salario = trabajadorRequest.Salario
                };

                _context.Trabajadors.Add(nuevoTrabajador);
                await _context.SaveChangesAsync();

                return new Result<ViewTrabajador> { Model = new ViewTrabajador(nuevoTrabajador.IdTrabajador, nuevoTrabajador.IdRol, nuevoTrabajador.IdHorario, nuevoTrabajador.IdPersona, nuevoTrabajador.FechaInicio, nuevoTrabajador.Salario), Message = "Trabajador agregado con éxito." };
            }
            catch (Exception)
            {
                return new Result<ViewTrabajador> { Message = "Error al agregar el Trabajador." };
            }
        }



        //public async Task<bool> UpdateTrabajador(int idTrabajador, int idRol, int idHorario, int idPersona, DateTime fechaInicio, double salario)
        //{
        //    try
        //    {
        //        var trabajador = await _context.Trabajadors.FindAsync(idTrabajador);
        //        if (trabajador == null)
        //        {
        //            return false; // Trabajador no encontrado
        //        }

        //        trabajador.IdRol = idRol;
        //        trabajador.IdHorario = idHorario;
        //        trabajador.IdPersona = idPersona;
        //        trabajador.FechaInicio = fechaInicio;
        //        trabajador.Salario = salario;

        //        _context.Trabajadors.Update(trabajador);
        //        await _context.SaveChangesAsync();
        //        return true; // Actualización exitosa
        //    }
        //    catch (Exception e)
        //    {
        //        return false; // Error durante la actualización
        //    }
        //} 
        public async Task<Result<ViewTrabajador>> UpdateTrabajador(int idTrabajador, ViewTrabajador trabajador)
        {
            try
            {
                var trabajadorEntity = await _context.Trabajadors.FindAsync(idTrabajador);
                if (trabajadorEntity == null)
                {
                    return new Result<ViewTrabajador> { Message = "Trabajador no encontrado." };
                }

                var persona = await _context.Personas.FindAsync(trabajador.IdPersona);
                if (persona == null)
                {
                    return new Result<ViewTrabajador> { Message = "La persona con el ID especificado no existe." };
                }

                trabajadorEntity.IdRol = trabajador.idRol;
                trabajadorEntity.IdHorario = trabajador.IdHorario;
                trabajadorEntity.IdPersona = trabajador.IdPersona;
                trabajadorEntity.FechaInicio = trabajador.FechaInicio;
                trabajadorEntity.Salario = trabajador.Salario;

                _context.Trabajadors.Update(trabajadorEntity);
                await _context.SaveChangesAsync();

                var updatedModel = new ViewTrabajador(
                    trabajadorEntity.IdTrabajador,
                    trabajadorEntity.IdRol,
                    trabajadorEntity.IdHorario,
                    trabajadorEntity.IdPersona,
                    trabajadorEntity.FechaInicio,
                    trabajadorEntity.Salario
                );

                return new Result<ViewTrabajador> { Model = updatedModel, Message = "Trabajador actualizado con éxito." };
            }
            catch (Exception)
            {
                return new Result<ViewTrabajador> { Message = "Error al actualizar el Trabajador." };
            }
        }




    }
}
