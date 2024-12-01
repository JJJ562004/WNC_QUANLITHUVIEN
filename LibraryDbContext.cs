using BaoCaoCuoiKi_QuanLyThuVien.Models;
using Microsoft.EntityFrameworkCore;

namespace BaoCaoCuoiKi_QuanLyThuVien.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        // DbSet properties
        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BorrowingRecord> Borrowing_Records { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<StaffAddBook> StaffAddBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");
            });

            modelBuilder.Entity<BorrowingRecord>()
            .ToTable("Borrowing_Record"); 

            modelBuilder.Entity<Student>()
            .ToTable("Student");

            modelBuilder.Entity<Author>()
            .ToTable("Author");

            modelBuilder.Entity<Publisher>()
            .ToTable("Publisher");

            modelBuilder.Entity<Category>()
            .ToTable("Category");

            modelBuilder.Entity<BookAuthor>()
            .ToTable("Book_Author");

            modelBuilder.Entity<Fine>()
            .ToTable("Fine");

            modelBuilder.Entity<Notification>()
            .ToTable("Notification");

            modelBuilder.Entity<StaffAddBook>()
            .ToTable("Staff_Add_Book");
            // Configure primary keys and relationships

            // BookAuthor (Many-to-Many)
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookID, ba.AuthorID });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookID);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorID);

            // BorrowingRecord (One-to-Many)
            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(br => br.Student)
                .WithMany(s => s.BorrowingRecords)
                .HasForeignKey(br => br.StudentID);

            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowingRecords)
                .HasForeignKey(br => br.BookID);

            modelBuilder.Entity<BorrowingRecord>()
                 .HasKey(br => br.BorrowID);

            // Fine (One-to-One)
            modelBuilder.Entity<Fine>()
                .HasOne(f => f.BorrowingRecord)
                .WithMany(br => br.Fines)
                .HasForeignKey(f => f.BorrowID);

            // Notification (One-to-Many)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Student)
                .WithMany(s => s.Notifications)
                .HasForeignKey(n => n.StudentID);

            // StaffAddBook (Many-to-Many-like relationship)
            modelBuilder.Entity<StaffAddBook>()
                .HasKey(sab => new { sab.RecordID });

            modelBuilder.Entity<StaffAddBook>()
                .HasOne(sab => sab.Staff)
                .WithMany(s => s.StaffAddBooks)
                .HasForeignKey(sab => sab.StaffID);

            modelBuilder.Entity<StaffAddBook>()
                .HasOne(sab => sab.Book)
                .WithMany(b => b.StaffAddBooks)
                .HasForeignKey(sab => sab.BookID);

            // Book (One-to-Many with Publisher and Category)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherID);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryID);
        }
    }
}
