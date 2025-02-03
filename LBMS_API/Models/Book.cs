using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBMS_API.Models;

public class Book {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    [Required]
    public string ISBN { get; set; }
    
    [Required]
    public int CategoryID { get; set; }
    
    public string SubCategoryIDs { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    public double? Rating { get; set; }
    
    [Required]
    public bool IsAvailable { get; set; }
    
    // public ICollection<Loan>? LoanHistory { get; set; } = new List<Loan>();
}