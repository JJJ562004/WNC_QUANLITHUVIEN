namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class BookAuthor
    {
        public int BookID { get; set; }
        public int AuthorID { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}
