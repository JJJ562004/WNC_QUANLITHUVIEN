using cuoiki_LTWNC.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cuoiki_LTWNC.Controllers
{
    public class LibraryBookController : Controller
    {
        private WNC_QUANLYTHIVIEN_REALEntities _context = new WNC_QUANLYTHIVIEN_REALEntities();

        public ActionResult Index() { return View(); }
        public ActionResult about() { return View(); }
        public ActionResult publishers() { return View(); }
        public ActionResult staff() { return View(); }
        public ActionResult students() { return View(); }

        public ActionResult books()
        {
            ViewBag.Publishers = _context.Publishers
                .Select(p => new SelectListItem
                {
                    Value = p.PublisherID.ToString(),
                    Text = p.PublisherName
                })
                .ToList();

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName
                })
                .ToList();

            ViewBag.Books = _context.Books
                .Select(b => new 
                {
                    b.BookID,
                    b.Title,
                    b.Publisher.PublisherName,
                    b.Category.CategoryName,
                    b.Description,
                    b.ImageURL,
                    b.PublishedYear,
                    b.Quantity
                })
                .ToList();

            return View();
        }

        [HttpPost]
        public ActionResult ManageBook(Book model, string action, HttpPostedFileBase Image)
        {         
            switch (action)
            {
                case "Create":
                    return CreateBook(model, Image);
                case "Update":
                    return UpdateBook(model, Image);
                case "Delete":
                    return DeleteBook(model.Title);
                default:
                    TempData["Error"] = "Invalid action.";
                    return RedirectToAction("books");
            }          
     
        }


        private ActionResult CreateBook(Book model, HttpPostedFileBase Image)
        {
            try
            {
                // Handle Image upload
                if (Image != null && Image.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(Image.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["Error"] = "Invalid image file type.";
                        return RedirectToAction("books");
                    }

                    var fileName = Path.GetFileName(Image.FileName);
                    var directoryPath = Server.MapPath("~/Uploaded/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var filePath = Path.Combine(directoryPath, fileName);
                    Image.SaveAs(filePath);
                    model.ImageURL = "/Uploaded/" + fileName;
                }
                else
                {
                    // Handle case where image is not provided (optional step)
                    model.ImageURL = "/Uploaded/default.jpg"; // Or handle default behavior
                }

                _context.Books.Add(model);
                _context.SaveChanges();

                TempData["Message"] = "Book created successfully!";
                return RedirectToAction("books");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                return RedirectToAction("books");
            }
        }



        private ActionResult UpdateBook(Book model, HttpPostedFileBase Image)
        {
            var book = _context.Books.FirstOrDefault(b => b.Title == model.Title);
            if (book != null && ModelState.IsValid)
            {
                book.Title = model.Title;
                book.PublisherID = model.PublisherID;
                book.CategoryID = model.CategoryID;
                book.Description = model.Description;
                book.PublishedYear = model.PublishedYear;
                book.Quantity = model.Quantity;

                // Handle Image upload if new image is selected
                if (Image != null && Image.ContentLength > 0)
                {
                    // Ensure the image is valid and save it
                    var fileExtension = Path.GetExtension(Image.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["Error"] = "Invalid image file type. Only JPG, PNG, or GIF are allowed.";
                        return RedirectToAction("books");
                    }

                    var fileName = Path.GetFileName(Image.FileName);
                    var directoryPath = Server.MapPath("~/Uploaded/");

                    // Ensure the directory exists
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var filePath = Path.Combine(directoryPath, fileName);
                    Image.SaveAs(filePath);
                    book.ImageURL = "/Uploaded/" + fileName;
                }

                _context.SaveChanges();
                TempData["Message"] = "Book updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update book.";
            }
            return RedirectToAction("books");
        }

        private ActionResult DeleteBook(string bookTitle)
        {
            var book = _context.Books.FirstOrDefault(b => b.Title == bookTitle);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                TempData["Message"] = "Book deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete book.";
            }
            return RedirectToAction("books");
        }

        [HttpPost]
        public ActionResult ManageStaff(Staff model, string action)
        {
            switch (action)
            {
                case "Create":
                    return CreateStaff(model);
                case "Update":
                    return UpdateStaff(model);
                case "Delete":
                    return DeleteStaff(model.PhoneNumber);
                default:
                    TempData["Error"] = "Invalid action.";
                    return RedirectToAction("Staff");
            }
            

        }

        private ActionResult DeleteStaff(string phoneNumber)
        {
            var sta = _context.Staffs.FirstOrDefault(b => b.PhoneNumber == phoneNumber);
            if (sta != null)
            {
                _context.Staffs.Remove(sta);
                _context.SaveChanges();
                TempData["Message"] = "Staff deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete staff.";
            }
            return RedirectToAction("staff");
        }

        private ActionResult UpdateStaff(Staff model)
        {
            var sta = _context.Staffs.FirstOrDefault(b => b.PhoneNumber == model.PhoneNumber);
            if (sta != null && ModelState.IsValid)
            {
                sta.FirstName = model.FirstName;
                sta.LastName = model.LastName;
                sta.Email = model.Email;
                sta.PhoneNumber = model.PhoneNumber;
                sta.Role = model.Role;             

                _context.SaveChanges();
                TempData["Message"] = "Staff updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update staff.";
            }
            return RedirectToAction("staff");
        }

        private ActionResult CreateStaff(Staff model)
        {
            try 
             { 
                _context.Staffs.Add(model);
                _context.SaveChanges();

                TempData["Message"] = "Staff created successfully!";
                return RedirectToAction("staff");
            }
            catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred: " + ex.Message;
                    return RedirectToAction("staff");
                }
        }

        [HttpPost]
        public ActionResult ManagePublisher(Publisher model, string action)
        {
            switch (action)
            {
                case "Create":
                    return CreatePublisher(model);
                case "Update":
                    return UpdatePublisher(model);
                case "Delete":
                    return DeletePublisher(model.PublisherName);
                default:
                    TempData["Error"] = "Invalid action.";
                    return RedirectToAction("publishers");
            }


        }

        private ActionResult DeletePublisher(string name)
        {
            var pub = _context.Publishers.FirstOrDefault(b => b.PublisherName == name);
            if (pub != null)
            {
                _context.Publishers.Remove(pub);
                _context.SaveChanges();
                TempData["Message"] = "Publisher deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete publisher.";
            }
            return RedirectToAction("publishers");
        }

        private ActionResult UpdatePublisher(Publisher model)
        {
            var pub = _context.Publishers.FirstOrDefault(b => b.PublisherName == model.PublisherName);
            if (pub != null && ModelState.IsValid) 
            {
                pub.PublisherName = model.PublisherName;
                pub.PublisherAddress = model.PublisherAddress;

                _context.SaveChanges();
                TempData["Message"] = "Publisher updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update publisher.";
            }
            return RedirectToAction("publishers");
        }

        private ActionResult CreatePublisher(Publisher model)
        {
            try
            {
                _context.Publishers.Add(model);
                _context.SaveChanges();

                TempData["Message"] = "Publisher created successfully!";
                return RedirectToAction("publishers");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
                return RedirectToAction("publishers");
            }
        }

    }
}

