using System.ComponentModel.DataAnnotations;

namespace EmployeesSystem.Dtos.AccountDTOS
{
	public class LoginDTO
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }

	}
}
