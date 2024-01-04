using API.Context;
using API.Interfaces;
using API.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace API.Services
{
	public class UserServices : IUserServices
	{
		private readonly HospitalContext _context;
		public UserServices(HospitalContext context)
		{
			_context = context;
		}
		public async Task<Result<ViewUserToken>> Login(ViewUserLogin user)
		{
			try
			{
				var sql = ($"Exec VerificarUsuario '{user.Username}', '{user.Password}', @Resultado OUTPUT");
				var resultadoParam = new SqlParameter("@Resultado", SqlDbType.NVarChar, 50)
				{
					Direction = ParameterDirection.Output
				};
				var log = await _context.Database.ExecuteSqlRawAsync(sql, resultadoParam);
				string? resultado = resultadoParam.Value as string;


				if (resultado == null)
				{
					return new Result<ViewUserToken>() { Model = null, Message = $"No se encontro el usuario {user.Username}", Status = StatusCodes.Status204NoContent };
				}
				if (resultado == "Contraseña válida")
				{
					var Token = GenerateJwtToken(user.Username, 90);
					return new Result<ViewUserToken>()
					{
						Model = new ViewUserToken(user.Username, Token),
						Message = "Usuario correcto.",
						Status = StatusCodes.Status200OK
					};

				}

				return new Result<ViewUserToken>() { Model = null, Message = "Datos incorrectos.", Status = StatusCodes.Status403Forbidden };

			}
			catch (Exception)
			{
				return new Result<ViewUserToken>() { Model = null, Message = "Ocurrio un error en el servidor. Intentelo más tarde.", Status = StatusCodes.Status500InternalServerError };

			}
		}
		public async Task<Result<ViewUserToken>> CreateUser(ViewUser user)
		{
			var sql = ($"Exec CrearUsuario '{user.IdTrabajador}', '{user.Username}', '{user.Password}', '{user.Email}', @Resultado OUTPUT");
			var resultadoParam = new SqlParameter("@Resultado", SqlDbType.NVarChar, 50)
			{
				Direction = ParameterDirection.Output
			};
			try
			{

				var log = await _context.Database.ExecuteSqlRawAsync(sql, resultadoParam);
				string? resultado = resultadoParam.Value as string;

				await _context.SaveChangesAsync();

				if (resultado == null)
				{
					throw new Exception();
				}

				if (resultado == "Usuario creado")
				{
					//var Token = GenerateJwtToken(user.Username);

					return new Result<ViewUserToken>()
					{
						Model = new ViewUserToken(user.Username, ""),
						Message = "Usuario creado con exito.",
						Status = StatusCodes.Status200OK
					};
				}

				return new Result<ViewUserToken>()
				{
					Model = null,
					Message = $"Usuario ya existente.",
					Status = StatusCodes.Status400BadRequest
				};


			}
			catch (Exception e)
			{
				return new Result<ViewUserToken>()
				{
					Model = null,
					Message = "Ocurrio un error al crear el usuario.",
					Status = StatusCodes.Status500InternalServerError
				};
			}
		}
		public Task<Result<ViewUserToken>> DeleteUser(ViewUserPasswordToken user)
		{
			throw new NotImplementedException();
		}
		public async Task<Result<ViewUserReqPassword>> RequestResetPassword(ViewUserReqPassword user)
		{
			var usuario = await _context.Usuarios
				.Where(u => u.Email == user.Email).FirstOrDefaultAsync();

			if (usuario == null)
			{
				return new Result<ViewUserReqPassword>()
				{
					Model = null,
					Message = "El usuario no existe.",
					Status = StatusCodes.Status204NoContent
				};
			}

			var tiempoEnMinutos = 5;
			var token = GenerateJwtToken(usuario.Usuario1, tiempoEnMinutos);
			var cuerpo = GenerateResetPasswordHtml(usuario.Usuario1, token);

			var fechaExpiracion = DateTime.Now.AddMinutes(tiempoEnMinutos);

			var crearRecuperacion = new RecuperacionContrasena
			{
				FechaExpiracion = fechaExpiracion,
				TokenRecuperacion = token,
				IdUsuario = usuario.IdUsuario,
				Utilizado = false
			};

			await _context.RecuperacionContrasenas.AddAsync(crearRecuperacion);
			_context.SaveChanges();

			if (EnviarCorreo(user.Email, "Recuperación de contraseña", cuerpo))
			{
				return new Result<ViewUserReqPassword>
				{
					Model = new ViewUserReqPassword(user.Email),
					Message = $"Correo de recuperación de contraseña enviado a: {user.Email}. El token tiene una duración de {tiempoEnMinutos} minutos.",
					Status = StatusCodes.Status200OK
				};

			}

			return new Result<ViewUserReqPassword>
			{
				Model = null,
				Message = $"Ocurrio un error en el servidor. Intentelo más tarde.",
				Status = StatusCodes.Status500InternalServerError
			};
		}
		public async Task<Result<ViewUserLogin>> ResetPassword(ViewUserPasswordToken user)
		{
			try
			{

				var valido = await ValidateToken(new ViewUserToken(Username: user.Username, Token: user.Token));

				if (valido.Status != 200)
				{
					return new Result<ViewUserLogin>() { Model = null, Message = "Token invalido.", Status = StatusCodes.Status400BadRequest };
				}

				var reset = await _context
					.Database
					.ExecuteSqlRawAsync($"UPDATE Usuario SET Contraseña = HASHBYTES('SHA2_256', '{user.Password}' + (SELECT TOP 1 Salt FROM Usuario WHERE Usuario = '{user.Username}')) WHERE Usuario = '{user.Username}'"
);

				if (reset <= 0)
				{
					return new Result<ViewUserLogin>()
					{
						Model = null,
						Message = "No se pudo actualizar la contraseña.",
						Status = StatusCodes.Status400BadRequest
					};
				}

				var valid = await _context.RecuperacionContrasenas.FirstOrDefaultAsync(token => token.TokenRecuperacion == user.Token);
				if (valid != null) valid.Utilizado = true;

				await _context.SaveChangesAsync();


				return new Result<ViewUserLogin>()
				{
					Model = new ViewUserLogin(user.Username, ""),
					Message = "Contraseña actualizada con éxito.",
					Status = StatusCodes.Status200OK
				};
			}
			catch (Exception)
			{
				return new Result<ViewUserLogin>()
				{
					Model = null,
					Message = "No se pudo actualizar la contraseña debido a un error en el servidor.",
					Status = StatusCodes.Status500InternalServerError
				};
			}
		}
		public async Task<Result<ViewUserToken>> ValidateToken(ViewUserToken token)
		{

			if (ValidateJwtToken(token.Token, token.Username))
			{
				return new Result<ViewUserToken>()
				{
					Model = null,
					Message = "Token valido.",
					Status = StatusCodes.Status200OK
				};
			}
			return new Result<ViewUserToken>()
			{
				Model = null,
				Message = "Token invalido.",
				Status = StatusCodes.Status403Forbidden
			};

		}
		public bool EnviarCorreo(string destinatario, string asunto, string html)
		{
			try
			{
				var remitenteDireccion = "escom8059@gmail.com";
				var remitenteContraseña = "ftna nxzn gaur xexe";

				var mensaje = new MailMessage(remitenteDireccion, destinatario)
				{
					Subject = asunto,
					Body = html,
					IsBodyHtml = true
				};

				using (var smtpCliente = new SmtpClient("smtp.gmail.com"))
				{
					smtpCliente.Port = 587;
					smtpCliente.Credentials = new NetworkCredential(remitenteDireccion, remitenteContraseña);
					smtpCliente.EnableSsl = true;
					smtpCliente.UseDefaultCredentials = false;
					smtpCliente.Send(mensaje);
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		private static string GenerateJwtToken(string username, int Tiempo)
		{

			var key = Encoding.ASCII.GetBytes("HospitalProyESCOMJavierMung_2023");

			var tiempo1 = DateTime.UtcNow;
			var tiempo2 = DateTime.Now;

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
				NotBefore = DateTime.Now,
				Expires = DateTime.Now.AddMinutes(Tiempo),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
		private static bool ValidateJwtToken(string token, string Username)
		{
			var key = Encoding.ASCII.GetBytes("HospitalProyESCOMJavierMung_2023");

			var tokenHandler = new JwtSecurityTokenHandler();

			var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
			var expiration = jwtToken?.ValidTo.ToLocalTime();


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

				var usernameClaim = principal.FindFirst(ClaimTypes.Name)?.Value;
				if (usernameClaim != Username)
				{
					return false;
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		private static string GenerateResetPasswordHtml(string usuario, string token)
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("<!DOCTYPE html>");
			stringBuilder.AppendLine("<html lang=\"en\">");
			stringBuilder.AppendLine("<head>");
			stringBuilder.AppendLine("<meta charset=\"UTF-8\">");
			stringBuilder.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
			stringBuilder.AppendLine("<title>Restablecimiento de Contraseña</title>");
			stringBuilder.AppendLine("<style>");
			stringBuilder.AppendLine("body {");
			stringBuilder.AppendLine("  font-family: 'Arial', sans-serif;");
			stringBuilder.AppendLine("  margin: 0;");
			stringBuilder.AppendLine("  padding: 0;");
			stringBuilder.AppendLine("  background-color: #f4f4f4;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine(".container {");
			stringBuilder.AppendLine("  max-width: 600px;");
			stringBuilder.AppendLine("  margin: 20px auto;");
			stringBuilder.AppendLine("  background-color: #fff;");
			stringBuilder.AppendLine("  padding: 20px;");
			stringBuilder.AppendLine("  border-radius: 8px;");
			stringBuilder.AppendLine("  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine("h2 {");
			stringBuilder.AppendLine("  color: #333;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine("p {");
			stringBuilder.AppendLine("  color: #555;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine(".logo {");
			stringBuilder.AppendLine("  text-align: center;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine(".logo img {");
			stringBuilder.AppendLine("  max-width: 100%;");
			stringBuilder.AppendLine("  height: auto;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine(".button-container {");
			stringBuilder.AppendLine("  text-align: center;");
			stringBuilder.AppendLine("  margin-top: 20px;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine(".button {");
			stringBuilder.AppendLine("  display: inline-block;");
			stringBuilder.AppendLine("  padding: 10px 20px;");
			stringBuilder.AppendLine("  background-color: #007BFF;");
			stringBuilder.AppendLine("  color: #fff;");
			stringBuilder.AppendLine("  text-decoration: none;");
			stringBuilder.AppendLine("  border-radius: 5px;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine(".footer {");
			stringBuilder.AppendLine("  margin-top: 20px;");
			stringBuilder.AppendLine("  text-align: center;");
			stringBuilder.AppendLine("  color: #777;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine("a {");
			stringBuilder.AppendLine("  text-decoration: none;");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine("@media only screen and (max-width: 600px) {");
			stringBuilder.AppendLine("  .button-container {");
			stringBuilder.AppendLine("    text-align: center;");
			stringBuilder.AppendLine("  }");
			stringBuilder.AppendLine("}");
			stringBuilder.AppendLine("</style>");
			stringBuilder.AppendLine("</head>");
			stringBuilder.AppendLine("<body>");
			stringBuilder.AppendLine("  <div class=\"container\">");
			stringBuilder.AppendLine("    <h2>Restablecimiento de Contraseña</h2>");
			stringBuilder.AppendLine($"    <p>Estimado  {usuario},</p>");
			stringBuilder.AppendLine("    <p>Hemos recibido una solicitud para restablecer la contraseña de su cuenta. Para continuar con este proceso, copie el siguiente token y haga clic en el botón a continuación:</p>");
			stringBuilder.AppendLine($"    <p><strong>{token}</strong></p>");
			stringBuilder.AppendLine($"    <div class=\"button-container\">");
			stringBuilder.AppendLine($"      <a style=\"color:#fff; text-decoration:none\" href=\"#\" class=\"button\">Restablecer Contraseña</a>");
			stringBuilder.AppendLine("    </div>");
			stringBuilder.AppendLine("    <p>Si no ha solicitado el restablecimiento de contraseña, puede ignorar este correo electrónico.</p>");
			stringBuilder.AppendLine("    <div class=\"footer\">");
			stringBuilder.AppendLine("      <p>Atentamente,<br>El Equipo de Soporte</p>");
			stringBuilder.AppendLine("    </div>");
			stringBuilder.AppendLine("  </div>");
			stringBuilder.AppendLine("</body>");
			stringBuilder.AppendLine("</html>");

			return stringBuilder.ToString();

		}
	}
}
