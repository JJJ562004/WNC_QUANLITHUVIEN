using Microsoft.AspNetCore.Mvc;
using BaoCaoCuoiKi_QuanLyThuVien.Models;
using BaoCaoCuoiKi_QuanLyThuVien.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoCaoCuoiKi_QuanLyThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookController(LibraryDbContext context)
        {
            _context = context;
        }

        // Get All Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // Get Book by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        // Insert Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book bookItem)
        {
            _context.Books.Add(bookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = bookItem.BookID }, bookItem);
        }

        // Update Book
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book bookItem)
        {
            if (id != bookItem.BookID)
            {
                return BadRequest();
            }

            _context.Entry(bookItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookItemExists(id))
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

        // Delete Book
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookItemExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
