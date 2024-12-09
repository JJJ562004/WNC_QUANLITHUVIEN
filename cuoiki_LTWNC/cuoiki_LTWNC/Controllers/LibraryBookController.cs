using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using cuoiki_LTWNC.Models;
namespace cuoiki_LTWNC.Controllers
{
    public class LibraryBookController : Controller
    {
        // GET: LibraryBook
        public ActionResult Index(int? categoryId)
        {
            using (var context = new cuoiki_LTWNC.Models.WNC_QUANLYTHIVIEN_REALEntities())
            {
                var categories = context.Categories
                    .Select(c => new CategoryViewModel
                    {
                        CategoryID = c.CategoryID,
                        CategoryName = c.CategoryName
                    })
                    .ToList();

                var booksQuery = context.Books.AsQueryable();

                if (categoryId.HasValue)
                {
                    booksQuery = booksQuery.Where(b => b.CategoryID == categoryId);
                }

                var books = booksQuery
                    .OrderBy(b => b.BookID)
                    .Select(b => new BookViewModel
                    {
                        BookID = b.BookID,
                        Title = b.Title,
                        Quantity = b.Quantity ?? 0,
                        Image = b.ImageURL,
                        CategoryID = b.CategoryID
                    })
                    .ToList();

                var viewModel = new LibraryIndexViewModel
                {
                    Categories = categories,
                    Books = books
                };

                return View(viewModel);
            }
        }


        public ActionResult Details(int? id)
        {
            using (var context = new WNC_QUANLYTHIVIEN_REALEntities())
            {
                var book = context.Books
                    .Where(b => b.BookID == id)
                    .Select(b => new BookViewModel
                    {
                        BookID = b.BookID,
                        Title = b.Title,
                        PublishYear = b.PublishedYear,
                        Quantity = b.Quantity ?? 0,
                        AuthorName = b.Authors.Select(a => a.AuthorName).ToList(), // Lấy danh sách tên tác giả
                        Image = b.ImageURL,
                        Description = b.Description            })
                    .FirstOrDefault();

                if (book == null)
                {
                    return HttpNotFound();
                }

                return View(book);
            }
        }




        public ActionResult about()
        {
            return View();
        }

        public ActionResult blog()
        {
            return View();
        }

        public ActionResult coffees()
        {
            return View();
        }

        public ActionResult contact()
        {
            return View();
        }
    }
}