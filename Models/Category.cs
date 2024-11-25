using System.Text.Json.Serialization;

namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<Book> Books { get; set; }
    }
}
