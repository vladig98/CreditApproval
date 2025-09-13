namespace CreditApproval.DTOs;

public class ReviewCreditDTO
{
    public string Identifier { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public DateTime ReviewTime { get; set; }
    public string Decision { get; set; } = string.Empty;
}
