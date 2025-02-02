using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBMS_API.Models;

public class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string? MiddleInitial { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateOnly BirthDate { get; set; }
    public ICollection<Loan>? Loans { get; set; } = new List<Loan>();
    public ICollection<Category>? CategoryHistory { get; set; } = new List<Category>();
}