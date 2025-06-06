using System;
using API.Entities;

namespace API.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
    string CreateRefreshToken();
    bool ValidateRefreshToken(string refreshToken);

}
