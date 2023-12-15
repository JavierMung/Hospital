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

                if (resultado.Model != null)
                {
                    return Ok(resultado);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, resultado.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Inténtelo más tarde.");
            }
        }


        [HttpPost("agregar")]
        public async Task<IActionResult> AddTrabajador([FromBody] ViewAddTrabajador trabajadorRequest)
        {
            var resultado = await _trabajadorServices.AddTrabajador(trabajadorRequest);

            if (resultado.Model != null)
            {
                return Ok(resultado);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resultado.Message);
            }
        }




        [HttpPut("actualizarTrabajador/{id}")]
        public async Task<IActionResult> UpdateTrabajador(int id, [FromBody] ViewTrabajador model)
        {
            if (model == null)
            {
                return BadRequest("Datos del trabajador son inválidos.");
            }

            var resultado = await _trabajadorServices.UpdateTrabajador(id, model);

            if (resultado.Model != null)
            {
                return Ok(resultado);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resultado.Message);
            }
        }





    }
}
