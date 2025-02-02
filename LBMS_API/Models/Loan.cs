using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBMS_API.Models;

public class Loan {
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ID { get; set; }
    public int BookID { get; set; }
    public int UserID { get; set; }
    public DateOnly BorrowDate { get; set; }
    public DateOnly DueDate { get; set; }
    public DateOnly? ReturnedDate { get; set; }
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