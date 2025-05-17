using System.ComponentModel.DataAnnotations;

namespace EmployeesSystem.Models
{
	public class Position
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public List<Employee> Employees { get; set; } = new List<Employee>();

	}
}
