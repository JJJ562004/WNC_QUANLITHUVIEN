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
     public ActionResult Index()
{
    using (var context = new cuoiki_LTWNC.Models.WNC_QUANLYTHIVIEN_REALEntities())
    {
      
        var books = context.Books
            .OrderBy(b => b.BookID) // Sắp xếp nếu cần thiết (theo BookID)          
            .Select(b => new BookViewModel
            {
                BookID = b.BookID,
                Title = b.Title,
                Quantity = b.Quantity ?? 0, // Xử lý null cho Quantity
                Image = b.ImageURL // Sinh ảnh từ imageurl
            })
            .ToList();

        return View(books);
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