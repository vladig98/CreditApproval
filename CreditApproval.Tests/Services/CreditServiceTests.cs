namespace CreditApproval.Tests.Services;

public class CreditServiceTests
{
    private readonly Mock<IUserService> _userService = new Mock<IUserService>();
    private readonly Mock<CreditIdentifierGenerator> _creditIdentifierGenerator = new Mock<CreditIdentifierGenerator>();

    private static CreditModel GetCredit()
    {
        return new CreditModel()
        {
            Id = 1,
            CreditAmount = 1000,
            CreditType = CreditType.Mortgage,
            Email = "test@gmail.com",
            Identifier = "CRE-20250901-0001",
            Name = "Test User",
            MonthlyIncome = 500,
            Status = CreditStatus.PendingReview
        };
    }

    private static CreditApprovalDbContext GetDbContext(string dbName)
    {
        DbContextOptions<CreditApprovalDbContext> options = new DbContextOptionsBuilder<CreditApprovalDbContext>().UseInMemoryDatabase(dbName).Options;
        CreditApprovalDbContext dbContext = new(options);

        return dbContext;
    }

    private CreditService GetCreditService(CreditApprovalDbContext dbContext)
    {
        CreditService creditService = new(dbContext, _creditIdentifierGenerator.Object, _userService.Object);
        return creditService;
    }

    [Fact]
    public void Test_Check_If_Can_Be_Approved_Should_Fail()
    {
        CreditModel credit = GetCredit();
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        credit.CreditAmount = 100_000;
        credit.MonthlyIncome = 100;

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.False(creditService.CheckIfCanBeApproved(credit.Identifier, CreditDecision.Approve));
    }

    [Fact]
    public void Test_Check_If_Can_Be_Approved_Should_Succeed_Reject_Decision_Exceeding_Limit()
    {
        CreditModel credit = GetCredit();
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        credit.CreditAmount = 100_000;
        credit.MonthlyIncome = 100;

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.True(creditService.CheckIfCanBeApproved(credit.Identifier, CreditDecision.Reject));
    }

    [Fact]
    public void Test_Check_If_Can_Be_Approved_Should_Succeed_Reject_Decision_Within_Limit()
    {
        CreditModel credit = GetCredit();
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        credit.CreditAmount = 100_000;
        credit.MonthlyIncome = 100;

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.True(creditService.CheckIfCanBeApproved(credit.Identifier, CreditDecision.Reject));
    }

    [Fact]
    public void Test_Check_If_Can_Be_Approved_Should_Succeed()
    {
        CreditModel credit = GetCredit();
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        credit.CreditAmount = 1_000;
        credit.MonthlyIncome = 100;

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.True(creditService.CheckIfCanBeApproved(credit.Identifier, CreditDecision.Approve));
    }

    [Fact]
    public void Test_Exists_Should_Return_True()
    {
        CreditModel credit = GetCredit();
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.True(creditService.Exists(credit.Identifier));
    }

    [Fact]
    public void Test_Exists_Should_Return_False()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        Assert.False(creditService.Exists("Test"));
    }

    [Fact]
    public async Task Test_Get_All_Type_Mortgage()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel mortgage = GetCredit();
        mortgage.Id = 1;
        mortgage.CreditType = CreditType.Mortgage;
        mortgage.Status = CreditStatus.PendingReview;

        CreditModel auto = GetCredit();
        auto.Id = 2;
        auto.CreditType = CreditType.Auto;
        auto.Status = CreditStatus.Approved;

        CreditModel personal = GetCredit();
        personal.Id = 3;
        personal.CreditType = CreditType.Personal;
        personal.Status = CreditStatus.Rejected;

        dbContext.Credits.AddRange(mortgage, auto, personal);
        dbContext.SaveChanges();

        Assert.Single(await creditService.GetAllAsync("mortgage", null, CancellationToken.None));
    }

    [Fact]
    public async Task Test_Get_All_Type_Auto()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel mortgage = GetCredit();
        mortgage.Id = 1;
        mortgage.CreditType = CreditType.Mortgage;
        mortgage.Status = CreditStatus.PendingReview;

        CreditModel auto = GetCredit();
        auto.Id = 2;
        auto.CreditType = CreditType.Auto;
        auto.Status = CreditStatus.Approved;

        CreditModel personal = GetCredit();
        personal.Id = 3;
        personal.CreditType = CreditType.Personal;
        personal.Status = CreditStatus.Rejected;

        dbContext.Credits.AddRange(mortgage, auto, personal);
        dbContext.SaveChanges();

        Assert.Single(await creditService.GetAllAsync("auto", null, CancellationToken.None));
    }

    [Fact]
    public async Task Test_Get_All_Type_Personal()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel mortgage = GetCredit();
        mortgage.Id = 1;
        mortgage.CreditType = CreditType.Mortgage;
        mortgage.Status = CreditStatus.PendingReview;

        CreditModel auto = GetCredit();
        auto.Id = 2;
        auto.CreditType = CreditType.Auto;
        auto.Status = CreditStatus.Approved;

        CreditModel personal = GetCredit();
        personal.Id = 3;
        personal.CreditType = CreditType.Personal;
        personal.Status = CreditStatus.Rejected;

        dbContext.Credits.AddRange(mortgage, auto, personal);
        dbContext.SaveChanges();

        Assert.Single(await creditService.GetAllAsync("personal", null, CancellationToken.None));
    }

    [Fact]
    public async Task Test_Get_All_Status_Pending()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel mortgage = GetCredit();
        mortgage.Id = 1;
        mortgage.CreditType = CreditType.Mortgage;
        mortgage.Status = CreditStatus.PendingReview;

        CreditModel auto = GetCredit();
        auto.Id = 2;
        auto.CreditType = CreditType.Auto;
        auto.Status = CreditStatus.Approved;

        CreditModel personal = GetCredit();
        personal.Id = 3;
        personal.CreditType = CreditType.Personal;
        personal.Status = CreditStatus.Rejected;

        dbContext.Credits.AddRange(mortgage, auto, personal);
        dbContext.SaveChanges();

        Assert.Single(await creditService.GetAllAsync(null, "pendingreview", CancellationToken.None));
    }

    [Fact]
    public async Task Test_Get_All_Status_Approved()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel mortgage = GetCredit();
        mortgage.Id = 1;
        mortgage.CreditType = CreditType.Mortgage;
        mortgage.Status = CreditStatus.PendingReview;

        CreditModel auto = GetCredit();
        auto.Id = 2;
        auto.CreditType = CreditType.Auto;
        auto.Status = CreditStatus.Approved;

        CreditModel personal = GetCredit();
        personal.Id = 3;
        personal.CreditType = CreditType.Personal;
        personal.Status = CreditStatus.Rejected;

        dbContext.Credits.AddRange(mortgage, auto, personal);
        dbContext.SaveChanges();

        Assert.Single(await creditService.GetAllAsync(null, "approved", CancellationToken.None));
    }

    [Fact]
    public async Task Test_Get_All_Status_Rejected()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel mortgage = GetCredit();
        mortgage.Id = 1;
        mortgage.CreditType = CreditType.Mortgage;
        mortgage.Status = CreditStatus.PendingReview;

        CreditModel auto = GetCredit();
        auto.Id = 2;
        auto.CreditType = CreditType.Auto;
        auto.Status = CreditStatus.Approved;

        CreditModel personal = GetCredit();
        personal.Id = 3;
        personal.CreditType = CreditType.Personal;
        personal.Status = CreditStatus.Rejected;

        dbContext.Credits.AddRange(mortgage, auto, personal);
        dbContext.SaveChanges();

        Assert.Single(await creditService.GetAllAsync(null, "rejected", CancellationToken.None));
    }

    [Fact]
    public async Task Test_Get_All_Should_Return_All()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel mortgage = GetCredit();
        mortgage.Id = 1;
        mortgage.CreditType = CreditType.Mortgage;
        mortgage.Status = CreditStatus.PendingReview;

        CreditModel auto = GetCredit();
        auto.Id = 2;
        auto.CreditType = CreditType.Auto;
        auto.Status = CreditStatus.Approved;

        CreditModel personal = GetCredit();
        personal.Id = 3;
        personal.CreditType = CreditType.Personal;
        personal.Status = CreditStatus.Rejected;

        dbContext.Credits.AddRange(mortgage, auto, personal);
        dbContext.SaveChanges();

        List<CreditDTO> credits = await creditService.GetAllAsync(null, null, CancellationToken.None);

        Assert.Equal(3, credits.Count);

        CreditDTO first = credits[0];

        Assert.Equal(mortgage.Name, first.Name);
        Assert.Equal(mortgage.Email, first.Email);
        Assert.Equal(mortgage.MonthlyIncome, first.MonthlyIncome);
        Assert.Equal(mortgage.CreditAmount, first.CreditAmount);
        Assert.Equal(mortgage.CreditType.ToString(), first.CreditType);
        Assert.Equal(mortgage.Status.ToString(), first.Status);
        Assert.Equal(mortgage.Identifier, first.Identifier);
    }

    [Fact]
    public async Task Test_Get_All_Should_Return_Empty()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        Assert.Empty(await creditService.GetAllAsync(null, null, CancellationToken.None));
    }

    [Fact]
    public void Test_Is_Reviewed_Should_Return_True_Approved()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel credit = GetCredit();
        credit.Status = CreditStatus.Approved;

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.True(creditService.IsReviewed(credit.Identifier));
    }

    [Fact]
    public void Test_Is_Reviewed_Should_Return_True_Rejected()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel credit = GetCredit();
        credit.Status = CreditStatus.Rejected;

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.True(creditService.IsReviewed(credit.Identifier));
    }

    [Fact]
    public void Test_Is_Reviewed_Should_Return_False()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel credit = GetCredit();

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        Assert.False(creditService.IsReviewed(credit.Identifier));
    }

    [Fact]
    public async Task Test_Review_Credit_Should_Succeed_Approve()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel credit = GetCredit();

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        User user = new()
        {
            Name = "User",
            Id = 1
        };

        dbContext.Users.Add(user);
        dbContext.SaveChanges();

        _userService.Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(user));

        ReviewCreditDTO reviewCreditDTO = new()
        {
            Identifier = credit.Identifier,
            Decision = "approve",
            ReviewerName = user.Name,
            ReviewTime = DateTime.Now
        };

        await creditService.ReviewCreditAsync(reviewCreditDTO, CancellationToken.None);
        CreditModel? dbEntry = dbContext.Credits.Include(x => x.Reviewer)
            .FirstOrDefault(x => x.Identifier == reviewCreditDTO.Identifier && x.Status != CreditStatus.PendingReview);

        Assert.NotNull(dbEntry);
        Assert.Equal(CreditStatus.Approved, dbEntry.Status);
        Assert.Equal(user.Name, dbEntry.Reviewer.Name);
    }

    [Fact]
    public async Task Test_Review_Credit_Should_Succeed_Reject()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        CreditModel credit = GetCredit();

        dbContext.Credits.Add(credit);
        dbContext.SaveChanges();

        User user = new()
        {
            Name = "User",
            Id = 1
        };

        dbContext.Users.Add(user);
        dbContext.SaveChanges();

        _userService.Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(user));

        ReviewCreditDTO reviewCreditDTO = new()
        {
            Identifier = credit.Identifier,
            Decision = "reject",
            ReviewerName = user.Name,
            ReviewTime = DateTime.Now
        };

        await creditService.ReviewCreditAsync(reviewCreditDTO, CancellationToken.None);
        CreditModel? dbEntry = dbContext.Credits.Include(x => x.Reviewer)
            .FirstOrDefault(x => x.Identifier == reviewCreditDTO.Identifier && x.Status != CreditStatus.PendingReview);

        Assert.NotNull(dbEntry);
        Assert.Equal(CreditStatus.Rejected, dbEntry.Status);
        Assert.Equal(user.Name, dbEntry.Reviewer.Name);
    }

    [Fact]
    public async Task Test_Submit_Credit_Should_Succeed_Mortgage()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        SubmitCreditDTO dto = new()
        {
            CreditAmount = 300,
            Email = "test@gmail.com",
            FullName = "Test User",
            MonthlyIncome = 200,
            Type = "mortgage"
        };

        await creditService.SubmitCreditAsync(dto, CancellationToken.None);

        Assert.Single(dbContext.Credits);
        CreditModel first = dbContext.Credits.First();

        Assert.Equal("test@gmail.com", first.Email);
        Assert.Equal("Test User", first.Name);
        Assert.Equal(300, first.CreditAmount);
        Assert.Equal(200, first.MonthlyIncome);
        Assert.Equal(CreditType.Mortgage, first.CreditType);
    }

    [Fact]
    public async Task Test_Submit_Credit_Should_Succeed_Auto()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        SubmitCreditDTO dto = new()
        {
            CreditAmount = 300,
            Email = "test@gmail.com",
            FullName = "Test User",
            MonthlyIncome = 200,
            Type = "auto"
        };

        await creditService.SubmitCreditAsync(dto, CancellationToken.None);

        Assert.Single(dbContext.Credits);
        CreditModel first = dbContext.Credits.First();

        Assert.Equal("test@gmail.com", first.Email);
        Assert.Equal("Test User", first.Name);
        Assert.Equal(300, first.CreditAmount);
        Assert.Equal(200, first.MonthlyIncome);
        Assert.Equal(CreditType.Auto, first.CreditType);
    }

    [Fact]
    public async Task Test_Submit_Credit_Should_Succeed_Personal()
    {
        CreditApprovalDbContext dbContext = GetDbContext(Guid.NewGuid().ToString());
        CreditService creditService = GetCreditService(dbContext);

        SubmitCreditDTO dto = new()
        {
            CreditAmount = 300,
            Email = "test@gmail.com",
            FullName = "Test User",
            MonthlyIncome = 200,
            Type = "personal"
        };

        await creditService.SubmitCreditAsync(dto, CancellationToken.None);

        Assert.Single(dbContext.Credits);
        CreditModel first = dbContext.Credits.First();

        Assert.Equal("test@gmail.com", first.Email);
        Assert.Equal("Test User", first.Name);
        Assert.Equal(300, first.CreditAmount);
        Assert.Equal(200, first.MonthlyIncome);
        Assert.Equal(CreditType.Personal, first.CreditType);
    }
}
