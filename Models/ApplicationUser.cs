using Microsoft.AspNetCore.Identity;

namespace userManagement.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
        public bool IsAccountDisabled { get; set; }
		public DateTime? LockoutEnd { get; set; }

		public string? VerificationToken { get; set; } = string.Empty;
        public DateTime VerificationTokenExpired { get; set; }
        public DateTime? VerifiedAt { get; set; }

		public string? PasswordResetToken { get; set; } = string.Empty;
		public DateTime? ResetPasswordExpires { get; set; }

		public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
