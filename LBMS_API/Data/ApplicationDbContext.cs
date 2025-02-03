using LBMS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options) {
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { // Category
        modelBuilder.Entity<Category>(entity => {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).UseIdentityColumn();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CanBeMainCategory)
                .IsRequired();
        });
        
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).UseIdentityColumn();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Author)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ISBN)
                .HasMaxLength(20);
            
            entity.Property(e => e.CategoryID)
                .HasMaxLength(50);
            
            entity.Property(e => e.SubCategoryIDs)
                .HasMaxLength(500);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Rating)
                .IsRequired(false);

            entity.Property(e => e.IsAvailable)
                .IsRequired();
        });
        

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.ID);

            entity.Property(e => e.BookID)
                .IsRequired();

            entity.Property(e => e.UserID)
                .IsRequired();

            entity.Property(e => e.BorrowDate)
                .IsRequired();

            entity.Property(e => e.DueDate)
                .IsRequired();

            entity.Property(e => e.ReturnedDate)
                .IsRequired(false);

            entity.Property(e => e.Status)
                .IsRequired();

            // Foreign key relationships
            entity.HasOne(e => e.Book)
                .WithMany()
                .HasForeignKey(e => e.BookID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ID).UseIdentityColumn();

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(25);

            entity.Property(e => e.MiddleInitial)
                .HasMaxLength(1)
                .IsRequired(false);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasIndex(e => e.UserName).IsUnique();

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(200);

            entity.Property(e => e.BirthDate)
                .IsRequired();

            entity.Property(e => e.AccountCreationDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.Discriminator)
                .HasMaxLength(50);

            entity.Property(e => e.Role)
                .HasMaxLength(50);
        });
    }
}

