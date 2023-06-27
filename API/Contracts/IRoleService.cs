using API.DTOs.Roles;

namespace API.Contracts;

public interface IRoleService
{
    IEnumerable<GetRoleDto> GetRole();
}