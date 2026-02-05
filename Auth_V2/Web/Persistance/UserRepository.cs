using Microsoft.EntityFrameworkCore;
using Web.Domain;

namespace Web.Persistance;

public class UserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext userDbContext)
    {
        _context = userDbContext;
    }

    public async Task Add(User user)
    {
        var roleEntity = await _context.Roles
                             .SingleOrDefaultAsync(r => r.Id == (int)EnumRole.Admin)
                         ?? throw new InvalidOperationException();

        var userEntity = new UserEntity()
        {
            Id = user.Id,
            UserName = user.Username,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            Roles = [roleEntity]
        };
        
        await _context.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity> GetByEmail(string email) =>
        await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new Exception();
    
    public async Task<HashSet<EnumPermission>> GetUserPermissions(Guid userId)
    {
        var roles = await _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (EnumPermission)p.Id)
            .ToHashSet();
    }
}