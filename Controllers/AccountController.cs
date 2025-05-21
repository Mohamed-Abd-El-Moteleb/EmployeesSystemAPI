using EmployeesSystem.Dtos.AccountDTOS;
using EmployeesSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _userManager.FindByNameAsync(model.UserName);

			if (user == null)
				return Unauthorized("Invalid username or password.");

			if(!await _userManager.IsInRoleAsync(user, "Admin"))
			{
				return Forbid("Access denied. Admins only.");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (!result.Succeeded)
			{
				return Unauthorized("Invalid username or password.");
			}
			return Ok(new { message = "Login successful." });

		}
	}
}
