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
    [ApiExplorerSettings(GroupName = "BorrowingRecordsAPI")]
    public class BorrowingRecordController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowingRecordController(LibraryDbContext context)
        {
            _context = context;
        }

        // Get All Borrowing Records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingRecord>>> GetAllBorrowingRecords()
        {
            return await _context.Borrowing_Records
                .Include(br => br.Student)
                .Include(br => br.Book)
                .ToListAsync();
        }

        // Get Borrowing Record by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowingRecord>> GetBorrowingRecordById(int id)
        {
            var borrowingRecord = await _context.Borrowing_Records
                .Include(br => br.Student)
                .Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.BorrowID == id);

            if (borrowingRecord == null)
            {
                return NotFound(new { message = "Borrowing record not found" });
            }

            return borrowingRecord;
        }

        // Get Borrowing Records by Student ID
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<BorrowingRecord>>> GetBorrowingRecordsByStudent(int studentId)
        {
            var records = await _context.Borrowing_Records
                .Where(br => br.StudentID == studentId)
                .Include(br => br.Book)
                .ToListAsync();

            if (!records.Any())
            {
                return NotFound(new { message = "No borrowing records found for the student" });
            }

            return records;
        }

        // Get Overdue Borrowing Records
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowingRecord>>> GetOverdueBorrowingRecords()
        {
            var overdueRecords = await _context.Borrowing_Records
                .Where(br => br.DueDate < DateTime.Now && br.ReturnDate == null)
                .Include(br => br.Student)
                .Include(br => br.Book)
                .ToListAsync();

            if (!overdueRecords.Any())
            {
                return NotFound(new { message = "No overdue borrowing records found" });
            }

            return overdueRecords;
        }

        // Add Borrowing Record
        [HttpPost]
        public async Task<ActionResult<BorrowingRecord>> AddBorrowingRecord(BorrowingRecord borrowingRecord)
        {
            _context.Borrowing_Records.Add(borrowingRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBorrowingRecordById), new { id = borrowingRecord.BorrowID }, borrowingRecord);
        }

        // Batch Add Borrowing Records
        [HttpPost("batch")]
        public async Task<ActionResult> AddBorrowingRecords(IEnumerable<BorrowingRecord> borrowingRecords)
        {
            _context.Borrowing_Records.AddRange(borrowingRecords);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Borrowing records added successfully!", TotalRecordsAdded = borrowingRecords.Count() });
        }

        // Update Borrowing Record
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowingRecord(int id, BorrowingRecord borrowingRecord)
        {
            if (id != borrowingRecord.BorrowID)
            {
                return BadRequest(new { message = "Borrow ID mismatch" });
            }

            _context.Entry(borrowingRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingRecordExists(id))
                {
                    return NotFound(new { message = "Borrowing record not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Delete Borrowing Record
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowingRecord(int id)
        {
            var borrowingRecord = await _context.Borrowing_Records.FindAsync(id);
            if (borrowingRecord == null)
            {
                return NotFound(new { message = "Borrowing record not found" });
            }

            _context.Borrowing_Records.Remove(borrowingRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Batch Delete Borrowing Records
        [HttpDelete("batch")]
        public async Task<IActionResult> BatchDeleteBorrowingRecords([FromBody] List<int> recordIds)
        {
            var borrowingRecords = await _context.Borrowing_Records.Where(br => recordIds.Contains(br.BorrowID)).ToListAsync();

            if (!borrowingRecords.Any())
            {
                return NotFound(new { message = "No borrowing records found for the provided IDs." });
            }

            _context.Borrowing_Records.RemoveRange(borrowingRecords);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Batch delete completed!", TotalRecordsDeleted = borrowingRecords.Count });
        }

        // Check if Borrowing Record Exists
        private bool BorrowingRecordExists(int id)
        {
            return _context.Borrowing_Records.Any(br => br.BorrowID == id);
        }
    }
}
