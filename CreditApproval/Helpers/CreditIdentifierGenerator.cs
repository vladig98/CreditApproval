namespace CreditApproval.Helpers;

public class CreditIdentifierGenerator
{
    private const string _prefix = "CRE";
    private const string _dateFormat = "yyyyMMdd";
    private const char _delimiter = '-';
    private readonly StringBuilder sb = new();

    public string GenerateIdentifier(long id)
    {
        sb.Clear();

        // Build the "CRE-" part
        sb.Append(_prefix);
        sb.Append(_delimiter);

        // Build the date part
        sb.Append(DateTime.UtcNow.ToString(_dateFormat));
        sb.Append(_delimiter);

        // Build the sequence number (4 leading digits)
        sb.Append(id.ToString("D4"));

        return sb.ToString();
    }
}
