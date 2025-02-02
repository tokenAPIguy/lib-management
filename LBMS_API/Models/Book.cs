using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBMS_API.Models;

public class Book {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public Category Category { get; set; }
    public List<Category> SubCategories { get; set; } = [];
    public string Description { get; set; }
    public double? Rating { get; set; }
    public bool IsAvailable { get; set; }
    public ICollection<Loan>? LoanHistory { get; set; } = new List<Loan>();
}