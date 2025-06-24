using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public string CreateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public string CreateToken(AppUser user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        var tokenKey = _configuration["MeetMe:TokenKey"]!;
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new Exception("Cannot create Token , Invalid User Account");    
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };
        var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
