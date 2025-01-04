using Microsoft.AspNetCore.Identity;

namespace userManagement.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
        public bool IsAccountDisabled { get; set; }
		public DateTime? LockoutEnd { get; set; }

	}
}
