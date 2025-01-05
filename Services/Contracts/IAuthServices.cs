using System.Runtime.CompilerServices;
using userManagement.DTOs;
using userManagement.Models;

namespace userManagement.Services.Contracts
{
	public interface IAuthServices
	{
		Task<AuthModel> RegisterAsync(registerDto registerDto);
		Task<AuthModel> LoginAsync(loginDto loginDto);
		Task<AuthModel> RefreshTokenAsync(string token);
		Task<bool> RefreshTokenRevokeAsync(string token);
		Task<bool> VerifyEmail(string token);
	}
}
