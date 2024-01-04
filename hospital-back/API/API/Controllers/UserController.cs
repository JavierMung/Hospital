using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserServices _users;
		public UserController(IUserServices users)
		{
			_users = users;
		}
		[HttpPost("login")]
		public async Task<ActionResult<Result<ViewUserToken>>> Login(ViewUserLogin usuario)
		{
			var user = await _users.Login(usuario);
			return user;
		}
		[HttpPost("validarToken")]
		public async Task<ActionResult<Result<ViewUserToken>>> ValidateToken(ViewUserToken usuario)
		{
			var user = await _users.ValidateToken(usuario);
			return user;
		}
		[HttpPost("crearUsuario")]
		public async Task<ActionResult<Result<ViewUserToken>>> CreateUser([FromBody] ViewUser usuario)
		{
			var user = await _users.CreateUser(usuario);
			return user;
		}		
		[HttpPost("resetContraseña")]
		public async Task<ActionResult<Result<ViewUserLogin>>> ResetPassword([FromBody] ViewUserPasswordToken usuario)
		{
			var user = await _users.ResetPassword(usuario);
			return user;
		}
		[HttpPost("solicitarRecuperacionContraseña")]
		public async Task<ActionResult<Result<ViewUserReqPassword>>> RequestResetPassword([FromBody] ViewUserReqPassword usuario)
		{
			var user = await _users.RequestResetPassword(usuario);
			return user;
		}

	}
}
