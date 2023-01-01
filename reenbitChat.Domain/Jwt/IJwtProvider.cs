using System.Security.Claims;
namespace reenbitChat.Domain.Jwt;
    
public interface IJwtProvider
{
    string GenerateToken(IEnumerable<Claim> claims);
}
