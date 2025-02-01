using LBMS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options) {
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { // Category
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.ID);
            
            e.Property(c => c.Name).IsRequired();
            e.Property(c => c.CanBeMainCategory).IsRequired();

        });
        
        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(b => b.ID);
            e.HasIndex(b => b.ISBN).IsUnique();

            e.Property(b => b.Name).IsRequired();
            e.Property(b => b.Author).IsRequired();
            e.Property(b => b.ISBN).IsRequired();
            e.ComplexProperty(b => b.SubCategories).IsRequired();
            e.Property(b => b.Description).IsRequired();
            e.Property(b => b.Rating);
            e.Property(b => b.IsAvailable).IsRequired();
            e.ComplexProperty(b => b.Category).IsRequired();
            
            e.HasMany(b => b.LoanHistory)
                .WithOne(b => b.Book)
                .HasForeignKey(l => l.BookID);
            
        });

        modelBuilder.Entity<Loan>(e =>
        {
            e.HasKey(l => l.ID);
            
            e.Property(l => l.BookID).IsRequired();
            e.Property(l => l.UserID).IsRequired();
            e.Property(l => l.BorrowDate).IsRequired();
            e.Property(l => l.DueDate).IsRequired();
            e.Property(l => l.ReturnedDate).IsRequired();
            e.Property(l => l.Status)
                .HasConversion(
                    l => l.ToString(), 
                    l => Enum.Parse<LoanStatus>(l))
                .IsRequired();
            
            e.HasOne(l => l.Book)
                .WithMany(b => b.LoanHistory)
                .HasForeignKey(l => l.BookID);
            e.HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserID);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.ID);
            e.HasIndex(u => u.UserName).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            
            e.Property(u => u.FirstName).IsRequired();
            e.Property(u => u.MiddleInitial);
            e.Property(u => u.LastName).IsRequired();
            e.Property(u => u.UserName).IsRequired();
            e.Property(u => u.Email).IsRequired();
            e.Property(u => u.Address).IsRequired();
            e.Property(u => u.BirthDate).IsRequired();
                
            e.HasMany(u => u.Loans)
                .WithOne(b => b.User)
                .HasForeignKey(l => l.UserID);
        });


        // User
    }
}

