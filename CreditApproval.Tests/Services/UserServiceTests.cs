namespace CreditApproval.Tests.Services;

public class UserServiceTests
{
    private static UserService GetUserService(string dbName)
    {
        DbContextOptions<CreditApprovalDbContext> options = new DbContextOptionsBuilder<CreditApprovalDbContext>().UseInMemoryDatabase(dbName).Options;
        CreditApprovalDbContext dbContext = new(options);

        dbContext.Users.Add(new User()
        {
            Id = 1,
            Name = "Test User",
            Role = Role.Admin
        });

        dbContext.SaveChanges();

        UserService userService = new(dbContext);
        return userService;
    }

    [Fact]
    public void Test_Exist_Should_Return_True()
    {
        UserService userService = GetUserService(Guid.NewGuid().ToString());
        Assert.True(userService.Exists("Test User"));
    }

    [Fact]
    public void Test_Exist_Should_Return_False()
    {
        UserService userService = GetUserService(Guid.NewGuid().ToString());
        Assert.False(userService.Exists("Fake name"));
    }

    [Fact]
    public void Test_IsAdmin_Should_Return_True()
    {
        UserService userService = GetUserService(Guid.NewGuid().ToString());
        Assert.True(userService.IsAdmin("Test User"));
    }

    [Fact]
    public void Test_IsAdmin_Should_Return_False()
    {
        UserService userService = GetUserService(Guid.NewGuid().ToString());
        Assert.False(userService.IsAdmin("Fake name"));
    }

    [Fact]
    public async Task Test_GetByName_Should_Work()
    {
        UserService userService = GetUserService(Guid.NewGuid().ToString());
        User user = await userService.GetByNameAsync("Test User", CancellationToken.None);

        Assert.Equal("Test User", user.Name);
        Assert.Equal(Role.Admin, user.Role);
        Assert.Equal(1, user.Id);
    }
}
