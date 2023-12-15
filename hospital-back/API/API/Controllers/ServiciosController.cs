using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ServiciosController : ControllerBase
	{
		private readonly IServiciosServices _servicioServices;

		public ServiciosController(IServiciosServices servicioServices)
		{
			_servicioServices = servicioServices;
		}

		[HttpGet("obtenerServicio/{id}")]
		public async Task<ActionResult<ViewServicio>> GetServicio(int id)
		{
			try
			{
				var res = await _servicioServices.GetServicio(id);
				if (res == null) return StatusCode(StatusCodes.Status204NoContent, "No se encontraron sevicios con ese ID.");
				return res;
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Intentelo más tarde. ");
			}

		}

		[HttpGet("obtenerServicios")]
		public async Task<ActionResult<List<ViewServicio>?>> GetALLServicios()
		{
			try
			{
				var res = await _servicioServices.GetAllServicios();
				if (res == null) return StatusCode(StatusCodes.Status204NoContent, "No se encontraron sevicios con ese ID.");
				return res;
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Intentelo más tarde. ");
			}

		}

        [HttpDelete("eliminarServicio/{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                var resultado = await _servicioServices.DeleteServicio(id);

                switch (resultado)
                {
                    case 0:
                        return NotFound("Servicio no encontrado.");
                    case 1:
                        return Ok("Servicio eliminado con éxito.");
                    case 2:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el servicio.");
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error inesperado.");
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar la excepción con algún mecanismo de logging
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en el servidor. Intentelo más tarde.");
            }
        }

        [HttpPost("agregarServicio")]
        public async Task<ActionResult<ViewServicio>> AddServicio([FromBody] ViewServicio model)
        {
            try
            {
                var respuesta = await _servicioServices.AddServicio(model);
                if (respuesta == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Error al crear el servicio, revise los datos por favor");
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPut("actualizarServicio")]
        public async Task<ActionResult<ViewServicio>> UpdateServicio([FromBody] ViewServicio model)
        {
            if (model == null)
            {
                return BadRequest("Datos de actualización inválidos.");
            }

            var resultado = await _servicioServices.UpdateServicio(model);

            if (resultado == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Error al crear el servicio, revise los datos por favor");
            }

            return resultado;
        }



    }
}
