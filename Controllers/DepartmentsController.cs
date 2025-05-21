using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesSystem.Models;
using Humanizer;
using EmployeesSystem.Dtos.DepartmentDTOS;
using Microsoft.AspNetCore.Authorization;

namespace EmployeesSystem.Controllers
{
	[Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: api/Departments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        //{
        //    IEnumerable<Department> departments = await _context.Departments.ToListAsync();
        //    return Ok(departments);
        //}

        [HttpGet("Count")]
		public async Task<ActionResult<IEnumerable<DeptWithEmpsCountDTO>>> GetDepartmentsWithCount()
		{
			IEnumerable<Department> departments = await _context.Departments.Include(d => d.Manager).Include(d => d.Employees).ToListAsync();

			List<DeptWithEmpsCountDTO> departmentsDto = new List<DeptWithEmpsCountDTO>();

			foreach (Department Dept in departments)
			{
                DeptWithEmpsCountDTO deptWithEmpsCountDTO = new DeptWithEmpsCountDTO();
				deptWithEmpsCountDTO.Name = Dept.Name;
                deptWithEmpsCountDTO.ManagerName=Dept.Manager!= null ? $"{Dept.Manager.FirstName} {Dept.Manager.LastName}" : "Not Assigned";

				deptWithEmpsCountDTO.EmployeesCount = Dept.Employees.Count();

                departmentsDto.Add(deptWithEmpsCountDTO);
			}

			return departmentsDto;
		}

		// GET: api/Departments/5
		[HttpGet("{id}")]
        public async Task<ActionResult<DeptWithEmpsCountDTO>> GetDepartment(int id)
        {
            Department? department = await _context.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);

			if (department == null)
            {
                return NotFound();
            }
            DeptWithEmpsCountDTO departmentDto = new DeptWithEmpsCountDTO
            {
                Name = department.Name,
                ManagerName = department.Manager != null ? $"{department.Manager.FirstName} {department.Manager.LastName}" : "Not Assigned",
                EmployeesCount = department.Employees.Count()
            };

			return departmentDto;
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, UpdateDepartmentDTO departmentDto)
        {

			Department? department = await _context.Departments.FindAsync(id);

			if (department == null)
			{
				return NotFound();
			}
			department.Name = departmentDto.Name;
			department.ManagerId = departmentDto.ManagerId;


			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DepartmentExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DeptWithEmpsCountDTO>> PostDepartment(CreateDepartmentDTO departmentDto)
        {
			Department department = new Department
			{
				Name = departmentDto.Name,
				ManagerId = departmentDto.ManagerId
			};
			_context.Departments.Add(department);
            await _context.SaveChangesAsync();

			await _context.Entry(department)
			.Reference(d => d.Manager)
			.LoadAsync();

			DeptWithEmpsCountDTO result = new DeptWithEmpsCountDTO
			{
				Name=department.Name,
				ManagerName = department.Manager != null ? $"{department.Manager.FirstName} {department.Manager.LastName}" : "Not Assigned",
			};

            return CreatedAtAction("GetDepartment", new { id = department.Id }, result);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
			Department? department = await _context.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);

			if (department == null)
			{
				return NotFound();
			}

			if (department.Id == 1)
			{
				return BadRequest("Cannot delete the default department.");
			}

			foreach (Employee emp in department.Employees)
			{
				emp.DepartmentId = 1;
			}

			_context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
