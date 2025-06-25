using System;

namespace API.Entities;

public class Message
{
    public int Id { get; set; }
    public required string SenderName { get; set; }
    public required string RecipientName { get; set; }
    public required string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSentAt { get; set; } = DateTime.UtcNow;
    public bool SenderDeleted { get; set; }
    public bool ReceipientDeleted { get; set; }

    // Navigation
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
    public virtual AppUser Sender { get; set; } = null!;
    public virtual AppUser Recipient { get; set; } = null!;
}
