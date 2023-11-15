using Forum.Models;
using System.Security.Claims;

namespace Forum.Tools
{
    public interface IJwtService
    {
        string GenerateToken(User user, IEnumerable<Claim> claims);
    }
}
