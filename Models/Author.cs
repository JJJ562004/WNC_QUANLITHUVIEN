using System.Text.Json.Serialization;

namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
