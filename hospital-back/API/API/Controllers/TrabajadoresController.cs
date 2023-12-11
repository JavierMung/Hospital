using API.Interfaces;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TrabajadoresController : ControllerBase
    {
        private readonly ITrabajadoresServices _trabajadorServices;
        private readonly IConfiguration _configuration;

        public TrabajadoresController(ITrabajadoresServices TrabajadoresServices, IConfiguration configuration)
        {
            _trabajadorServices = TrabajadoresServices;
        }
        [HttpGet("obtenerTrabajador/{id}")]
        public async Task<ActionResult<ViewTrabajadores>> GetTrabajador(int id)
        {
            try
            {
                var res = await _trabajadorServices.GetTrabajador(id);
                if (res == null) return StatusCode(StatusCodes.Status204NoContent, "No se encontraron Trabajadores con ese ID.");
                return res;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Intentelo más tarde. ");
            }

        }

        [HttpGet("obtenerTrabajadores")]
        public async Task<ActionResult<ViewListTrabajadores>> GetTrabajadores()
        {
            //if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest, "El ID es incorrecto.");

            try
            {
                var res = await _trabajadorServices.GetTrabajadores();
                if (res == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "No existen medicos con ese ID. intentelo con otro ID por favor.");
                }
                return res;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Error en el servidor: " + ex.Message + ". Intentelo mas tarde.");
            }
        }

        [HttpDelete("eliminarTrabajador/{id}")]
        public async Task<IActionResult> DeleteTrabajador(int id)
        {
            try
            {
                var resultado = await _trabajadorServices.DeleteTrabajador(id);

                switch (resultado)
                {
                    case 0:
                        return NotFound("Trabajador no encontrado.");
                    case 1:
                        return Ok("Trabajador eliminado con éxito.");
                    case 2:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el Trabajador.");
                    case 3:
                        return Conflict("No se puede eliminar el Trabajador debido a las dependencias existentes.");
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error inesperado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Intentelo más tarde.");
            }
        }

        [HttpPost("agregarTrabajador")]
        public async Task<IActionResult> AddTrabajador([FromBody] ViewTrabajador model)
        {
            if (model == null)
            {
                return BadRequest("Datos del trabajador son inválidos.");
            }

            var resultado = await _trabajadorServices.AddTrabajador(model.idRol, model.IdHorario, model.IdPersona, model.FechaInicio, model.Salario);

            switch (resultado)
            {
                case 1:
                    return Ok("Trabajador agregado con éxito.");
                case -1:
                    return BadRequest("El ID de la persona proporcionado no existe.");
                case 0:
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al agregar el trabajador, verificar los datos.");
            }
        }

        [HttpPut("actualizarTrabajador/{id}")]
        public async Task<IActionResult> UpdateTrabajador(int id, [FromBody] ViewTrabajador model)
        {
            if (model == null)
            {
                return BadRequest("Datos del trabajador son inválidos.");
            }

            var resultado = await _trabajadorServices.UpdateTrabajador(model.idTrabajador, model.idRol, model.IdHorario, model.IdPersona, model.FechaInicio, model.Salario);

            if (resultado)
            {
                return Ok("Trabajador actualizado con éxito.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el trabajador.");
            }
        }




    }    
}
