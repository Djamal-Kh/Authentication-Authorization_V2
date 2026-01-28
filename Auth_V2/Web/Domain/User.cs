namespace Web.Domain;

public class User
{
    private User(Guid id, string username, string passwordHash, string email)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
    }
    
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash {get; set;}
    public string Email { get; set; }

    public static User Create(Guid id, string username, string passwordHash, string email)
    {
        return new User(id, username, passwordHash, email);
    }
}