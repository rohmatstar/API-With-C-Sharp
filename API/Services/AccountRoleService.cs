using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;

namespace API.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleService(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    public IEnumerable<GetAccountRoleDto>? GetAccountRole()
    {
        var accountRoles = _accountRoleRepository.GetAll();
        if (!accountRoles.Any())
        {
            return null; // No accountRoles found
        }

        var toDto = accountRoles.Select(accountRole =>
                                            new GetAccountRoleDto
                                            {
                                                Guid = accountRole.Guid,
                                                AccountGuid = accountRole.AccountGuid,
                                                RoleGuid = accountRole.RoleGuid
                                            }).ToList();

        return toDto; // AccountRoles found
    }

    public GetAccountRoleDto? GetAccountRole(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return null; // AccountRole not found
        }

        var toDto = new GetAccountRoleDto
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        };

        return toDto; // AccountRoles found
    }

    public GetAccountRoleDto? CreateAccountRole(NewAccountRoleDto newAccountRoleDto)
    {
        var accountRole = new AccountRole
        {
            AccountGuid = newAccountRoleDto.AccountGuid,
            RoleGuid = newAccountRoleDto.RoleGuid,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccountRole = _accountRoleRepository.Create(accountRole);
        if (createdAccountRole is null)
        {
            return null; // AccountRole not created
        }

        var toDto = new GetAccountRoleDto
        {
            Guid = createdAccountRole.Guid,
            AccountGuid = createdAccountRole.AccountGuid,
            RoleGuid = createdAccountRole.RoleGuid
        };

        return toDto; // AccountRole created
    }

    public int UpdateAccountRole(UpdateAccountRoleDto updateAccountRoleDto)
    {
        var isExist = _accountRoleRepository.IsExist(updateAccountRoleDto.Guid);
        if (!isExist)
        {
            return -1; // AccountRole not found
        }

        var getAccountRole = _accountRoleRepository.GetByGuid(updateAccountRoleDto.Guid);

        var accountRole = new AccountRole
        {
            Guid = updateAccountRoleDto.Guid,
            AccountGuid = updateAccountRoleDto.AccountGuid,
            RoleGuid = updateAccountRoleDto.RoleGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccountRole!.CreatedDate
        };

        var isUpdate = _accountRoleRepository.Update(accountRole);
        if (!isUpdate)
        {
            return 0; // AccountRole not updated
        }

        return 1;
    }

    public int DeleteAccountRole(Guid guid)
    {
        var isExist = _accountRoleRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // AccountRole not found
        }

        var accountRole = _accountRoleRepository.GetByGuid(guid);
        var isDelete = _accountRoleRepository.Delete(accountRole!);
        if (!isDelete)
        {
            return 0; // AccountRole not deleted
        }

        return 1;
    }
}