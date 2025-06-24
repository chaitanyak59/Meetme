using System;

namespace API.Entities;

public class Message
{
    public int Id { get; set; }
    public string SenderName { get; set; }
    public string RecipientName { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSentAt { get; set; }
    public bool SenderDeleted { get; set; }
    public bool ReceipientDeleted { get; set; }

    // Navigation
    public int SenderId { get; set; }
    public int RecepientId { get; set; }
    public AppUser Sender { get; set; } = null!;
    public AppUser Recipient { get; set; } = null!;
}
