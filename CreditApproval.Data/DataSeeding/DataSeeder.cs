namespace CreditApproval.Data.DataSeeding;

public class DataSeeder(IServiceProvider serviceProvider) : IDataSeeder
{
    public async Task SeedUsers()
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        using CreditApprovalDbContext dbContext = scope.ServiceProvider.GetRequiredService<CreditApprovalDbContext>();

        await dbContext.Users.AddAsync(new User()
        {
            Name = "Support user",
            Role = Role.Support
        });

        await dbContext.Users.AddAsync(new User()
        {
            Name = "Manager user",
            Role = Role.Manager
        });

        await dbContext.Users.AddAsync(new User()
        {
            Name = "Admin user",
            Role = Role.Admin
        });

        await dbContext.SaveChangesAsync();
    }
}
