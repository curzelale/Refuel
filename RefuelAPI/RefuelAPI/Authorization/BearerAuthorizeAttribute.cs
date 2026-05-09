using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace RefuelAPI.Authorization;

public class BearerAuthorizeAttribute : AuthorizeAttribute
{
    public BearerAuthorizeAttribute()
    {
        AuthenticationSchemes = IdentityConstants.BearerScheme;
    }

    public BearerAuthorizeAttribute(string role)
    {
        AuthenticationSchemes = IdentityConstants.BearerScheme;
        Roles = role;
    }
}
