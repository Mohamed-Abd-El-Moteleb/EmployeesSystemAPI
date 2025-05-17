using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesSystem.Models;
using EmployeesSystem.Dtos.PositionDTOS;
using System.Collections;

namespace EmployeesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDTO>>> GetPositions()
        {
            IEnumerable<Position> positions = await _context.Positions.Include(p => p.Employees).ToListAsync();
            List<PositionDTO> positionDtos = new List<PositionDTO>();
			foreach (Position position in positions)
			{
                PositionDTO positionDTO = new PositionDTO();
                positionDTO.Name = position.Name;
                positionDTO.EmployeesCount = position.Employees.Count();

                positionDtos.Add(positionDTO);
			}
            return positionDtos;
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PositionDTO>> GetPosition(int id)
        {
            Position? position = await _context.Positions.Include(p=>p.Employees).FirstOrDefaultAsync(p=>p.Id==id);

            if (position == null)
            {
                return NotFound();
            }

			PositionDTO positionDTO = new PositionDTO 
            {
                Name=position.Name,
                EmployeesCount=position.Employees.Count()
            };


			return positionDTO;
        }

        // PUT: api/Positions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, PositionUpdateDTO positionDto)
        {

            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

			if (position == null)
			{
				return NotFound();
			}

			position.Name = positionDto.Name;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PositionExists(id))
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

        // POST: api/Positions
        [HttpPost]
        public async Task<ActionResult<PositionDTO>> PostPosition(PositionCreateDTO positionDto)
        {

            Position position = new Position{

                Name=positionDto.Name
            };

            _context.Positions.Add(position);

            await _context.SaveChangesAsync();

            PositionDTO result = new PositionDTO
            {
                Name=position.Name
            };

            return CreatedAtAction("GetPosition", new { id = position.Id }, result);
        }

        // DELETE: api/Positions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            Position? position = await _context.Positions.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            if (position.Id == 1)
            {
				return BadRequest("Cannot delete the default position.");

			}

			foreach (Employee employee in position.Employees)
			{
                employee.PositionId = 1;
			}

			_context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.Id == id);
        }
    }
}
