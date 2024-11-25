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

        // Navigation properties
        public Publisher Publisher { get; set; }
        public Category Category { get; set; }

        [JsonIgnore]
        public ICollection<BookAuthor> BookAuthors { get; set; }
        [JsonIgnore]
        public ICollection<BorrowingRecord> BorrowingRecords
        {
            get; set;
        }

        // Navigation property
        [JsonIgnore]
        public ICollection<StaffAddBook> StaffAddBooks { get; set; }
    }
}
