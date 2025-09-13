namespace CreditApproval.Services.Interfaces;

public interface IUserService
{
    bool Exists(string name);
    Task<User> GetByNameAsync(string reviewerName, CancellationToken token);
    bool IsAdmin(string name);
}
