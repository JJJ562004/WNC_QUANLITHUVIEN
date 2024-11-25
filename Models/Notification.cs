namespace BaoCaoCuoiKi_QuanLyThuVien.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int StudentID { get; set; }
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool ReadStatus { get; set; }

        // Navigation property
        public Student Student { get; set; }
    }
}
