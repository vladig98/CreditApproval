namespace CreditApproval.Data.Models;

public class CreditModel
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal CreditAmount { get; set; }
    public CreditType CreditType { get; set; }
    public CreditStatus Status { get; set; }
    public string Identifier { get; set; } = string.Empty;
}
