namespace CreditApproval.DTOs;

public class SubmitCreditDTO
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public decimal CreditAmount { get; set; }
    public string Type { get; set; } = string.Empty;
}
