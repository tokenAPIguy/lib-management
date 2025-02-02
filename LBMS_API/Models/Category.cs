using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBMS_API.Models;

public class Category {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required]
    public bool CanBeMainCategory { get; set; }
}