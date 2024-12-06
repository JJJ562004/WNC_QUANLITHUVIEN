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
            using (var context = new cuoiki_LTWNC.Models.WNC_QUANLYTHUVIENEntities1())
            {
                // Kiểm tra nếu context.Books không null
                var books = context.Books.Select(b => new BookViewModel
                    {
                        BookID =b.BookID,
                        Title = b.Title,
                        Quantity = b.Quantity ?? 0, // Xử lý null cho Quantity
                        Image ="book-"+b.BookID+".png" // Sinh ảnh từ BookID
            }).ToList();

                return View(books);
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