namespace CreditApproval.Tests.Helpers;

public class CreditIdentifierGeneratorTests
{
    private readonly CreditIdentifierGenerator creditIdentifierGenerator = new();

    [Fact]
    public void Should_Create_Correct_Id()
    {
        string id = creditIdentifierGenerator.GenerateIdentifier();

        DateTime utc = DateTime.UtcNow;
        DateTime date = new(utc.Year, utc.Month, utc.Day);

        Assert.Equal($"CRE-{date:yyyMMdd}-0001", id);
    }

    [Fact]
    public void Should_Create_Consecutive_Ids()
    {
        int length = 10;

        DateTime utc = DateTime.UtcNow;
        DateTime date = new(utc.Year, utc.Month, utc.Day);

        for (int i = 1; i <= length; i++)
        {
            string id = creditIdentifierGenerator.GenerateIdentifier();
            Assert.Equal($"CRE-{date:yyyMMdd}-{i:D4}", id);
        }
    }
}
