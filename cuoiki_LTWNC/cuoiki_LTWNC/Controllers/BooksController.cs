using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;
using System.Linq;

namespace cuoiki_LTWNC.Controllers
{
	public class BooksController : Controller
	{
		private readonly ApplicationDbContext _context;

		public BooksController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Display the main view
		public IActionResult Index()
		{
			var books = _context.Books
				.Include(b => b.Publisher)
				.Include(b => b.Category)
				.ToList();
			return View(books);
		}

		// Fetch data for the grid
		public IActionResult GetBooks()
		{
			var books = _context.Books
				.Include(b => b.Publisher)
				.Include(b => b.Category)
				.Select(b => new
				{
					b.BookID,
					b.Title,
					PublisherName = b.Publisher.PublisherName,
					CategoryName = b.Category.CategoryName,
					b.PublishedYear,
					b.Quantity
				})
				.ToList();
			return Json(books);
		}

		// Add a new book
		[HttpPost]
		public IActionResult AddBook(Book book)
		{
			if (ModelState.IsValid)
			{
				_context.Books.Add(book);
				_context.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		// Update an existing book
		[HttpPost]
		public IActionResult UpdateBook(Book book)
		{
			if (ModelState.IsValid)
			{
				_context.Books.Update(book);
				_context.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		// Delete a book
		[HttpPost]
		public IActionResult DeleteBook(int id)
		{
			var book = _context.Books.Find(id);
			if (book != null)
			{
				_context.Books.Remove(book);
				_context.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}
	}
}

