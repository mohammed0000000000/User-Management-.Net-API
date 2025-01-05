using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using userManagement.Data;
using userManagement.DTOs;
using userManagement.Models;
using userManagement.Services.Contracts;

namespace userManagement.Services.Implement
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> userManager;
		public UserService(UserManager<ApplicationUser> userManager) {
			this.userManager = userManager;
		}

		public async Task<userDto> updateUser(string userId, updateUserDetails model) {
			try {
				var user = await userManager.FindByIdAsync(userId);
				if (user is null) {
					return new userDto() {
						Success = false,
						Error = true,
						Message = "InValid User Identifier"
					};
				}
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.UserName = $"{model.FirstName} {model.LastName}";

				var updateInfo = await userManager.UpdateAsync(user);
				if(!updateInfo.Succeeded){
					string errorMessage = string.Join(" ", updateInfo.Errors.Select(x => x.Description));
					return new userDto() {
						Success = false,
						Error = true,
						Message = errorMessage
					};
				}
				return new userDto() {
					Message = "User Update Successfully",
					Success = true,
					Error = false,
					UserName = $"{model.FirstName} {model.LastName}"
				};
			} catch (Exception ex) {
				return new userDto() {
					Error = true,
					Message = "An unexpected error occurred while update the user."
				};
			}
		}

		public async Task<userDto> deleteUser(string userId) {
			try {
				var user = await userManager.FindByIdAsync(userId);
				if (user is null) {
					return new userDto() {
						Success = false,
						Error = true,
						Message = "InValid User Identifier"
					};
				}
				user.IsAccountDisabled = true;
				user.LockoutEnd = DateTime.UtcNow;

				var updateInfo = await userManager.UpdateAsync(user);
				if (!updateInfo.Succeeded) {
					string errorMessage = string.Join(" ", updateInfo.Errors.Select(x => x.Description));
					return new userDto() {
						Success = false,
						Error = true,
						Message = errorMessage
					};
				}
				return new userDto() {
					Message = "User Delete Successfully",
					Success = true,
					Error = false,
				};
			} catch (Exception ex) {
				return new userDto() {
					Error = true,
					Message = "An unexpected error occurred while delete the user."
				};
			}
		}
		public async Task<bool> validUser(string userEmial) {
			try {
				var user = await userManager.FindByEmailAsync(userEmial);
				if (user is null) { return false; }
				if (user.IsAccountDisabled) {
					return false;
				}
				return true;
			} catch (Exception e) {
				return false;
			}
		}
	}
}
