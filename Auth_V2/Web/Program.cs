using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Auth;
using Web.Endpoints;
using Web.Persistance;
using Web.Services;
using Web;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddOpenApi();

services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("UserDbContext"));
});

services.AddScoped<JwtProvider>();
services.AddScoped<PasswordHasher>();
services.AddScoped<UserRepository>();
services.AddScoped<UserService>();

services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(builder.Configuration.GetSection(nameof(AuthorizationOptions)));

services.AddApiAuthentication(builder.Configuration);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "User API"));
}

app.UseHttpsRedirection();
app.MapUsersEndpoints();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();
