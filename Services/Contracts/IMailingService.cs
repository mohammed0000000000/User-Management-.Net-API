using Microsoft.AspNetCore.Http;
using userManagement.Models;

namespace userManagement.Services.Contracts
{
	public interface IMailingService
	{
		Task <bool> SendEmailAsync(MailRequest mailRequest);
		string GenerateEmailVerificationTextBody(string username, string verificationLink);
		string GenerateEmailVerificationTextBody(string username, int verificationNumber);
	}
}
