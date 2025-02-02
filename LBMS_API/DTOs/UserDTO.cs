namespace LBMS_API.Data.DTO;

public class UserDTO {
    public string FirstName { get; set; }
    public string? MiddleInitial { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateOnly BirthDate { get; set; }
}