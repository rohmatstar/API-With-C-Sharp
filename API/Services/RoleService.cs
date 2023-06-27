﻿using API.Contracts;
using API.DTOs.Roles;
using API.Models;

namespace API.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IEnumerable<GetRoleDto>? GetRole()
    {
        var roles = _roleRepository.GetAll();
        if (roles is null)
        {
            return null; // No roles found
        }

        var toDto = roles.Select(role =>
                                            new GetRoleDto
                                            {
                                                Guid = role.Guid,
                                                Name = role.Name
                                            }).ToList();

        return toDto; // Roles found
    }

    /*public IEnumerable<GetRoleDto>? GetRole(string name)
    {
        var roles = _roleRepository.GetByName(name);
        if (!roles.Any())
        {
            return null; // No roles found
        }

        var toDto = roles.Select(role =>
                                            new GetRoleDto
                                            {
                                                Guid = role.Guid,
                                                Name = role.Name
                                            }).ToList();

        return toDto; // Roles found
    }*/

    public GetRoleDto? GetRole(Guid guid)
    {
        var role = _roleRepository.GetByGuid(guid);
        if (role is null)
        {
            return null; // Role not found
        }

        var toDto = new GetRoleDto
        {
            Guid = role.Guid,
            Name = role.Name
        };

        return toDto; // Roles found
    }

    public GetRoleDto? CreateRole(NewRoleDto newRoleDto)
    {
        var role = new Role
        {
            Name = newRoleDto.Name,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRole = _roleRepository.Create(role);
        if (createdRole is null)
        {
            return null; // Role not created
        }

        var toDto = new GetRoleDto
        {
            Guid = createdRole.Guid,
            Name = createdRole.Name
        };

        return toDto; // Role created
    }

    public int UpdateRole(UpdateRoleDto updateRoleDto)
    {
        var isExist = _roleRepository.IsExist(updateRoleDto.Guid);
        if (!isExist)
        {
            return -1; // Role not found
        }

        var getRole = _roleRepository.GetByGuid(updateRoleDto.Guid);

        var role = new Role
        {
            Guid = updateRoleDto.Guid,
            Name = updateRoleDto.Name,
            ModifiedDate = DateTime.Now,
            CreatedDate = getRole!.CreatedDate
        };

        var isUpdate = _roleRepository.Update(role);
        if (!isUpdate)
        {
            return 0; // Role not updated
        }

        return 1;
    }

    public int DeleteRole(Guid guid)
    {
        var isExist = _roleRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Role not found
        }

        var role = _roleRepository.GetByGuid(guid);
        var isDelete = _roleRepository.Delete(role!);
        if (!isDelete)
        {
            return 0; // Role not deleted
        }

        return 1;
    }
}