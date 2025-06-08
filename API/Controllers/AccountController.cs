using System;
using System.Security.Cryptography;
using API.Data;
using API.DTO;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly MeetMeDBContext context;

    public ITokenService tokenService { get; }

    public AccountController(MeetMeDBContext context, ITokenService tokenService)
    {
        this.context = context;
        this.tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var username = registerUserDTO.UserName;
        var password = registerUserDTO.Password;

        // Check if the user already exists
        var existingUser = await this.context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (existingUser != null)
        {
            return BadRequest("Username already exists");
        }

        // Hash the password using HMACSHA512
        {
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var passwordSalt = hmac.Key;
            var user = new AppUser
            {
                UserName = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            this.context.Users.Add(user);
            var result = await this.context.SaveChangesAsync();
            if (result <= 0)
            {
                return BadRequest("Failed to register user");
            }
            return Ok(new { Message = "User registered successfully", UserName = username, Token = tokenService.CreateToken(user) });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] RegisterUserDTO loginUserDTO)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var username = loginUserDTO.UserName.ToLower();
        var password = loginUserDTO.Password;

        // Find the user by username
        var user = await this.context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName.ToLower() == username);

        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        // Verify the password
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(new { Message = "Login successful", Token = tokenService.CreateToken(user), user.UserName });
    }
}
