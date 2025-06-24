using System;
using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public DateOnly DateOfBirth { get; set; }
    public string KnownAs { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public required string Gender { get; set; }
    public string? Introduction { get; set; } = string.Empty;
    public string? LookingFor { get; set; } = string.Empty;
    public string? Interests { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
    public ICollection<Message> MessagesSent { get; set; } = [];
    public ICollection<Message> MessagesReceived { get; set; } = [];
}
