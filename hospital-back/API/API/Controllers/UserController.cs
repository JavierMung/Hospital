using API.Interfaces;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<Result<ViewUserResponseToken>>> Login(ViewUserLogin usuario)
        {
            var user = await _users.Login(usuario);
            return user;
        }
        [HttpPost("validarToken")]
        public async Task<ActionResult<Result<ViewUserResponseToken>>> ValidarToken(ViewUserCreate usuario)
        {
            var user = await _users.ValidateToken(usuario);
            return user;
        }


    }
}
