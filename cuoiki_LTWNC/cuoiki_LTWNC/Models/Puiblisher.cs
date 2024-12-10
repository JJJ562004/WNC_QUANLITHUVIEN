namespace cuoiki_LTWNC.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }
        public string PublisherAddress { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}