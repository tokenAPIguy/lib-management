using System.ComponentModel.DataAnnotations;

namespace LBMS_API.Models;

public class Patron : User {
    [Required]
    public new UserRole Role { get; set; } = UserRole.Patron;
}