using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.DTO;

public class SeedEntityDTO
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MeetMeDBContext>();
        // Launch Pending Migrations
        await dbContext.Database.MigrateAsync();

        if (dbContext.Users.Any())
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

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            foreach (var photo in user.Photos)
            {
                photo.DateAdded = DateTime.UtcNow;
            }

            dbContext.Users.Add(user);
        }
        Console.WriteLine("Seeding data Successful...");
        await dbContext.SaveChangesAsync();
    }

}
