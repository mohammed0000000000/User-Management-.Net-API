﻿using System.ComponentModel.DataAnnotations;

namespace userManagement.DTOs
{
	public class loginDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(8)]
		public string Password { get; set; } = string.Empty;
	}
}
