using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.DTO;

public class SeedEntityDTO
{
    public static async Task SeedDataAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {

        if (await userManager.Users.AnyAsync())
        {
            Console.WriteLine("Skipped seeding. Data already exists.");
            return;
        }

        var userData = string.Empty;
        if (File.Exists("Data/SeedData.json"))
        {
            userData = await File.ReadAllTextAsync("Data/SeedData.json");
        }
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null)
        {
            Console.WriteLine("Skipped seeding");
            return;
        }

        // Add default roles
        var roles = new List<AppRole>()
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            if (string.IsNullOrWhiteSpace(user.UserName)) continue;

            foreach (var photo in user.Photos ?? Enumerable.Empty<Photo>())
            {
                photo.DateAdded = DateTime.UtcNow;
            }

            var result = await userManager.CreateAsync(user, "Pa$$w0rd");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Member");
                Console.WriteLine($"Seeded user: {user.UserName}");
            }
            else
            {
                Console.WriteLine($"Failed to create user: {user.UserName}");
            }
        }
        Console.WriteLine("Seeding data Successful...");
    }
}
