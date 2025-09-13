namespace CreditApproval.Services;

public class CreditService(
    CreditApprovalDbContext dbContext,
    CreditIdentifierGenerator identifierGenerator) : ICreditService
{
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

    public async Task SubmitCreditAsync(SubmitCreditDTO data, CancellationToken token)
    {
        CreditModel credit = new()
        {
            Email = data.Email,
            Name = data.FullName,
            Status = CreditStatus.PendingReview,
            CreditAmount = data.CreditAmount,
            CreditType = Enum.Parse<CreditType>(data.Type, ignoreCase: true),
            MonthlyIncome = data.MonthlyIncome
        };

        await dbContext.Credits.AddAsync(credit, token);
        await dbContext.SaveChangesAsync(token);

        string identifier = identifierGenerator.GenerateIdentifier(credit.Id);
        credit.Identifier = identifier;

        await dbContext.SaveChangesAsync(token);
    }
}
