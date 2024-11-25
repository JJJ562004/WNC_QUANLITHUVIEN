using System.Text.Json.Serialization;

namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StudentAddress { get; set; }

        // Navigation properties
        [JsonIgnore]
        public ICollection<BorrowingRecord> BorrowingRecords { get; set; }

        [JsonIgnore]
        public ICollection<Notification> Notifications { get; set; }
    }
}
