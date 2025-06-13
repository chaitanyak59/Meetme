using System;
using API.DTO;
using API.Entities;

namespace API.Repository;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?>GetUserByNameAsync(string userName);
    Task<bool> UserExistsAsync(string username);
    Task AddUserAsync(AppUser user);
    Task UpdateUserAsync(AppUser user);
    Task DeleteUserAsync(int id);
    Task<bool> RegisterUserAsync(AppUser user);
}
