namespace LBMS_API.Models;

public class User {
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string? MiddleInitial { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateOnly BirthDate { get; set; }
    public ICollection<Loan>? Loans { get; set; } = new List<Loan>();
    public Category[]? CategoryHistory { get; set; } = new Category[10];
}