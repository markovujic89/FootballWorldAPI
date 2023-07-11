using Microsoft.AspNetCore.Identity;

namespace Application
{
    public interface ITokenService
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
