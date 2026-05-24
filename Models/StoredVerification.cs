namespace SalvageCore.Models;

public class StoredVerification
{
    public string Code { get; set; }
    public string Phone { get; set; }
    public TimeSpan Expiration { get; set; }
}