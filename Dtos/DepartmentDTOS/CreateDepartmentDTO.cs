using System.ComponentModel.DataAnnotations;

namespace EmployeesSystem.Dtos.DepartmentDTOS
{
	public class CreateDepartmentDTO
	{
		[MaxLength(100)]
		public string Name { get; set; }
		public int? ManagerId { get; set; }
	}
}
