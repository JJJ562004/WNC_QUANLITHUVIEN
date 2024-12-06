using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuoiki_LTWNC.Models
{
    public class BookViewModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; } // Dùng để lưu đường dẫn ảnh
    }
}