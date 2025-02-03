using System.ComponentModel.DataAnnotations;

namespace LBMS_API.Models;

public class Employee : User {
    [Required]
    public new UserRole Role { get; set; } = UserRole.Employee;
    
    // [Required]
    // public bool IsAdmin { get; set; } = false;
}
