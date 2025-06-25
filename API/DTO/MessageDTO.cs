using System;

namespace API.DTO;

public class MessageDTO
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public string RecipientName { get; set; }
    public string SenderPhotoUrl { get; set; }
    public string RecipientPhotoUrl { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSentAt { get; set; }
}
