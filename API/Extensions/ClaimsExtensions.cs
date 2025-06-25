using System;
using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsExtensions
{
    /// <summary>Return ClaimTypes.NameIdentifier or null.</summary>
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user?.FindFirstValue(ClaimTypes.NameIdentifier);

    /// <summary>Return ClaimTypes.Name or null.</summary>
    public static string? GetUserName(this ClaimsPrincipal user) =>
        user?.FindFirstValue(ClaimTypes.Name);

    /// <summary>Return ClaimTypes.Email or null.</summary>
    public static string? GetEmail(this ClaimsPrincipal user) =>
        user?.FindFirstValue(ClaimTypes.Email);

    /// <summary>Return all role claims (ClaimTypes.Role) as read-only list.</summary>
    public static List<string> GetRoles(this ClaimsPrincipal user)
    {
        return user?.FindAll(ClaimTypes.Role)
             .Select(c => c.Value)
             .ToList() ?? [];
    }

    /// <summary>Generic helper – gets the first claim value for any type.</summary>
    public static string? FindFirstValue(this ClaimsPrincipal user, string claimType) =>
        user?.FindFirst(claimType)?.Value;
}
