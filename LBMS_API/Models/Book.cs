namespace LBMS_API.Models;

public class Book {
    public Guid ID { get; set; }
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