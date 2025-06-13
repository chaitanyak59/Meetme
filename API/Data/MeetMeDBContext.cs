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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasMany(e => e.Photos)
            .WithOne(e => e.AppUser)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<Photo> Photos { get; set; }
}
