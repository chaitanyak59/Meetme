using System;
using System.Security.Cryptography;
using API.Data;
using API.DTO;
using API.Entities;
using API.Repository;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IUserRepository userRepository;

    public ITokenService tokenService { get; }

    private readonly UserManager<AppUser> _userManager;

    public AccountController(IUserRepository userRepository, ITokenService tokenService, UserManager<AppUser> userManager)
    {
        this.userRepository = userRepository;
        this.tokenService = tokenService;
        this._userManager = userManager;
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


        var user = new AppUser
        {
            UserName = username,
            Gender = "Male",
            KnownAs = username,
        };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => new
            {
                Error = e.Description
            }));
        }
        return Ok(new { Message = "User registered successfully", UserName = username, Token = tokenService.CreateToken(user), Thumbnail = user.Photos.FirstOrDefault(p => p.IsMain)?.Url });
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

        var passwordVerified = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordVerified)
        {
            return Unauthorized("Invalid username or password");
        }
        user.LastActive = DateTime.UtcNow;
        await this.userRepository.UpdateUserAsync(user);
        return Ok(new { Message = "Login successful", Token = tokenService.CreateToken(user), user.UserName, Thumbnail = user.Photos.FirstOrDefault(p => p.IsMain)?.Url });
    }
}
