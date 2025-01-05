using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using userManagement.Models;
using userManagement.Services.Contracts;
using userManagement.Settings;

namespace userManagement.Services.Implement
{
	public class JwtServices : IJwtServices
	{
		private readonly JwtSettings _jwt;
		private readonly UserManager<ApplicationUser> userManager;
		public JwtServices(IOptions<JwtSettings> jwt, UserManager<ApplicationUser> userManager) {
			_jwt = jwt.Value;
			this.userManager = userManager;
		}
		public async Task<string> GenerateToken(ApplicationUser user) {
			try {
				var userClaims = new List<Claim>();
				userClaims.AddRange([
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name , user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				]);

				var userRoles = await userManager.GetRolesAsync(user);
				if (userRoles.Any()) {
					userClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)).ToList());
				}
				userClaims.AddRange([
					//Used to identify the user or entity that the token is issued for.
					//Helps ensure the token is uniquely tied to a user.
					new Claim(JwtRegisteredClaimNames.Name, user.UserName),
					//A unique identifier for the token.
					new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
					//Makes it easier for downstream services to retrieve and validate the user's email without querying a database.
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
				]);

				var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecrityKey));
				//Contains the key and algorithm that will be used to sign the JWT, ensuring its integrity and authenticity
				var signInCredentialList = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);
				var tokenDescribtor = new JwtSecurityToken(issuer: _jwt.IssuerIp,
				audience: _jwt.AudienceIP,
				claims: userClaims,
				expires: DateTime.Now.AddMinutes(_jwt.AccessTokenExpired),
				signingCredentials: signInCredentialList);

				var tokenHanlder = new JwtSecurityTokenHandler();
				var token = tokenHanlder.WriteToken(tokenDescribtor);
				return token;
			} catch (Exception ex) { throw; }
		}

		RefreshToken IJwtServices.GenerateRefreshToken() {
			return new RefreshToken() {
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				CreatedOn = DateTime.UtcNow,
				ExpiresOn = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpired),
			};
		}
	}
}
