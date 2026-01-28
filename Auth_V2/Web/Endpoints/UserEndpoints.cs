using Web.Dto;
using Web.Services;

namespace Web.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", Register);

        app.MapPost("login", Login);

        app.MapGet("email", GetByEmail)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> Register(
        UserService userService,
        RegisterUserRequest request)
    {
        await userService.Register(request.Username, request.Email, request.Password);

        return TypedResults.Ok();
    }

    private static async Task<IResult> Login(
        UserService userService,
        LoginUserRequest request,
        HttpContext context)
    {
        var token = await userService.Login(request.Email, request.Password);
        
        context.Response.Cookies.Append("cookies", token);
        
        return Results.Ok(token);
    }

    private static async Task<IResult> GetByEmail(
        string email,
        UserService userService)
    {
        var user = await userService.GetByEmail(email);
        
        return TypedResults.Ok(user);
    }
}