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
        public int? PublishYear { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? CategoryID { get; set; }
        public List<string>
    AuthorName
        { get; set; } // Danh sách tên tác giả
    }
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
    public class LibraryIndexViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public List<BookViewModel> Books { get; set; }
    }
}