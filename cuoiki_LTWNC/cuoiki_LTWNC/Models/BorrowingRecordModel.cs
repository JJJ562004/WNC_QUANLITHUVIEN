using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuoiki_LTWNC.Models
{
    public class BorrowingRecordModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int StudentId { get; set; }
        public int BorrowID { get; set; }

        public DateTime BorrowDay { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string BorrowDateString { get; set; }
        public string ReturnDateString { get; set; }
        public string DueDateString { get; set; }
    }
}