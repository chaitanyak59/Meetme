using System;
using API.Data;
using API.DTO;
using API.Entities;
using API.Helpers.JsonModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class MessageRepository : IMessageRepository
{
    private readonly MeetMeDBContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(MeetMeDBContext dBContext, IMapper mapper)
    {
        _context = dBContext;
        _mapper = mapper;
    }

    public async Task AddMessage(Message message)
    {
        await _context.Messages.AddAsync(message);
    }

    public async Task DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public async Task<IEnumerable<MessageDTO>> GetMessageThread(int userID, string receipientUsername)
    {
        var messages = _context.Messages
                         .Where(m => (m.SenderId == userID && m.RecipientName.ToLower() == receipientUsername.ToLower())
                         || (m.SenderName.ToLower() == receipientUsername.ToLower() && userID == m.RecipientId))
                         .Include(m => m.Sender).ThenInclude(s => s.Photos)
                         .Include(m => m.Recipient).ThenInclude( r => r.Photos)
                         .OrderBy(m => m.MessageSentAt);

        
        var unreadMessages = messages
                             .Where(
                                m => m.DateRead == null
                                && m.RecipientId == userID).ToList(); // My unread thread

        if (unreadMessages.Count() > 0)
        {
            unreadMessages.ForEach((u) =>
            {
                u.DateRead = DateTime.UtcNow;
            });

            await _context.SaveChangesAsync();
        }

        return _mapper.Map<List<MessageDTO>>(messages);
    }

    public async Task<PagedList<MessageDTO>> GetUserMessages(MessageParams messageParams)
    {
        var query = _context.Messages.AsNoTracking().OrderByDescending(m => m.MessageSentAt);
        var filteredQuery = messageParams.Container switch
        {
            "Inbox" => query.Where(m => messageParams.UserID == m.RecipientId),
            "Outbox" => query.Where(m => messageParams.UserID == m.SenderId),
            _ => query.Where(m => messageParams.UserID == m.RecipientId)
        };

        var messagesDTOQuery = filteredQuery.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);
        return await PagedList<MessageDTO>.CreateAsync(messagesDTOQuery, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<bool> SaveAllAynsc()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
