using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Helper
{
	public class Token
	{
		public static bool ValidateJwtToken(string token, string Username)
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
		public static string GenerateJwtToken(string username, int Tiempo)
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

	}
}
