using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Web.Auth;
using Web.Domain;
using Web.Persistance.Configurations;
using Web.Persistance.Entities;

namespace Web.Persistance;

public class UserDbContext(
    DbContextOptions<UserDbContext> options,
    IOptions<AuthorizationOptions> authOptions) : DbContext(options)
{
    
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<RoleEntity> Roles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
    }
}