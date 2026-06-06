using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, Guid? customerId = null);
    int AccessTokenLifetimeSeconds { get; }
}
