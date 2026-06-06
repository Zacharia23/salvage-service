using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SalvageCore.Interface;
using SalvageCore.Models;

namespace SalvageCore.Service;

public class TokenService : ITokenService
{
    private const int AccessTokenMinutes = 15;
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _securityKey;

    public int AccessTokenLifetimeSeconds => (int)TimeSpan.FromMinutes(AccessTokenMinutes).TotalSeconds;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        var signingKey = _configuration["JWT:SigninKey"];
        if (string.IsNullOrWhiteSpace(signingKey) || Encoding.UTF8.GetByteCount(signingKey) < 32)
            throw new InvalidOperationException("JWT signing key must contain at least 32 bytes.");

        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    }

    public string CreateToken(ApplicationUser user, Guid? customerId = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.GivenName, user.SystemUser?.Username ?? user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
            new("system_user_id", user.SystemUserId.ToString())
        };

        var role = NormalizeRole(user.SystemUser?.Role);
        if (!string.IsNullOrWhiteSpace(role))
            claims.Add(new Claim("role", role));

        if (customerId.HasValue)
            claims.Add(new Claim("customer_id", customerId.Value.ToString()));

        var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(AccessTokenMinutes),
            SigningCredentials = creds,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private static string? NormalizeRole(string? role)
    {
        if (role?.Equals("Administrator", StringComparison.OrdinalIgnoreCase) == true)
            return "Administrator";
        if (role?.Equals("Manager", StringComparison.OrdinalIgnoreCase) == true)
            return "Manager";
        if (role?.Equals("Customer", StringComparison.OrdinalIgnoreCase) == true)
            return "Customer";

        return role;
    }
}
