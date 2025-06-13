using System;
using System.Security.Cryptography;
using API.Data;
using API.DTO;
using API.Entities;
using API.Repository;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IUserRepository userRepository;

    public ITokenService tokenService { get; }

    public AccountController(IUserRepository userRepository, ITokenService tokenService)
    {
        this.userRepository = userRepository;
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
        var existingUser = await this.userRepository.UserExistsAsync(username);
        if (existingUser)
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
                PasswordSalt = passwordSalt,
                Gender = "Male",
            };

            if (await userRepository.RegisterUserAsync(user) == false)
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
        var user = await this.userRepository.GetUserByNameAsync(username);

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
