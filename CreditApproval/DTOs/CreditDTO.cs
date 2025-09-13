namespace CreditApproval.DTOs;

public class CreditDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public decimal CreditAmount { get; set; }
    public string CreditType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;  
    public string Identifier { get; set; } = string.Empty;
}
