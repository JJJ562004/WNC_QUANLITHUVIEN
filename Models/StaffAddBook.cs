namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class StaffAddBook
    {
        public int RecordID { get; set; }
        public int StaffID { get; set; }
        public int BookID { get; set; }

        // Navigation properties
        public Staff Staff { get; set; }
        public Book Book { get; set; }
    }
}