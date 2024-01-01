using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services
{
    public class UserServices : IUserServices
    {
        private readonly HospitalContext _context;
        public UserServices(HospitalContext context)
        {
            _context = context;
        }
        public async Task<Result<ViewUserResponseToken>> Login(ViewUserLogin user)
        {
            try
            {
                var sql = ($"Exec VerificarUsuario '{user.Username}', '{user.Password}', @Resultado OUTPUT");
                var resultadoParam = new SqlParameter("@Resultado", SqlDbType.NVarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                var log = await _context.Database.ExecuteSqlRawAsync(sql, resultadoParam);
                var resultado = resultadoParam.Value as string;


                if (resultado == null)
                {
                    return new Result<ViewUserResponseToken>() { Model = null, Message = $"No se encontro el usuario {user.Username}", Status = StatusCodes.Status204NoContent };
                }
                if (resultado == "Contraseña válida")
                {
                    var Token = GenerateJwtToken(user.Username);
                    return new Result<ViewUserResponseToken>() { 
                        Model = new ViewUserResponseToken(user.Username, Token), 
                        Message = "Usuario correcto.", 
                        Status = StatusCodes.Status200OK };

                }

                return new Result<ViewUserResponseToken>() { Model = null, Message = "Datos incorrectos.", Status = StatusCodes.Status403Forbidden };

            }
            catch (Exception ex)
            {
                return new Result<ViewUserResponseToken>() { Model = null, Message = "Ocurrio un error en el servidor. Intentelo más tarde.", Status = StatusCodes.Status500InternalServerError };

            }
        }
        public Task<Result<ViewUserResponseToken>> CreateUser(ViewUserCreate user)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ViewUser>> DeleteUser(ViewUserDelete user)
        {
            throw new NotImplementedException();
        }


        public Task<Result<ViewUserResetPassword>> ResetPassword(ViewUserResetPassword user)
        {
            throw new NotImplementedException();
        }

        public async  Task<Result<ViewUserResponseToken>> ValidateToken(ViewUserCreate token)
        {

            if (ValidateJwtToken(token.Token))
            {
                return new Result<ViewUserResponseToken>()
                {
                    Model = null,
                    Message = "Token valido.",
                    Status = StatusCodes.Status200OK
                };
            }
            return new Result<ViewUserResponseToken>()
            {
                Model = null,
                Message = "Token invalido.",
                Status = StatusCodes.Status403Forbidden
            };

        }

        private string GenerateJwtToken(string username)
        {
  
            var key = Encoding.ASCII.GetBytes("HospitalProyESCOMJavierMung_2023");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public bool ValidateJwtToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("HospitalProyESCOMJavierMung_2023");

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var expiration = jwtToken?.ValidTo;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                // La validación fue exitosa
                return true;
            }
            catch (Exception)
            {
                // La validación falló
                return false;
            }
        }
    }
}
