namespace EmployeesSystem.Dtos.EmployeeDTOS
{
	public class EmployeeUpdateDTO
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public DateOnly HireDate { get; set; }
		public decimal Salary { get; set; }
		public int DepartmentId { get; set; }
		public int PositionId { get; set; }
	}
}
