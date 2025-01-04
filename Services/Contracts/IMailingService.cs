using Microsoft.AspNetCore.Http;
using userManagement.Models;

namespace userManagement.Services.Contracts
{
	public interface IMailingService
	{
		Task <bool> SendEmailAsync(MailRequest mailRequest);
	}
}
