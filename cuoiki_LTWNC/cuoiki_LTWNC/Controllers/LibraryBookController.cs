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
            using (var context = new cuoiki_LTWNC.Models.WNC_QUANLYTHUVIEN_Entities2())
            {

                var books = context.Books
                    .OrderBy(b => b.BookID) // Sắp xếp nếu cần thiết (theo BookID)          
                    .Select(b => new BookViewModel
                    {
                        BookID = b.BookID,
                        Title = b.Title,
                        Quantity = b.Quantity ?? 0, // Xử lý null cho Quantity
                        Image = b.ImageURL,
                        Description = b.Description // Sinh ảnh từ BookID
                    })
                    .ToList();

                return View(books);
            }
        }
        public ActionResult Details(int? id)
        {
            using (var context = new WNC_QUANLYTHUVIEN_Entities2())
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
                        Description = b.Description// Giả định có cột mô tả
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

        public ActionResult Borrow_Book(int? id)
        {
            using (var context = new WNC_QUANLYTHUVIEN_Entities2())
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
                        Description = b.Description// Giả định có cột mô tả
                    })
                    .FirstOrDefault();

                if (book == null)
                {
                    return HttpNotFound();
                }

                return View(book);
            }
        }

        public ActionResult DanhSachPhieuMuon(int? studentID)
        {
            using (var context = new cuoiki_LTWNC.Models.WNC_QUANLYTHUVIEN_Entities2())
            {

                var borrow_records = context.Borrowing_Record
             .OrderBy(b => b.DueDate) // Sorting by DueDate
             .Join(
                 context.Books, // Join with the Book table
                 b => b.BookID, // Foreign key in Borrowing_Record
                 book => book.BookID, // Primary key in Book table
                 (b, book) => new BorrowingRecordModel
                 {
                     Title = book.Title,
                     StudentId = (int) b.StudentID,
                     BorrowID = b.BorrowID,
                     BorrowDay = b.BorrowDate,
                     DueDate = b.DueDate,
                     ReturnDate = b.ReturnDate // Keep DateTime? as is for now
                 }).Where(b => b.StudentId == studentID)
                    .ToList();
                // After retrieving data, format the dates
                foreach (var record in borrow_records)
                {
                    record.BorrowDateString = record.BorrowDay.ToString("dd/MM/yyyy"); // Short date format
                    record.DueDateString = record.DueDate.ToString("dd/MM/yyyy"); // Short date format

                    record.ReturnDateString = record.ReturnDate.HasValue
                        ? record.ReturnDate.Value.ToString("dd/MM/yyyy")
                        : "Chưa trả sách"; // If null, set the default message
                }

                return View(borrow_records);
            }
        }
    }
}