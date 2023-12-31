﻿using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.ViewModels;
using API.Context;

namespace API.Controllers
{

	[Route("[controller]")]
	[ApiController]
	public class MedicosController : ControllerBase
	{
		private readonly IMedicosServices _medicosServices;
		private readonly IConfiguration _configuration;

		public MedicosController(IMedicosServices medicosServices, IConfiguration configuration)
		{
			_medicosServices = medicosServices;
			_configuration = configuration;
		}

		[HttpGet("obtenerMedico/{id}")]
		public async Task<ActionResult<Result<ViewMedicos>>> GetMedico(int id)
		{
			if (id <= 0)
				return BadRequest(new Result<ViewTrabajador>
				{
					Model = null,
					Message = "El ID es incorrecto.",
					Status = 400
				});

			return await ExecuteOperation(async () => await _medicosServices.GetMedico(id));
		}

		[HttpGet("obtenerMedicos")]
		public async Task<ActionResult<Result<ViewListMedicos>>> GetMedicos()
		{
			return await ExecuteOperation(async () => await _medicosServices.GetMedicos());
		}

		public async Task<ActionResult<Result<T>>> ExecuteOperation<T>(Func<Task<Result<T>>> operation)
		{
			try
			{
				var result = await operation();

				if (result.Status == 204)
				{
					return StatusCode(StatusCodes.Status204NoContent, result);
				}
				else if (result.Status == 500)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, result);
				}
				else if (result.Status == 400)
				{
					return StatusCode(StatusCodes.Status400BadRequest, result);
				}

				return StatusCode(StatusCodes.Status200OK, result);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error en el servidor: {ex.Message}. Inténtelo más tarde.");
			}
		}

	}
}
