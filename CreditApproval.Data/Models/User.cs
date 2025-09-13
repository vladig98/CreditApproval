namespace CreditApproval.Data.Models;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Role Role { get; set; }
}
