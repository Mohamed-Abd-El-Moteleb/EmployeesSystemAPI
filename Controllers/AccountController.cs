using EmployeesSystem.Dtos.AccountDTOS;
using EmployeesSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IConfiguration _configuration;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
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
			else
			{
				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier,user.Id),
					new Claim(ClaimTypes.Name,user.UserName),
					new Claim(ClaimTypes.Role,"Admin"),
					new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
				};

				var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

				JwtSecurityToken token = new JwtSecurityToken
					(
						issuer: _configuration["Jwt:Issuer"],
						audience: _configuration["Jwt:Audience"],
						expires:DateTime.UtcNow.AddHours(1),
						claims:authClaims,
						signingCredentials:new SigningCredentials(authSignInKey,SecurityAlgorithms.HmacSha256)
					);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo
				});
			}
		}
	}
}
