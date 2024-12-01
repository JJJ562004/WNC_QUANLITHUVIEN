using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        public DateTime? EnrollmentDate { get; set; }

        // Navigation properties
        [JsonIgnore]
        [ValidateNever]
        public ICollection<BorrowingRecord>? BorrowingRecords { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<Notification>? Notifications { get; set; }
    }
}
