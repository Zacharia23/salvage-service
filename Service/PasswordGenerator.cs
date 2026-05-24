using System.Security.Cryptography;
using System.Text;

namespace SalvageCore.Service;

public class PasswordGenerator
{
    private const int DefaultLength = 12;

    public Task<string> Generate(int length = DefaultLength, bool includeUppercase = true, bool includeNumbers = true,
        bool includeSpecialChars = true)
    {
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numberChars = "0123456789";
        const string specialChars = "@$%&";

        var validChars = new StringBuilder(lowercaseChars);

        if (includeUppercase) validChars.Append(uppercaseChars);

        if (includeNumbers) validChars.Append(numberChars);

        if (includeSpecialChars) validChars.Append(specialChars);

        using (var rng = RandomNumberGenerator.Create())
        {
            var randomBytes = new byte[length];
            rng.GetBytes(randomBytes);

            var password = new StringBuilder(length);

            foreach (var b in randomBytes) password.Append(validChars[b % validChars.Length]);

            return Task.FromResult(password.ToString());
        }
    }
}