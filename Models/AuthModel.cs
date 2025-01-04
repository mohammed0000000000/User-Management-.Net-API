namespace userManagement.Models
{
	public class AuthModel
	{
        public string Message { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;

		public bool isAuthenticated { get; set; }
		public string? VerificationToken { get; set; } = string.Empty;
		public DateTime? VerifiedAt { get; set; }

		public string? PasswordResetToken { get; set; } = string.Empty;
		public DateTime? ResetPasswordExpires { get; set; }


        public string AccessToken { get; set; } = string.Empty;
        public DateTime AcessTokenExpireOn { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpireOn { get; set; }

		public List<string> Roles { get; set; } = new List<string>();
    }
}
