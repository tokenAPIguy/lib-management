using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace LBMS_API.Models;

public class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int ID { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    public string? MiddleInitial { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public DateOnly BirthDate { get; set; }
    
    [Required]
    public DateOnly AccountCreationDate { get; set; }
    
    [Required]
    public string Discriminator { get; set; }

    [Required]
    public UserRole Role { get; set; }
    
    public ICollection<Loan> Loans { get; set; } = new List<Loan>(); // Move to Patron
    
    public ICollection<Category> CategoryHistory { get; set; } = new List<Category>(); // Move to Patron
}

public enum UserRole {
    Patron,
    Employee,
    Admin
}