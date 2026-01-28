using Web.Auth;
using Web.Domain;
using Web.Persistance;

namespace Web.Services;

public class UserService
{
    private readonly PasswordHasher _passwordHasher;
    private readonly UserRepository _userRepository;
    private readonly JwtProvider _jwtProvider;

    public UserService(PasswordHasher passwordHasher, UserRepository userRepository, JwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        
        var user = User.Create(Guid.NewGuid() ,userName, hashedPassword, email);
        
        await _userRepository.Add(user);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);
        
        var result = _passwordHasher.Verify(password, user.PasswordHash);
        
        if (result is false)
            throw new Exception("Failed to login");
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
    
    public async Task<User> GetByEmail(string email) =>
        await _userRepository.GetByEmail(email);
}