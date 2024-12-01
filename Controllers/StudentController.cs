using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaoCaoCuoiKi_QuanLyThuVien.Models;
using BaoCaoCuoiKi_QuanLyThuVien.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace BaoCaoCuoiKi_QuanLyThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Students")]
    public class StudentController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public StudentController(LibraryDbContext context)
        {
            _context = context;
        }

        // Get All Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // Get Student By ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        // Search Students by Name or Email
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchStudents(string query)
        {
            var students = await _context.Students
                .Where(s => s.FirstName.Contains(query) || s.LastName.Contains(query) || s.Email.Contains(query))
                .ToListAsync();

            if (!students.Any())
            {
                return NotFound(new { message = "No students found matching the search criteria." });
            }

            return students;
        }

        // Filter Students By Address
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Student>>> FilterStudentsByAddress(string address)
        {
            var students = await _context.Students
                .Where(s => s.StudentAddress.Contains(address))
                .ToListAsync();

            if (!students.Any())
            {
                return NotFound(new { message = "No students found in the specified address." });
            }

            return students;
        }

        // Get Paginated Students
        [HttpGet("paged")]
        public async Task<ActionResult> GetPagedStudents(int pageNumber = 1, int pageSize = 10)
        {
            var totalStudents = await _context.Students.CountAsync();
            var students = await _context.Students
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalStudents = totalStudents,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Students = students
            });
        }

        // Add a New Student
        [HttpPost]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            // Ensure the StaffID is not manually set
            student.StudentID = 0; // Reset ID to let the database generate it

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentID }, student);
        }

        // Batch Add Students
        [HttpPost("batch")]
        public async Task<ActionResult> AddStudents(IEnumerable<Student> students)
        {
            _context.Students.AddRange(students);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Students added successfully!", TotalStudentsAdded = students.Count() });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto studentDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            // Map DTO properties to the existing student entity
            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Email = studentDto.Email;
            student.StudentAddress = studentDto.StudentAddress;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // Delete Student
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Batch Delete Students
        [HttpDelete("batch")]
        public async Task<IActionResult> BatchDeleteStudents([FromBody] List<int> studentIds)
        {
            var students = await _context.Students.Where(s => studentIds.Contains(s.StudentID)).ToListAsync();

            if (!students.Any())
            {
                return NotFound(new { message = "No students found for the provided IDs." });
            }

            _context.Students.RemoveRange(students);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Batch delete completed!", TotalStudentsDeleted = students.Count });
        }

        // Check if Student Exists
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
