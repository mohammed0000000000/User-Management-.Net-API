using System.ComponentModel.DataAnnotations;

namespace userManagement.DTOs
{
	public class verifyEmailDto
	{
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
