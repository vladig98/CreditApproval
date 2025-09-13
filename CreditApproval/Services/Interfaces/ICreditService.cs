namespace CreditApproval.Services.Interfaces;

public interface ICreditService
{
    Task<List<CreditDTO>> GetAllAsync(string? creditType, string? status, CancellationToken token);
    Task SubmitCreditAsync(SubmitCreditDTO data, CancellationToken token);
}
