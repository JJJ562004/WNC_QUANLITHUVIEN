using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace cuoiki_LTWNC.Models
{
    public partial class LoginModel : DbContext
    {
        public LoginModel()
            : base("name=LoginModel")
        {
        }

      
        public virtual DbSet<Student> Students { get; set; }
      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);
        }
    }
}
