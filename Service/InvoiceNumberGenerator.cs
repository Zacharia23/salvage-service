namespace SalvageCore.Service;

public class InvoiceNumberGenerator
{
    private static readonly object _lock = new();

    public static string GenerateTimeBasedInvoice(string prefix = "INV")
    {
        lock (_lock)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var sequence = GetNextSequence().ToString("D3");
            return $"{prefix}-{timestamp}-{sequence}";
        }
    }

    private static int GetNextSequence()
    {
        return (int)(DateTime.Now.Ticks % 1000);
    }
}