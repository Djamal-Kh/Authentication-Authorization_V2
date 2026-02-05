using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web.Domain;

namespace Web.Auth;

// по хорошему нужен интерфейс
public class JwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(UserEntity user)
    {
        Claim[] claims = [new("userId", user.Id.ToString())];
        
        var signingCredentials = new SigningCredentials(
            // здесь располагается секретный ключ, который по идее должен храниться в защищенном месте
            // т.к. проект учебный, он будет сохранен в файле appsettings.json
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpitesHours),
            claims: claims
        );
        
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        
        return tokenValue;
    }
}