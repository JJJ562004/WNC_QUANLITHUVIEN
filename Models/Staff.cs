using System.Text.Json.Serialization;

namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<StaffAddBook> StaffAddBooks { get; set; }
    }
}
