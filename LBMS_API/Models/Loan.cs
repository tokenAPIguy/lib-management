using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBMS_API.Models;

public class Loan {
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public Guid ID { get; set; }
    
    [Required]
    public int BookID { get; set; }
    
    [Required]
    public int UserID { get; set; }
    
    [Required]
    public DateOnly BorrowDate { get; set; }
    
    [Required]
    public DateOnly DueDate { get; set; }
    
    public DateOnly? ReturnedDate { get; set; }
    
    [Required]
    public LoanStatus Status { get; set; }
    
    // Navigation Properties
    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
}

public enum  LoanStatus {
    Active,
    Returned,
    Overdue
}