using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaoCaoCuoiKi_QuanLyThuVien.Models;
using BaoCaoCuoiKi_QuanLyThuVien.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaoCaoCuoiKi_QuanLyThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Staffs")]
    public class StaffController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public StaffController(LibraryDbContext context)
        {
            _context = context;
        }

        // Get All Staff Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Staff>>> GetAllStaffs()
        {
            return await _context.Staff.ToListAsync();
        }

        // Get Staff by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Staff>> GetStaffById(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return staff;
        }

        // Insert New Staff
        [HttpPost]
        public async Task<ActionResult<Staff>> PostStaff(Staff staff)
        {
            // Ensure the StaffID is not manually set
            staff.StaffID = 0; // Reset ID to let the database generate it

            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStaffById), new { id = staff.StaffID }, staff);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, Staff staff)
        {
            if (id != staff.StaffID)
            {
                return BadRequest();
            }

            _context.Entry(staff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Map the updated staff to StaffResponseDTO
            var staffResponse = new StaffDTO
            {
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Email = staff.Email,
                PhoneNumber = staff.PhoneNumber,
                Role = staff.Role
            };

            return Ok(staffResponse);
        }


        // Delete Staff
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Search Staff by Name
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Staff>>> SearchStaff(string name)
        {
            return await _context.Staff
                .Where(s => s.FirstName.Contains(name) || s.LastName.Contains(name))
                .ToListAsync();
        }

        // Get Staffs by Role
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaffByRole(string role)
        {
            return await _context.Staff
                .Where(s => s.Role.Equals(role, System.StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        // Get Staffs with Pagination
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaffsPaged(int pageNumber = 1, int pageSize = 10)
        {
            var staffs = await _context.Staff
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return staffs;
        }

        // Get Staffs Count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetStaffCount()
        {
            return await _context.Staff.CountAsync();
        }

        // Check if a Staff Exists by Email
        [HttpGet("exists/{email}")]
        public async Task<ActionResult<bool>> CheckIfStaffExists(string email)
        {
            return await _context.Staff.AnyAsync(s => s.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase));
        }

        // Private Helper Method: Check if Staff Exists by ID
        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.StaffID == id);
        }
    }
}
