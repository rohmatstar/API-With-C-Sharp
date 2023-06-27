using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Enums;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public IEnumerable<GetEmployeeDto>? GetEmployee()
    {
        var employees = _employeeRepository.GetAll();
        if (!employees.Any())
        {
            return null; // No employees found
        }

        var toDto = employees.Select(employee =>
                                            new GetEmployeeDto
                                            {
                                                Guid = employee.Guid,
                                                Nik = employee.Nik,
                                                FirstName = employee.FirstName,
                                                LastName = employee.LastName,
                                                BirthDate = employee.BirthDate,
                                                Gender = employee.Gender,
                                                HiringDate = employee.HiringDate,
                                                Email = employee.Email,
                                                PhoneNumber = employee.PhoneNumber
                                            }).ToList();

        return toDto; // Employees found
    }

    public IEnumerable<GetEmployeeDto>? GetEmployee(string name)
    {
        var employees = _employeeRepository.GetByName(name);
        if (!employees.Any())
        {
            return null; // No employees found
        }

        var toDto = employees.Select(employee =>
                                            new GetEmployeeDto
                                            {
                                                Guid = employee.Guid,
                                                Nik = employee.Nik,
                                                FirstName = employee.FirstName,
                                                LastName = employee.LastName,
                                                BirthDate = employee.BirthDate,
                                                Gender = employee.Gender,
                                                HiringDate = employee.HiringDate,
                                                Email = employee.Email,
                                                PhoneNumber = employee.PhoneNumber
                                            }).ToList();

        return toDto; // Employees found
    }

    public GetEmployeeDto? GetEmployee(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return null; // Employee not found
        }

        var toDto = new GetEmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        };

        return toDto; // Employees found
    }

    public GetEmployeeDto? CreateEmployee(NewEmployeeDto newEmployeeDto)
    {
        var employee = new Employee
        {
            Nik = newEmployeeDto.Nik,
            FirstName = newEmployeeDto.FirstName,
            LastName = newEmployeeDto.LastName,
            BirthDate = newEmployeeDto.BirthDate,
            Gender = newEmployeeDto.Gender,
            HiringDate = newEmployeeDto.HiringDate,
            Email = newEmployeeDto.Email,
            PhoneNumber = newEmployeeDto.PhoneNumber,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null; // Employee not created
        }

        var toDto = new GetEmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        };

        return toDto; // Employee created
    }

    public int UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
    {
        var isExist = _employeeRepository.IsExist(updateEmployeeDto.Guid);
        if (!isExist)
        {
            return -1; // Employee not found
        }

        var getEmployee = _employeeRepository.GetByGuid(updateEmployeeDto.Guid);

        var employee = new Employee
        {
            Guid = updateEmployeeDto.Guid,
            Nik = updateEmployeeDto.Nik,
            FirstName = updateEmployeeDto.FirstName,
            LastName = updateEmployeeDto.LastName,
            BirthDate = updateEmployeeDto.BirthDate,
            Gender = updateEmployeeDto.Gender,
            HiringDate = updateEmployeeDto.HiringDate,
            Email = updateEmployeeDto.Email,
            PhoneNumber = updateEmployeeDto.PhoneNumber,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEmployee!.CreatedDate
        };

        var isUpdate = _employeeRepository.Update(employee);
        if (!isUpdate)
        {
            return 0; // Employee not updated
        }

        return 1;
    }

    public int DeleteEmployee(Guid guid)
    {
        var isExist = _employeeRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Employee not found
        }

        var employee = _employeeRepository.GetByGuid(guid);
        var isDelete = _employeeRepository.Delete(employee!);
        if (!isDelete)
        {
            return 0; // Employee not deleted
        }

        return 1;
    }
}