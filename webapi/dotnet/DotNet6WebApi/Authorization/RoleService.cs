﻿public class RoleService : IRoleService
{
    private List<string> RoleRepository = new List<string> {"admin1","user" };

    private string[]? _roles = null;

    public string[] GetRoles()
    {
        if (this._roles == null)
        {
            this._roles = RoleRepository.ToArray();
        }
        return this._roles;
    }

    public bool IsInRole(string roleName)
    {
        return RoleRepository.Any(o => o==roleName);
    }
}
