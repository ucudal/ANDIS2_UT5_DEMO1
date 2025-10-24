using Monolith.Application.Users.Models;
using Monolith.Domain.Users;
using Monolith.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Monolith.Application.Users;

public class UsersService : IUsersService
{
    private readonly AppDbContext _db;
    public UsersService(AppDbContext db) => _db = db;

    public async Task<List<UserDto>> GetAllAsync() =>
        await _db.Users.AsNoTracking()
            .Select(u => new UserDto(u.Id, u.Email, u.Name))
            .ToListAsync();

    public async Task<UserDto?> GetByIdAsync(Guid id) =>
        await _db.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDto(u.Id, u.Email, u.Name))
            .FirstOrDefaultAsync();

    public async Task<UserDto> CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.Trim(),
            Name = request.Name.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return new UserDto(user.Id, user.Email, user.Name);
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return null;
        user.Name = request.Name.Trim();
        await _db.SaveChangesAsync();
        return new UserDto(user.Id, user.Email, user.Name);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return false;
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}
