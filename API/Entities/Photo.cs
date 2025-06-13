using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string? PublicId { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public bool IsMain { get; set; } = false;
    public virtual AppUser AppUser { get; set; } = null!;
    public int AppUserId { get; set; }
}
