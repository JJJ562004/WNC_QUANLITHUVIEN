using System.Text.Json.Serialization;

namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }
        public string PublisherAddress { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<Book> Books { get; set; }
    }
}
