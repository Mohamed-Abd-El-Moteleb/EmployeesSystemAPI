using System.ComponentModel.DataAnnotations;

namespace EmployeesSystem.Models
{
	public class Department
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		public int? ManagerId { get; set; }
		public Employee Manager { get; set; }
		public List<Employee> Employees { get; set; } = new List<Employee>();
	}
}
