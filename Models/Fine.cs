namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Fine
    {
        public int FineID { get; set; }
        public int BorrowID { get; set; }
        public decimal FineAmount { get; set; }
        public bool PaidStatus { get; set; }

        // Navigation property
        public BorrowingRecord BorrowingRecord { get; set; }
    }
}
