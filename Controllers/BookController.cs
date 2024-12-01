using Microsoft.AspNetCore.Mvc;
using BaoCaoCuoiKi_QuanLyThuVien.Models;
using BaoCaoCuoiKi_QuanLyThuVien.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaoCaoCuoiKi_QuanLyThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Books")]
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

        // Search Books by Title or Author
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks(string query)
        {
            var result = await _context.Books
                .Where(b => b.Title.Contains(query))
                .ToListAsync();

            if (!result.Any())
            {
                return NotFound(new { message = "No books found matching the search criteria." });
            }

            return result;
        }

        // Get Books with Pagination
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<Book>>> GetPagedBooks(int pageNumber = 1, int pageSize = 10)
        {
            var totalBooks = await _context.Books.CountAsync();
            var books = await _context.Books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalBooks = totalBooks,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Books = books
            });
        }

        // Filter Books by Category
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByCategory(int categoryId)
        {
            var books = await _context.Books.Where(b => b.CategoryID == categoryId).ToListAsync();

            if (!books.Any())
            {
                return NotFound(new { message = "No books found in the selected category." });
            }

            return books;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookDTO bookDTO)
        {
            // Map BookDTO to the Book entity
            var bookItem = new Book
            {
                Title = bookDTO.Title,
                PublisherID = bookDTO.PublisherID,
                CategoryID = bookDTO.CategoryID,
                PublishedYear = bookDTO.PublishedYear,
                Quantity = bookDTO.Quantity
            };

            _context.Books.Add(bookItem);
            await _context.SaveChangesAsync();

            // Return the created book as BookDTO (exclude BookID and navigation properties)
            return CreatedAtAction(nameof(GetBookById), new { id = bookItem.BookID }, new BookDTO
            {
                Title = bookItem.Title,
                PublisherID = bookItem.PublisherID,
                CategoryID = bookItem.CategoryID,
                PublishedYear = bookItem.PublishedYear
            });
        }

        // Batch Insert Books
        [HttpPost("batch")]
        public async Task<ActionResult> PostBooks(IEnumerable<Book> books)
        {
            _context.Books.AddRange(books);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Books added successfully!", TotalBooksAdded = books.Count() });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookDTO bookDTO)
        {

            // Find the existing book by ID
            var bookItem = await _context.Books.FindAsync(id);
            if (bookItem == null)
            {
                return NotFound();
            }

            // Map the values from BookDTO to the existing Book entity
            bookItem.Title = bookDTO.Title;
            bookItem.PublisherID = bookDTO.PublisherID;
            bookItem.CategoryID = bookDTO.CategoryID;
            bookItem.PublishedYear = bookDTO.PublishedYear;
            bookItem.Quantity = bookDTO.Quantity;

            // Mark the entity as modified
            _context.Entry(bookItem).State = EntityState.Modified;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle any concurrency issues
                if (!BookItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return the updated book (as BookDTO, excluding sensitive fields like BookID)
            return Ok(new BookDTO
            {
                Title = bookItem.Title,
                PublisherID = bookItem.PublisherID,
                CategoryID = bookItem.CategoryID,
                PublishedYear = bookItem.PublishedYear
            });
        }


        // Batch Update Books
        [HttpPut("batch")]
        public async Task<ActionResult> BatchUpdateBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                _context.Entry(book).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Batch update completed!" });
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

        // Batch Delete Books
        [HttpDelete("batch")]
        public async Task<IActionResult> BatchDeleteBooks([FromBody] List<int> bookIds)
        {
            var books = await _context.Books.Where(b => bookIds.Contains(b.BookID)).ToListAsync();

            if (!books.Any())
            {
                return NotFound(new { message = "No books found for the provided IDs." });
            }

            _context.Books.RemoveRange(books);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Batch delete completed!", TotalBooksDeleted = books.Count });
        }

        private bool BookItemExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
