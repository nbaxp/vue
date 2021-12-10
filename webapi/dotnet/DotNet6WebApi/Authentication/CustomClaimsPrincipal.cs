using System.Security.Claims;

public class CustomClaimsPrincipal : ClaimsPrincipal
{
    private readonly IRoleService _roleService;

    public CustomClaimsPrincipal(IRoleService roleService, ClaimsPrincipal claimsPrincipal) : base(claimsPrincipal)
    {
        this._roleService = roleService;
    }
    public override bool IsInRole(string role)
    {
        return this._roleService.IsInRole(role);
    }
}
