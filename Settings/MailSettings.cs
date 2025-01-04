namespace userManagement.Settings
{
	public class MailSettings
	{
        public string SmtpServer { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public int SmtpPort { get; set; }
        public bool UseSsl { get; set; }
    }
}
