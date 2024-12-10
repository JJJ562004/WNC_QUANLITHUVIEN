using cuoiki_LTWNC.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;

namespace cuoiki_LTWNC.Controllers
{
    public class LibraryBookController : Controller
    {
        private WNC_QUANLYTHUVIEN_REALEntities _context = new WNC_QUANLYTHUVIEN_REALEntities();

        public ActionResult Index()
        {
            using (var context = new cuoiki_LTWNC.Models.WNC_QUANLYTHUVIEN_REALEntities())
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
            using (var context = new WNC_QUANLYTHUVIEN_REALEntities())
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
                        Description = b.Description
                    })
                    .FirstOrDefault();

                if (book == null)
                {
                    return HttpNotFound();
                }

                return View(book);
            }
        }

        public ActionResult charts()
        {
            return View();
        }

        public JsonResult GetChartDataCategory()
        {
            var data = _context.Categories
                .Select(c => new
                {
                    CategoryName = c.CategoryName,
                    BookCount = _context.Books.Count(b => b.CategoryID == c.CategoryID)
                })
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChartDataStaff()
        {
            var data = _context.Staffs
                 .GroupBy(s => s.Role) // Group by the Role
                .Select(g => new
                {
                    Role = g.Key,            // The Role being grouped
                    StaffCount = g.Count()  // Count the number of staff in each role
                })
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult books(int? bookId, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;


            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName
                })
                .ToList();

            ViewBag.Publishers = _context.Publishers
                .Select(p => new SelectListItem
                {
                    Value = p.PublisherID.ToString(),
                    Text = p.PublisherName
                })
                .ToList();

            var books = _context.Books.OrderBy(s => s.Title).ToPagedList(pageNumber, pageSize); ;

            if (bookId.HasValue)
            {
                var book = _context.Books.FirstOrDefault(b => b.BookID == bookId);
                if (book != null)
                {
                    ViewBag.Book = book;
                }
            }

            return View(books);
        }

        [HttpPost]
        public ActionResult ManageBook(Book model, string action, HttpPostedFileBase ImageURL)
        {
            if (action == "Create")
            {
                // Check if an image is uploaded
                if (ImageURL != null && ImageURL.ContentLength > 0)
                {
                    // Generate a unique file name to avoid name conflicts
                    var fileName = Path.GetFileName(ImageURL.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/images"), fileName); // Use '~' to map correctly

                    // Save the image file to the server
                    ImageURL.SaveAs(filePath);

                    // Store the relative file path in the database
                    model.ImageURL = "/images/" + fileName; // Use the relative path for easy access in the view
                }
                else
                {
                    // Set a default image if no image is selected
                    model.ImageURL = "/images/blog-img1.png"; // Default image path
                }

                // Add the new book to the context and save
                _context.Books.Add(model);
                _context.SaveChanges();
                TempData["Message"] = "Book added successfully!";
            }


            else if (action == "Update")
            {
                var existingBook = _context.Books.Find(model.BookID);
                if (existingBook != null)
                {
                    // Update existing book
                    existingBook.Title = model.Title;
                    existingBook.CategoryID = model.CategoryID;
                    existingBook.PublisherID = model.PublisherID;
                    existingBook.PublishedYear = model.PublishedYear;
                    existingBook.Quantity = model.Quantity;
                    existingBook.Description = model.Description;
                    if (ImageURL != null && ImageURL.ContentLength > 0)
                    {
                        // Generate a unique file name and save the file
                        var fileName = Path.GetFileName(ImageURL.FileName);
                        var filePath = Path.Combine(Server.MapPath("images"), fileName);
                        ImageURL.SaveAs(filePath);

                        // Save the file path in the database
                        existingBook.ImageURL = "images/" + fileName;
                    }

                    _context.SaveChanges();
                    TempData["Message"] = "Book updated successfully!";
                }
            }
            else if (action == "Delete")
            {
                var book = _context.Books.Find(model.BookID);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                    TempData["Message"] = "Book deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Book not found.";
                }
            }

            return RedirectToAction("books");
        }

        public ActionResult staff(int? staffId, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var staff = _context.Staffs.OrderBy(s => s.LastName).ToPagedList(pageNumber, pageSize);

            if (staffId.HasValue)
            {
                var stf = _context.Staffs.FirstOrDefault(b => b.StaffID == staffId);
                if (stf != null)
                {
                    ViewBag.Staff = stf;
                }
            }

            return View(staff);
        }

        [HttpPost]
        public ActionResult ManageStaff(Staff model, string action)
        {
            if (action == "Create")
            {

                _context.Staffs.Add(model);
                _context.SaveChanges();
                TempData["Message"] = "Staff added successfully!";
            }


            else if (action == "Update")
            {
                var existingStaff = _context.Staffs.Find(model.StaffID);
                if (existingStaff != null)
                {
                    // Update existing staff
                    existingStaff.FirstName = model.FirstName;
                    existingStaff.LastName = model.LastName;
                    existingStaff.Email = model.Email;
                    existingStaff.PhoneNumber = model.PhoneNumber;
                    existingStaff.Role = model.Role;
                    _context.SaveChanges();
                    TempData["Message"] = "Staff updated successfully!";
                }
            }
            else if (action == "Delete")
            {
                var stf = _context.Staffs.Find(model.StaffID);
                if (stf != null)
                {
                    _context.Staffs.Remove(stf);
                    _context.SaveChanges();
                    TempData["Message"] = "Staff deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Staff not found.";
                }
            }

            return RedirectToAction("staff");

        }


        public ActionResult publishers(int? publisherId, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var publishers = _context.Publishers.OrderBy(s => s.PublisherName).ToPagedList(pageNumber, pageSize);

            if (publisherId.HasValue)
            {
                var publisher = _context.Publishers.FirstOrDefault(b => b.PublisherID == publisherId);
                if (publisher != null)
                {
                    ViewBag.Publisher = publisher;
                }
            }

            return View(publishers);
        }

        [HttpPost]
        public ActionResult ManagePublisher(Models.Publisher model, string action)
        {
            if (action == "Create")
            {
                _context.Publishers.Add(model);
                _context.SaveChanges();
                TempData["Message"] = "Publisher added successfully!";
            }


            else if (action == "Update")
            {
                var existingPublisher = _context.Publishers.Find(model.PublisherID);
                if (existingPublisher != null)
                {
                    // Update existing staff
                    existingPublisher.PublisherName = model.PublisherName;
                    existingPublisher.PublisherAddress = model.PublisherAddress;
                    _context.SaveChanges();
                    TempData["Message"] = "Publisher updated successfully!";
                }
            }
            else if (action == "Delete")
            {
                var publisher = _context.Publishers.Find(model.PublisherID);
                if (publisher != null)
                {
                    _context.Publishers.Remove(publisher);
                    _context.SaveChanges();
                    TempData["Message"] = "Publisher deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Publisher not found.";
                }
            }

            return RedirectToAction("publishers");


        }

        public ActionResult fines(int? fineId, int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            ViewBag.Borrows = _context.Borrowing_Record
               .Select(c => new SelectListItem
               {
                   Value = c.BorrowID.ToString(),
                   Text = c.BorrowDate.ToString(),
               })
               .ToList();

            var fines = _context.Fines.OrderBy(s => s.FineID).ToPagedList(pageNumber, pageSize);

            if (fineId.HasValue)
            {
                var fine = _context.Fines.FirstOrDefault(b => b.FineID == fineId);
                if (fine != null)
                {
                    ViewBag.Fine = fine;
                }
            }

            return View(fines);
        }

        [HttpPost]
        public ActionResult ManageFine(Models.Fine model, string action)
        {
            if (action == "Create")
            {
                _context.Fines.Add(model);
                _context.SaveChanges();
                TempData["Message"] = "Fine data added successfully!";
            }


            else if (action == "Update")
            {
                var existingFine = _context.Fines.Find(model.FineID);
                if (existingFine != null)
                {
                    existingFine.BorrowID = model.BorrowID;
                    existingFine.FineAmount = model.FineAmount;
                    existingFine.PaidStatus = model.PaidStatus;
                    _context.SaveChanges();
                    TempData["Message"] = "Fine data updated successfully!";
                }
            }
            else if (action == "Delete")
            {
                var fine = _context.Fines.Find(model.FineID);
                if (fine != null)
                {
                    _context.Fines.Remove(fine);
                    _context.SaveChanges();
                    TempData["Message"] = "fine data deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "fine data not found.";
                }
            }

            return RedirectToAction("fines");


        }

    }
}

