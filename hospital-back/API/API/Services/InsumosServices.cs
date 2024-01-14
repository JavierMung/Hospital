using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Services
{
	public class InsumosServices : IInsumosServices
	{
		private readonly HospitalContext _context;
		public InsumosServices(HospitalContext context)
		{
			_context = context;
		}

		public async Task<Result<ViewInsumo>> AddInsumo(ViewInsumoAdd Insumo)
		{
			using var transaction = _context.Database.BeginTransaction();
			try
			{

				Insumo insumoAdd = new Insumo();
				insumoAdd.IdTipoInsumo = Insumo.Tipo;
				insumoAdd.Nombre = Insumo.Nombre;
				insumoAdd.Stock = Insumo.Stock;
				insumoAdd.Precio = Insumo.Precio;

				var agregado = await _context.Insumos.AddAsync(insumoAdd);

				_context.SaveChanges();

				if (agregado == null)
				{
					return new Result<ViewInsumo>()
					{
						Model = null,
						Message = "No se pudo crear el insumo. Revisa los datos.",
						Status = StatusCodes.Status400BadRequest
					};
				}

				transaction.Commit();

				TipoInsumo ? tipo = await _context.TipoInsumos.FindAsync(Insumo.Tipo);

				return new Result<ViewInsumo>()
				{
					Model = new ViewInsumo(
						insumoAdd.IdInsumo,
						insumoAdd.Nombre,
						insumoAdd.Precio,
						tipo.Tipo,
						insumoAdd.IdTipoInsumo,
						insumoAdd.Stock ?? 0
						),
					Message = "Insumo creado con exito.",
					Status = StatusCodes.Status200OK
				};


			}
			catch (Exception)
			{
				transaction.Rollback();
				return new Result<ViewInsumo>()
				{
					Model = null,
					Message = "Error al crear el insumo.",
					Status = StatusCodes.Status500InternalServerError
				};
			}

		}

		public async Task<Result<int>> DeleteInsumo(int Id)
		{
			var deleteInsumo = await _context.Insumos.FindAsync(Id);
			if (deleteInsumo == null)
			{
				return new Result<int>()
				{
					Model = 0,
					Message = "El insumo no existe.",
					Status = StatusCodes.Status400BadRequest
				};
			}
			_context.Insumos.Remove(deleteInsumo);

			return new Result<int>()
			{
				Model = 1,
				Message = "Insumo borrado con exito.",
				Status = StatusCodes.Status400BadRequest
			};


		}

		public async Task<Result<ViewInsumo>> GetInsumoById(int Id)
		{
			try
			{

				var Insumo = await _context.Insumos.Include(insum => insum.IdTipoInsumoNavigation).Where(ins => ins.IdInsumo == Id).FirstOrDefaultAsync();
				if (Insumo == null)
				{
					return new Result<ViewInsumo>()
					{
						Model = null,
						Message = "El insumo no existe.",
						Status = StatusCodes.Status400BadRequest
					};
				}

				return new Result<ViewInsumo>()
				{
					Model = new ViewInsumo(
							Insumo.IdInsumo,
							Insumo.Nombre,
							Insumo.Precio,
							Insumo.IdTipoInsumoNavigation.Tipo,
							Insumo.IdTipoInsumo,
							Insumo.Stock ?? 0
							),
					Message = "Insumo recuperado con exito.",
					Status = StatusCodes.Status200OK
				};
			}
			catch (Exception)
			{
				return new Result<ViewInsumo>()
				{
					Model = null,
					Message = "Ocurrio un error interno al recuperar el insumo.",
					Status = StatusCodes.Status500InternalServerError
				};
			}

		}

		public async Task<Result<List<ViewInsumo>>> GetInsumos()
		{

			try
			{
				var Insumos = await _context.Insumos.Include(insum => insum.IdTipoInsumoNavigation).ToListAsync();

				if (Insumos == null)
				{
					return new Result<List<ViewInsumo>>()
					{
						Model = null,
						Message = "No existen insumos.",
						Status = StatusCodes.Status200OK
					};
				}
				List<ViewInsumo> listInsumo = new List<ViewInsumo>();

				foreach (var Insumo in Insumos)
				{
					var i = new ViewInsumo(
							Insumo.IdInsumo,
							Insumo.Nombre,
							Insumo.Precio,
							Insumo.IdTipoInsumoNavigation.Tipo,
							Insumo.IdTipoInsumo,
							Insumo.Stock ?? 0
							);
					listInsumo.Add(i);

				}
				return new Result<List<ViewInsumo>>()
				{
					Model = listInsumo,
					Message = "Insumos recuperados con exito.",
					Status = StatusCodes.Status200OK
				};
			}
			catch (Exception)
			{
				return new Result<List<ViewInsumo>>()
				{
					Model = null,
					Message = "Ocurrio un error interno al recuperar los insumos.",
					Status = StatusCodes.Status500InternalServerError
				};

			}
		}

		public async Task<Result<ViewInsumo>> UpdateInsumo(ViewInsumoUpdate Insumo)
		{
			try
			{
				var insumoAdd = await _context.Insumos.Include(insum => insum.IdTipoInsumoNavigation).Where(ins => ins.IdInsumo == Insumo.IdInsumo).FirstOrDefaultAsync();
				if (insumoAdd == null)
				{
					return new Result<ViewInsumo>()
					{
						Model = null,
						Message = "El insumo no existe.",
						Status = StatusCodes.Status400BadRequest
					};
				}

				insumoAdd.IdTipoInsumo = Insumo.IdTipo;
				insumoAdd.Nombre = Insumo.Nombre;
				insumoAdd.Stock = Insumo.Stock;
				insumoAdd.Precio = Insumo.Precio;

				await _context.SaveChangesAsync();

				return new Result<ViewInsumo>()
				{
					Model = new ViewInsumo(
							insumoAdd.IdInsumo,
							insumoAdd.Nombre,
							insumoAdd.Precio,
							insumoAdd.IdTipoInsumoNavigation.Tipo,
							insumoAdd.IdTipoInsumo,
							insumoAdd.Stock ?? 0
							),
					Message = "Insumo actualizado con exito.",
					Status = StatusCodes.Status200OK
				};

			}
			catch (Exception)
			{
				return new Result<ViewInsumo>()
				{
					Model = null,
					Message = "Ocurrio un error interno al actualizar el insumo.",
					Status = StatusCodes.Status500InternalServerError
				};

			}
		}
	}
}
