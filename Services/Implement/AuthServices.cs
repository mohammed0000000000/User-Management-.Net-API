using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using userManagement.DTOs;
using userManagement.Models;
using userManagement.Services.Contracts;
using userManagement.Settings;

namespace userManagement.Services.Implement
{
	public class AuthServices : IAuthServices {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IJwtServices JwtServices;
		private readonly JwtSettings jwt;
		private readonly IMailingService mailingService;
		public AuthServices(UserManager<ApplicationUser> userManager, IJwtServices JwtServices, IOptions<JwtSettings> jwt, IMailingService mailingService) {
			this.userManager = userManager;
			this.JwtServices = JwtServices;
			this.jwt = jwt.Value;
			this.mailingService = mailingService;
		}
		public async Task<AuthModel> RegisterAsync(registerDto model) {
			try {
				if (await userManager.Users.AnyAsync(x => x.UserName == $"{model.FirstName} {model.LastName}"))
					return new AuthModel() {
						Success = false,
						Message = "Username Already Use!."
					};
				var checkEmail = await userManager.Users.AnyAsync(x => x.Email == model.Email);
				if (checkEmail) {
					return new AuthModel() {
						Message = "Email Already Use!.",
						Success = false
					};
				}
				var newUser = new ApplicationUser() {
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName,
					UserName = $"{model.FirstName} {model.LastName}",
					PasswordHash = model.Password,
					VerificationToken = GenerateVerifyNumber(),
				};
				var createdUser = await userManager.CreateAsync(newUser, model.Password);
				if (!createdUser.Succeeded) {
					var errorMessage = string.Join(" ", createdUser.Errors.Select(x => x.Description));
					return new AuthModel() {
						Message = errorMessage,
						Success = false
					};
				}
				var mailRequest = new MailRequest() {
					ToEmail = model.Email,
					Subject = "Successful Registeration",
					Body = "Registration Successful! Welcome to [Your Application Name]. Please check your email to verify your account and activate it. If you don’t see the email, be sure to check your spam or junk folder."
				};
				await mailingService.SendEmailAsync(mailRequest);
				var accessToken = await JwtServices.GenerateToken(newUser);

				string verificationToken = GenerateRandomToken();
				newUser.VerificationToken = verificationToken;
				newUser.VerificationTokenExpired = DateTime.UtcNow.AddHours(6);

				await userManager.UpdateAsync(newUser);
				return new AuthModel() {
					Success = true,
					UserName = newUser.UserName,
					Email = newUser.Email,
					AccessToken = accessToken,
					AcessTokenExpireOn = DateTime.UtcNow.AddMinutes(jwt.AccessTokenExpired),
					VerificationToken = verificationToken
				};
			} catch (Exception ex) {
				return new AuthModel() {
					Success = false,
					Message = $"An unexpected error occurred while register the user."
				};
			}
		}
		public async Task<AuthModel> LoginAsync(loginDto model) {
			try {
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user is null)
					return new AuthModel() {
						Success = false,
						Message = "Email Not Found"
					};
				if( user.VerificationToken is not null && user.VerifiedAt is null){
					return new AuthModel() {
						Success = false,
						Message = "This Email Not Verifyed.\n Please Check your email."
					};
				}
				if (await userManager.CheckPasswordAsync(user, model.Password) is false)
					return new AuthModel() {
						Success = false,
						Message = "InCorrect Password",
					};
				var accessToken = await JwtServices.GenerateToken(user);
				var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.isActive);
				if (refreshToken is null) {
					refreshToken = JwtServices.GenerateRefreshToken();
					user.RefreshTokens.Add(refreshToken);
					await userManager.UpdateAsync(user);
				}

				return new AuthModel() {
					Success = true,
					Message = "User Login successfully",
					AccessToken = accessToken,
					AcessTokenExpireOn = DateTime.UtcNow.AddMinutes(jwt.AccessTokenExpired),
					RefreshToken = refreshToken.Token,
					Email = model.Email,
					UserName = user.UserName,
					isAuthenticated = true
				};
			} catch (Exception ex) {
				return new AuthModel() {
					Success = false,
					Message = "An unexpected error occured while login user",
				};
			}
		}

		public async Task<AuthModel> RefreshTokenAsync(string token) {
			try {// validate on token 
				var user = await userManager.Users.SingleOrDefaultAsync(user => user.RefreshTokens.Any(x => x.Token == token && x.isActive));
				if (user is null)
					return new AuthModel() {
						Success = false,
						Message = "InValid Token"
					};
				var refreshToken = user.RefreshTokens.First(x => x.Token == token);
				refreshToken.RevokedOn = DateTime.UtcNow;

				var newRefreshToken = JwtServices.GenerateRefreshToken();
				user.RefreshTokens.Add(refreshToken);
				await userManager.UpdateAsync(user);

				var accessToken = await JwtServices.GenerateToken(user);

				return new AuthModel() {
					AccessToken = accessToken,
					AcessTokenExpireOn = DateTime.UtcNow.AddMinutes(jwt.AccessTokenExpired),
					RefreshToken = newRefreshToken.Token,
					RefreshTokenExpireOn = newRefreshToken.ExpiresOn,
				};
			} catch (Exception ex) {
				return new AuthModel() {
					Success = false,
					Message = ex.Message,
				};
			}
		}

		public async Task<bool> RefreshTokenRevokeAsync(string token) {
			try {
				var user = await userManager.Users.SingleOrDefaultAsync(user => user.RefreshTokens.Any(x => x.Token == token && x.isActive));
				if (user is null)
					return false;
				var refreshToken = user.RefreshTokens.First(x => x.Token == token);
				refreshToken.RevokedOn = DateTime.UtcNow;
				return true;
			} catch (Exception ex) {
				return false;
			}
		}
		public async Task<bool> VerifyEmail(string token) {
			try {
				/*
				 * to Verify Email 
				 */
			} catch (Exception ex) {
				throw;
			}
		}
		private string GenerateRandomToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		private string GenerateVerifyNumber() => (new Random().Next(100000, 999999));
	}
}
