using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using reenbitChat.Domain.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace reenbitChat.BLL.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JwtSettings");
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
        var secret = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
           issuer: _jwtSettings["validIssuer"],
           audience: _jwtSettings["validAudience"],
           claims: claims,
           expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
           signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
