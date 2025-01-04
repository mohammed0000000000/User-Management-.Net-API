using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using userManagement.Models;
using userManagement.Services.Contracts;

namespace userManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MailController : ControllerBase
	{
        private readonly IMailingService _mailingService;
        public MailController(IMailingService mailingService)
        {
            _mailingService = mailingService;   
        }
        [HttpPost("Send")]
        public async Task<IActionResult> SendMail([FromForm]MailRequest mailRequest) {
            try {
                var res = await _mailingService.SendEmailAsync(mailRequest);
                return Ok(new { 
                    Sucess = res
                });
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
