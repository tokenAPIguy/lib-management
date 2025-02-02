using LBMS_API.Models;

namespace LBMS_API.Data.DTO;

public class BookDTO {
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public Category Category { get; set; }
    public List<Category> SubCategories { get; set; } = [];
    public string Description { get; set; }
}