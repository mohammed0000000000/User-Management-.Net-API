using userManagement.Models;

namespace userManagement.Services.Contracts
{
	public interface IJwtServices
	{
		Task<string> GenerateToken(ApplicationUser user);
		RefreshToken GenerateRefreshToken();
	}
}
