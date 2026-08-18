using LibraryManagementSystem.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().Property(c => c.Roles).HasConversion<string>();
            builder.Entity<Book>().HasKey(c => c.id);
            builder.Entity<Member>().HasKey(c => c.id);
            builder.Entity<Loan>().HasKey(c => c.id);
            builder.Entity<Member>().Property(c => c.Status).HasConversion<string>();
            builder.Entity<Loan>().Property(c => c.status).HasConversion<string>();
            builder.Entity<Loan>().HasOne(c => c.Member).WithMany(c => c.loans).HasForeignKey(c => c.memberId);
            builder.Entity<Loan>().HasOne(c => c.Book).WithMany(c => c.loans).HasForeignKey(c => c.bookId);
            base.OnModelCreating(builder);
        }
    }

    
}
