using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int PublisherID { get; set; }
        public int CategoryID { get; set; }
        public int PublishedYear { get; set; }

        public int Quantity { get; set; }

        // Navigation properties
        public Publisher Publisher { get; set; }
        public Category Category { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<BookAuthor>? BookAuthors { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public ICollection<BorrowingRecord>? BorrowingRecords{ get; set; }

        // Navigation property
        [JsonIgnore]
        [ValidateNever]
        public ICollection<StaffAddBook>? StaffAddBooks { get; set; }
    }
}
