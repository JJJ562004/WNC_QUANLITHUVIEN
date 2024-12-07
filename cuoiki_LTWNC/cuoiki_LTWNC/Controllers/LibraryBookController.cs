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
    using (var context = new cuoiki_LTWNC.Models.WNC_QUANLYTHUVIENEntities2())
    {
      
        var books = context.Books
            .OrderBy(b => b.BookID) // Sắp xếp nếu cần thiết (theo BookID)          
            .Select(b => new BookViewModel
            {
                BookID = b.BookID,
                Title = b.Title,
                Quantity = b.Quantity ?? 0, // Xử lý null cho Quantity
                Image = "book-" + b.BookID + ".png" // Sinh ảnh từ BookID
            })
            .ToList();

        return View(books);
    }
}
        public ActionResult Details(int? id)
        {
            using (var context = new WNC_QUANLYTHUVIENEntities2())
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
                Image = "book-" + b.BookID + ".png",
                        Description = "Đây là 1 sách rất hay." // Giả định có cột mô tả
            })
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