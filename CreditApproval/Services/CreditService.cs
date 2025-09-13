namespace CreditApproval.Services;

public class CreditService(
    CreditApprovalDbContext dbContext,
    CreditIdentifierGenerator identifierGenerator,
    IUserService userService) : ICreditService
{
    private const decimal CREDIT_REJECTION_MULTIPLIER = 20M;

    public bool CheckIfCanBeApproved(string identifier, CreditDecision decision)
    {
        CreditModel credit = dbContext.Credits.First(x => x.Identifier == identifier);

        if (credit.CreditAmount > credit.MonthlyIncome * CREDIT_REJECTION_MULTIPLIER && decision == CreditDecision.Approve)
        {
            return false;
        }

        return true;
    }

    public bool Exists(string identifier)
    {
        return dbContext.Credits.Any(x => x.Identifier == identifier);
    }

    public async Task<List<CreditDTO>> GetAllAsync(string? creditType, string? status, CancellationToken token)
    {
        IQueryable<CreditModel> query = dbContext.Credits.AsQueryable();

        if (!string.IsNullOrWhiteSpace(creditType))
        {
            CreditType type = Enum.Parse<CreditType>(creditType, ignoreCase: true);

            query = query.Where(x => x.CreditType == type);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            CreditStatus statusEnum = Enum.Parse<CreditStatus>(status, ignoreCase: true);

            query = query.Where(x => x.Status == statusEnum);
        }

        CreditModel[] credits = await query.ToArrayAsync(token);
        List<CreditDTO> dtos = [.. credits.Select(x => new CreditDTO()
        {
            CreditAmount = x.CreditAmount,
            Email = x.Email,
            Name = x.Name,
            MonthlyIncome = x.MonthlyIncome,
            CreditType = x.CreditType.ToString(),
            Identifier = x.Identifier,
            Status = x.Status.ToString()
        })];

        return dtos;
    }

    public bool IsReviewed(string identifier)
    {
        return dbContext.Credits.First(x => x.Identifier == identifier).Status != CreditStatus.PendingReview;
    }

    public async Task ReviewCreditAsync(ReviewCreditDTO data, CancellationToken token)
    {
        CreditModel credit = await dbContext.Credits.FirstAsync(x => x.Identifier == data.Identifier, token);
        CreditDecision decision = Enum.Parse<CreditDecision>(data.Decision, ignoreCase: true);

        credit.Status = decision switch
        {
            CreditDecision.Approve => CreditStatus.Approved,
            CreditDecision.Reject => CreditStatus.Rejected,
            _ => CreditStatus.PendingReview
        };

        User reviewer = await userService.GetByNameAsync(data.ReviewerName, token);

        credit.Reviewer = reviewer;
        credit.ReviewTime = data.ReviewTime;

        await dbContext.SaveChangesAsync(token);
    }

    public async Task SubmitCreditAsync(SubmitCreditDTO data, CancellationToken token)
    {
        CreditModel credit = new()
        {
            Email = data.Email,
            Name = data.FullName,
            Status = CreditStatus.PendingReview,
            CreditAmount = data.CreditAmount,
            CreditType = Enum.Parse<CreditType>(data.Type, ignoreCase: true),
            MonthlyIncome = data.MonthlyIncome,
            Identifier = identifierGenerator.GenerateIdentifier()
        };

        await dbContext.Credits.AddAsync(credit, token);
        await dbContext.SaveChangesAsync(token);
    }
}
