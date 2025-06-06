using System;
using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data;

public class MeetMeDBContext : DbContext
{
    private readonly IConfiguration _config;

    public MeetMeDBContext(DbContextOptions<MeetMeDBContext> options, IConfiguration config) 
        : base(options)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine(optionsBuilder.IsConfigured);
    }

    public DbSet<AppUser> Users { get; set; }
}
