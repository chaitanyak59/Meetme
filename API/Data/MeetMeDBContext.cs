using System;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace API.Data;

public class MeetMeDBContext : IdentityDbContext<AppUser, AppRole, int,
IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    private readonly IConfiguration _config;

    public MeetMeDBContext(DbContextOptions<MeetMeDBContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("Database is ready : " + optionsBuilder.IsConfigured);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasMany(e => e.Photos)
            .WithOne(e => e.AppUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        modelBuilder.Entity<AppRole>()
            .HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(e => e.RoleId)
            .IsRequired();

        modelBuilder.Entity<Message>()
            .HasOne(e => e.Recipient)
            .WithMany(e => e.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

         modelBuilder.Entity<Message>()
            .HasOne(e => e.Sender)
            .WithMany(e => e.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Photo> Photos { get; set; }
    public DbSet<Message> Messages { get; set; }
}
