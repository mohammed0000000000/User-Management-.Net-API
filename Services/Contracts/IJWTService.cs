using System.IdentityModel.Tokens.Jwt;
using userManagement.Models;

namespace userManagement.Services.Contracts
{
	public interface IJWTService
	{
		Task<JwtSecurityToken> GenerateToken(ApplicationUser user);
		string GenerateRefreshToken();
	}
}
