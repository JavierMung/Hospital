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

        public async Task<int> DeleteTrabajador(int idTrabajador)
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
        }

        public async Task<int> AddTrabajador(int id_Rol, int id_Horario, int id_Persona, DateTime Fecha_Inicio, double Salario)
        {
            try
            {
                var persona = await _context.Personas.FindAsync(id_Persona);
                if (persona == null)
                {
                    return -1; // Código para indicar que idPersona no existe
                }
                var nuevoTrabajador = new Trabajador
                {
                    IdRol = id_Rol,
                    IdHorario = id_Horario,
                    IdPersona = id_Persona,
                    FechaInicio = Fecha_Inicio,
                    Salario = Salario
                };

                _context.Trabajadors.Add(nuevoTrabajador);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return 0;
            }
        }

        public async Task<bool> UpdateTrabajador(int idTrabajador, int idRol, int idHorario, int idPersona, DateTime fechaInicio, double salario)
        {
            try
            {
                var trabajador = await _context.Trabajadors.FindAsync(idTrabajador);
                if (trabajador == null)
                {
                    return false; // Trabajador no encontrado
                }

                trabajador.IdRol = idRol;
                trabajador.IdHorario = idHorario;
                trabajador.IdPersona = idPersona;
                trabajador.FechaInicio = fechaInicio;
                trabajador.Salario = salario;

                _context.Trabajadors.Update(trabajador);
                await _context.SaveChangesAsync();
                return true; // Actualización exitosa
            }
            catch (Exception e)
            {
                return false; // Error durante la actualización
            }
        }



    }
}
