using LBMS_API.Models;

namespace LBMS_API.Data.DTO;

public class LoanDTO {
    public int BookID { get; set; }
    public int UserID { get; set; }
    public LoanStatus Status { get; set; }
}