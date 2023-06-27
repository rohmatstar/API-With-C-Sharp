using API.DTOs.AccountRoles;

namespace API.Contracts;

public interface IAccountRoleService
{
    IEnumerable<GetAccountRoleDto> GetAccountRole();
}