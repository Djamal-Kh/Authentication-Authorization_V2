using Microsoft.AspNetCore.Authorization;
using Web.Domain;

namespace Web.Auth;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(EnumPermission[] permissions)
    {
        Permissions = permissions;       
    }
    
    public EnumPermission[] Permissions { get; set; } = [];
}