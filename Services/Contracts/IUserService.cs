using userManagement.DTOs;
using userManagement.Models;

namespace userManagement.Services.Contracts
{
	public interface IUserService
	{
		Task<userDto> createUser(registerDto model);
		Task<userDto> updateUser(string userId, updateUserDetails model);
		Task<userDto> deleteUser(string userId);
		Task<bool> validUser(string userEmail);
	}
}
