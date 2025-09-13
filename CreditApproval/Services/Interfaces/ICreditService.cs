namespace CreditApproval.Services.Interfaces;

public interface ICreditService
{
    bool Exists(string identifier);
    bool IsReviewed(string identifier);
    bool CheckIfCanBeApproved(string identifier, CreditDecision decision);

    Task<List<CreditDTO>> GetAllAsync(string? creditType, string? status, CancellationToken token);
    Task ReviewCreditAsync(ReviewCreditDTO data, CancellationToken token);
    Task SubmitCreditAsync(SubmitCreditDTO data, CancellationToken token);
}
