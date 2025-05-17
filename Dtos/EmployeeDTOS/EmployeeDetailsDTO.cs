using EmployeesSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EmployeesSystem.Dtos.EmployeeDTOS
{
	public class EmployeeDetailsDTO
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public DateOnly HireDate { get; set; }

		public decimal Salary { get; set; }

		public string Department { get; set; }

		public string Position { get; set; }

	}
}
