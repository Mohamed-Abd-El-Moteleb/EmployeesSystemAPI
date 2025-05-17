using System.ComponentModel.DataAnnotations;

namespace EmployeesSystem.Dtos.DepartmentDTOS
{
	public class UpdateDepartmentDTO
	{

		[MaxLength(100)]
		public string Name { get; set; }
		public int? ManagerId { get; set; } 
	}
}
