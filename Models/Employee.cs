using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesSystem.Models
{
	public class Employee
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Phone]
		public string Phone { get; set; }

		[Required]
		public DateOnly HireDate { get; set; }

		public decimal Salary { get; set; }

		[Required]
		[DefaultValue(1)]
		public int DepartmentId { get; set; }

		[Required]
		public int PositionId { get; set; }

		public Position Position { get; set; }
		public Department Department { get; set; }

	}

}
