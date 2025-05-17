using System.ComponentModel.DataAnnotations;

namespace EmployeesSystem.Dtos.PositionDTOS
{
	public class PositionCreateDTO
	{
		[MaxLength(50)]
		public string Name { get; set; }
	}
}
