using System;
using API.DTO;
using API.Entities;
using API.Helpers.JsonModels;

namespace API.Repository;

public interface IMessageRepository
{
    Task AddMessage(Message message);
    Task DeleteMessage(Message message);
    Task<Message?> GetMessage(int id);
    Task<PagedList<MessageDTO>> GetUserMessages(MessageParams messageParams);
    Task<IEnumerable<MessageDTO>> GetMessageThread(int userID, string receipientUsername);
    Task<bool> SaveAllAynsc();
}
