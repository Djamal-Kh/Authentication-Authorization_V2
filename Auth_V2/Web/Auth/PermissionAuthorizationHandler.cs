using Microsoft.AspNetCore.Authorization;
using Web.Persistance;

namespace Web.Auth;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId);

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var permissionRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();

        var permissions = await permissionRepository.GetUserPermissions(id);
        
        if(permissions.Intersect(requirement.Permissions).Any())
            context.Succeed(requirement);
    }
}