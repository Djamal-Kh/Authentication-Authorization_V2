using Microsoft.EntityFrameworkCore;
using Web.Domain;

namespace Web.Persistance;

public class UserRepository
{
    private readonly UserDbContext _userDbContext;

    public UserRepository(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public async Task Add(User user)
    {
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();
    }

    public async Task<User> GetByEmail(string email) =>
        await _userDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new Exception();
}