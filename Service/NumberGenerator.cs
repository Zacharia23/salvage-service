using System.Globalization;
using System.Security.Cryptography;
using SalvageCore.Data;

namespace SalvageCore.Service;

public class NumberGenerator
{
    private const string OfferPrefix = "SLV";
    private const string BidPrefix = "SBD";
    private const string UserPrefix = "SM";
    private const string CustomerPrefix = "CSM";
    private const string CompanyPrefix = "ISM";
    private const string VehiclePrefix = "SVH";
    private static readonly Random _rand = new();

    private readonly ApplicationDbContext _context;
    private string _defaultValue;

    public NumberGenerator(ApplicationDbContext context, string defaultValue = "0000")
    {
        _context = context;
        _defaultValue = defaultValue;
    }

    public string GenerateReference()
    {
        var chars = RandomString(4);
        var today = DateTime.Now;

        var number = string.Concat(OfferPrefix, $"{today:HHmmss}", "-", chars);

        return number;
    }

    public static string GenerateUserNumber()
    {
        var chars = RandomString(4);
        var today = DateTime.Now;

        var number = string.Concat(UserPrefix, $"{today:HHmmss}", "-", chars);

        return number;
    }

    public static string GenerateCompanyNumber()
    {
        var chars = RandomString(4);
        var today = DateTime.Now;

        var number = string.Concat($"{today:HHmmss}", CompanyPrefix, "-", chars);

        return number;
    }

    public static string GenerateCustomerNumber()
    {
        var chars = RandomString(4);
        var today = DateTime.Now;

        var number = string.Concat(CustomerPrefix, $"{today:HHmmss}", "-", chars);

        return number;
    }

    public static string GenerateVehicleReference()
    {
        var chars = RandomString(4);
        var today = DateTime.Now;

        var number = string.Concat(VehiclePrefix, $"{today:HHmmss}", "-", chars);

        return number;
    }

    public string GenerateBidReference()
    {
        var chars = RandomString(4);
        var today = DateTime.Now;

        var number = string.Concat(BidPrefix, $"{today:HHmmss}", "-", chars);

        return number;
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var randomArray = new char[length];

        for (var i = 0; i < length; i++) randomArray[i] = chars[_rand.Next(chars.Length)];

        return new string(randomArray);
    }

    public static string GenerateCode(int length)
    {
        const string digits = "0123456789";
        return GenerateSecureString(digits, length);
    }

    private static string GenerateSecureString(string allowedChars, int length)
    {
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);

        var result = new char[length];
        for (var i = 0; i < length; i++) result[i] = allowedChars[bytes[i] % allowedChars.Length];

        return new string(result);
    }

    public static string FormatCurrency(decimal amount)
    {
        var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        culture.NumberFormat.CurrencySymbol = "TZS";
        culture.NumberFormat.CurrencyDecimalDigits = 2;

        return string.Format(culture, "{0:C}", amount);
    }
}