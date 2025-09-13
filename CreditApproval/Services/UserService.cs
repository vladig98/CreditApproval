namespace CreditApproval.Services;

public class UserService(CreditApprovalDbContext dbContext) : IUserService
{
    public bool Exists(string name)
    {
        return dbContext.Users.Any(x => x.Name == name);
    }

    public Task<User> GetByNameAsync(string reviewerName, CancellationToken token)
    {
        return dbContext.Users.FirstAsync(x => x.Name == reviewerName, token);
    }

    public bool IsAdmin(string name)
    {
        return dbContext.Users.Any(x => x.Name == name && x.Role == Role.Admin);
    }
}
