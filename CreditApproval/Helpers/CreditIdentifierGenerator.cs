namespace CreditApproval.Helpers;

public class CreditIdentifierGenerator
{
    private DateTime _lastDate = new(1, 1, 1);
    private long _lastNumber = 0;

    private const string _prefix = "CRE";
    private const string _dateFormat = "yyyyMMdd";
    private const char _delimiter = '-';
    private readonly StringBuilder sb = new();

    public string GenerateIdentifier()
    {
        sb.Clear();

        // Build the "CRE-" part
        sb.Append(_prefix);
        sb.Append(_delimiter);

        DateTime utcNow = DateTime.UtcNow;
        DateTime now = new(utcNow.Year, utcNow.Month, utcNow.Day);

        // Build the date part
        sb.Append(now.ToString(_dateFormat));
        sb.Append(_delimiter);

        // Build the sequence number (4 leading digits)
        if (now > _lastDate)
        {
            _lastDate = now;
            _lastNumber = 0;
        }

        _lastNumber++;

        sb.Append(_lastNumber.ToString("D4"));

        return sb.ToString();
    }
}
