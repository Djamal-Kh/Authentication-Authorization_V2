using Microsoft.EntityFrameworkCore;
using Web.Domain;

namespace Web.Persistance;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users");
            
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .HasColumnName("id");
        
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasColumnName("email");
        
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasColumnName("Usersname");
        
        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .HasColumnName("password_hash");
    }
}