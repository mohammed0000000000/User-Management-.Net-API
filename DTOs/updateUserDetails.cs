using System.ComponentModel.DataAnnotations;

namespace userManagement.DTOs
{
	public class updateUserDetails
	{	
		[Required]
		[MinLength(3)]
		public string FirstName { get; set; } = string.Empty;
		[Required]
		[MinLength(3)]
		public string LastName { get; set; } = string.Empty;
	}
}
