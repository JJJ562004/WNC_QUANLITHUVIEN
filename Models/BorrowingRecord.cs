using System.Text.Json.Serialization;

namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class BorrowingRecord
    {
        public int BorrowID { get; set; }
        public int StudentID { get; set; }
        public int BookID { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Navigation properties
        public Student Student { get; set; }
        public Book Book { get; set; }

        [JsonIgnore]
        public ICollection<Fine> Fines { get; set; }
    }
}
