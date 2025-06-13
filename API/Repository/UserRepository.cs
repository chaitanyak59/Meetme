using System;
using API.Data;
using API.DTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class UserRepository : IUserRepository
{
    private readonly MeetMeDBContext _context;
    public UserRepository(MeetMeDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task AddUserAsync(AppUser user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await _context.Users.Include(u => u.Photos).AsNoTracking().ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task<bool> RegisterUserAsync(AppUser user)
    {
        _context.Users.Add(user);
        return _context.SaveChangesAsync().ContinueWith(task => task.Result > 0);
    }

    public Task UpdateUserAsync(AppUser user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AsNoTracking().AnyAsync(u => u.UserName.ToLower() == username.ToLower());
    }

    public async Task<AppUser?>GetUserByNameAsync(string userName)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
    }
}
