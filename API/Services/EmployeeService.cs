using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Repositories;
using API.Utilities.Enums;
using System.Net;
using System.Xml.Linq;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
    }

    public IEnumerable<DetailEmployeeDto>? GetMaster()
    {
        var master = (from e in _employeeRepository.GetAll()
                      join education in _educationRepository.GetAll() on e.Guid equals education.Guid
                      join u in _universityRepository.GetAll() on education.UniversityGuid equals u.Guid
                      select new DetailEmployeeDto
                      {
                          Guid = e.Guid,
                          FullName = e.FirstName + " " + e.LastName,
                          NIK = e.Nik,
                          BirthDate = e.BirthDate,
                          Email = e.Email,
                          Gender = e.Gender,
                          HiringDate = e.HiringDate,
                          PhoneNumber = e.PhoneNumber,
                          Major = education.Major,
                          Degree = education.Degree,
                          GPA = education.Gpa,
                          UniversityName = u.Name
                      });

        if (!master.Any())
        {
            return null;
        }

        return master!;
    }

    public DetailEmployeeDto? GetMasterByGuid(Guid guid)
    {
        var master = GetMaster();
        var singleMaster = master.FirstOrDefault(m => m.Guid == guid);
        return singleMaster;
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
    public OtpResponseDto? GetByEmail(string email)
    {
        var account = _employeeRepository.GetAll()
            .FirstOrDefault(e => e.Email.Contains(email));

        if (account != null)
        {
            return new OtpResponseDto
            {
                Email = account.Email,
                Guid = account.Guid
            };
        }

        return null;
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
        // Generate NIK
        var Nik = _employeeRepository.GetAll().LastOrDefault().Nik;
        int InitNik = 11111;
        if (Nik != null)
        {
            InitNik = Convert.ToInt32(Nik) + 1;
        }

        var employee = new Employee
        {
            Nik = InitNik.ToString(),
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