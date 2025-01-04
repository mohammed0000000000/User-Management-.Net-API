namespace userManagement.DTOs
{
	public class userDto
	{
        public string?UserName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public bool Success { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
    }
}
