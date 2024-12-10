using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cuoiki_LTWNC.Models
{
    public class LibraryIndexViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public List<BookViewModel> Books { get; set; }
    }
}