﻿using System.ComponentModel.DataAnnotations;

namespace userManagement.DTOs
{
	public class resetPasswordDto
	{
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
