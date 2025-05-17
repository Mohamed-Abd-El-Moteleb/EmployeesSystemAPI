using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesSystem.Models;
using EmployeesSystem.Dtos.EmployeeDTOS;
using Humanizer;

namespace EmployeesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDetailsDTO>>> GetEmployees()
        {
            List<EmployeeDetailsDTO> employees = await _context.Employees
           .Include(e => e.Department)
           .Include(e => e.Position).Select(e => new EmployeeDetailsDTO
           {
               Id = e.Id,
               FirstName = e.FirstName,
               LastName = e.LastName,
               Email = e.Email,
               Phone = e.Phone,
               HireDate = e.HireDate,
               Salary = e.Salary,
               Department = e.Department != null ? e.Department.Name : "Not Assigned",
               Position = e.Position != null ? e.Position.Name : "Not Assigned"

           }).ToListAsync();

            return employees;
		}

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDetailsDTO>> GetEmployee(int id)
        {
			EmployeeDetailsDTO? employee = await _context.Employees
		        .Include(e => e.Department)
		        .Include(e => e.Position)
		        .Where(e => e.Id == id)
		        .Select(e => new EmployeeDetailsDTO
		        {
			        Id = e.Id,
			        FirstName = e.FirstName,
			        LastName = e.LastName,
			        Email = e.Email,
			        Phone = e.Phone,
			        HireDate = e.HireDate,
			        Salary = e.Salary,
			        Department = e.Department != null ? e.Department.Name : "Not Assigned",
			        Position = e.Position != null ? e.Position.Name : "Not Assigned"
		        })
		        .FirstOrDefaultAsync();

			if (employee == null)
			{
				return NotFound();
			}

			return employee;
		}

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeUpdateDTO employeeDto)
        {
            
			Employee? employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id==id);

            if (employee == null)
            {
                return NotFound();
            }

			Department? department = await _context.Departments.FindAsync(employeeDto.DepartmentId);
			Position? position = await _context.Positions.FindAsync(employeeDto.PositionId);

            if (department == null || position == null)
            {
                return BadRequest("Invalid DepartmentId or PositionId.");
            }

			employee.FirstName = employeeDto.FirstName;
			employee.LastName = employeeDto.LastName;
			employee.Email = employeeDto.Email;
			employee.Phone = employeeDto.Phone;
			employee.HireDate = employeeDto.HireDate;
			employee.Salary = employeeDto.Salary;
			employee.Department = department;
			employee.Position = position;


			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!EmployeeExists(id))
					return NotFound();
				else
					throw;
			}

			return NoContent();
        }


        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDetailsDTO>> PostEmployee(EmployeeCreateDTO employeeDto)
        {
			Department? department = await _context.Departments.FindAsync(employeeDto.DepartmentId);
			Position? position = await _context.Positions.FirstOrDefaultAsync(e => e.Id == employeeDto.PositionId);

			if (department == null || position == null)
			{
				return BadRequest("Invalid DepartmentId or PositionId.");
			}

			Employee employee = new Employee
			{
				FirstName = employeeDto.FirstName,
				LastName = employeeDto.LastName,
				Email = employeeDto.Email,
				Phone = employeeDto.Phone,
				HireDate = employeeDto.HireDate,
				Salary = employeeDto.Salary,
				Department = department,
				Position = position
			};

			_context.Employees.Add(employee);
			await _context.SaveChangesAsync();

			EmployeeDetailsDTO result = new EmployeeDetailsDTO
			{
				Id = employee.Id,
				FirstName = employee.FirstName,
                LastName= employee.LastName,
				Email = employee.Email,
				Phone = employee.Phone,
				HireDate = employee.HireDate,
				Salary = employee.Salary,
				Department = department.Name,
				Position = position.Name
			};
			return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, result);


		}

		// DELETE: api/Employees/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            Employee? employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

			try
			{
				_context.Employees.Remove(employee);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return BadRequest("Unable to delete employee due to related data.");
			}


			return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
