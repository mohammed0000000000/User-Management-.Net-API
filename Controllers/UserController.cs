using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using userManagement.DTOs;
using userManagement.Services.Contracts;

namespace userManagement.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IAuthServices authServices;
		public UserController(IAuthServices authServices) {
			this.authServices = authServices;
		}
		[HttpPost("SignUp")]
		public async Task<IActionResult> SignUpAsync(registerDto registerDto) {
			try {
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				var res = await authServices.RegisterAsync(registerDto);
				if (!res.Success)
					return BadRequest(new {
						Message = res.Message
					});
				return Ok(res);
			} catch (Exception ex) {
				throw;
			}
		}
		[HttpPost("SignIn")]
		public async Task<IActionResult> SignInAsync(loginDto loginDto) {
			try {
				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				var response = await authServices.LoginAsync(loginDto);
				if (!response.Success)
					return BadRequest(new {
						Message = response.Message
					});
				return Ok(response);
			} catch (Exception ex) {
				throw;
			}
		}
	}
}
