using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VRCModding.Api.Login;
using Microsoft.IdentityModel.Tokens;

namespace VRCModding.Services; 

public class TokenGenerator {
	private readonly byte[] privateKey;
	
	public TokenGenerator(byte[] privateKey) {
		this.privateKey = privateKey;
	}

	public string GenerateToken(LoginModel credentials) {
		var tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.WriteToken(
			tokenHandler.CreateToken(
				new SecurityTokenDescriptor {
					Subject = new ClaimsIdentity(new Claim[] {
						new(ClaimTypes.PrimarySid, credentials.User.Id.ToString()),
						new(ClaimTypes.Role, credentials.User.Permissions)
					}),
					Expires = DateTime.UtcNow.AddMinutes(15),
					SigningCredentials = new SigningCredentials(
						new SymmetricSecurityKey(privateKey),
						SecurityAlgorithms.HmacSha256Signature
					)
				}
			)
		);
	}
}
