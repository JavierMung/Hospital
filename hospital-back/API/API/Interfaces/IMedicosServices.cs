﻿using API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces
{
	public interface IMedicosServices
	{
		public Task<Result<ViewMedicos>> GetMedico(int id);
		public Task<Result<ViewMedicos>> GetMedicoByIdTrabajador(int id);
		public Task<Result<ViewListMedicos>> GetMedicos();
		public Task<Result<ViewMedicos>> UpdateMedico(ViewMedicosUpdate medicosAdd);
		public Task<Result<ViewMedicos>> AddMedico(ViewMedicoAdd medicosAdd);
	}
}

