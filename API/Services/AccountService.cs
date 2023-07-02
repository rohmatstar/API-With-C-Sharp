using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Universities;
using API.Models;
using API.Repositories;
using API.Utilities;
using API.Utilities.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Configuration;
using API.Controllers;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IRoleRepository _roleRepository;

    private readonly IConfiguration _configuration;
    private readonly ITokenHandler _tokenHandler;

    public AccountService(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository, IConfiguration configuration, ITokenHandler tokenHandler)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _accountRoleRepository = accountRoleRepository;
        _roleRepository = roleRepository;

        _configuration = configuration;
        _tokenHandler = tokenHandler;
    }

    public int GenerateOtp()
    {
        Random random = new Random();
        HashSet<int> uniqueDigits = new HashSet<int>();

        while (uniqueDigits.Count < 6)
        {
            int digit = random.Next(0, 9);
            uniqueDigits.Add(digit);
        }

        int generatedOtp = uniqueDigits.Aggregate(0, (acc, digit) => acc * 10 + digit);

        return generatedOtp;
    }

    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var isExist = _employeeRepository.GetAll().FirstOrDefault(e => e.Email == changePasswordDto.Email);
        if (isExist is null)
        {
            return -1; // not found
        }

        var getAccount = _accountRepository.GetByGuid(isExist.Guid);
        if (getAccount.Otp != changePasswordDto.Otp)
        {
            return 0;
        }

        if (getAccount.IsUsed == true)
        {
            return 1;
        }

        if (getAccount.ExpiredTime < DateTime.Now)
        {
            return 2;
        }

        var account = new Account
        {
            Guid = getAccount.Guid,
            IsUsed = getAccount.IsUsed,
            IsDeleted = getAccount.IsDeleted,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate,
            Otp = getAccount.Otp,
            ExpiredTime = getAccount.ExpiredTime,
            Password = Hashing.HashPassword(changePasswordDto.NewPassword),
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0; // Account not updated
        }

        return 3;
    }

    public string Login(LoginDto loginDto)
    {
        var employees = _employeeRepository.GetAll();
        var employee = employees.FirstOrDefault(e => e.Email == loginDto.Email);

        if (employee != null)
        {
            var employeeGuid = employee.Guid;

            var getAccount = GetAccount().FirstOrDefault(account =>
                account != null && account.Guid == employeeGuid);

            if (getAccount != null)
            {
                var accountRoles = _accountRoleRepository.GetAll().Where(ar => ar.AccountGuid == getAccount.Guid).ToList();
                var roleNames = new List<string>();

                foreach (var accountRole in accountRoles)
                {
                    var role = _roleRepository.GetByGuid(accountRole.RoleGuid);
                    roleNames.Add(role.Name);
                }

                var roles = string.Join(", ", roleNames);

                if (Hashing.ValidatePassword(loginDto.Password, getAccount.Password))
                {
                    var claims = new List<Claim>() {
                        new Claim("NIK", employee.Nik),
                        new Claim("FullName", $"{employee.FirstName} {employee.LastName}"),
                        new Claim("Email", loginDto.Email),
                        new Claim("Role", roles)};

                    ITokenHandler tokenHandler = new TokenHandler(_configuration);
                    var getToken = tokenHandler.GenerateToken(claims);
                    return getToken;
                }
            }
        }
        return "0";
    }

    public IEnumerable<GetAccountDto>? GetAccount()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return null; // No accounts found
        }

        var toDto = accounts.Select(account =>
                                            new GetAccountDto
                                            {
                                                Guid = account.Guid,
                                                Password = account.Password,
                                                IsDeleted = account.IsDeleted,
                                                Otp = account.Otp,
                                                IsUsed = account.IsUsed,
                                                ExpiredTime = account.ExpiredTime
                                            }).ToList();

        return toDto; // Accounts found
    }

    public GetAccountDto? GetAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return null; // Account not found
        }

        var toDto = new GetAccountDto
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            Otp = account.Otp,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };

        return toDto; // Accounts found
    }

    public GetAccountDto? CreateAccount(NewAccountDto newAccountDto)
    {
        var account = new Account
        {
            Password = Hashing.HashPassword(newAccountDto.Password),
            IsDeleted = newAccountDto.IsDeleted,
            Otp = newAccountDto.Otp,
            IsUsed = newAccountDto.IsUsed,
            ExpiredTime = newAccountDto.ExpiredTime,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccount = _accountRepository.Create(account);
        if (createdAccount is null)
        {
            return null; // Account not created
        }

        var toDto = new GetAccountDto
        {
            Guid = createdAccount.Guid,
            Password = createdAccount.Password,
            IsDeleted = createdAccount.IsDeleted,
            Otp = createdAccount.Otp,
            IsUsed = createdAccount.IsUsed,
            ExpiredTime = createdAccount.ExpiredTime
        };

        return toDto; // Account created
    }

    public int UpdateAccount(UpdateAccountDto updateAccountDto)
    {
        var isExist = _accountRepository.IsExist(updateAccountDto.Guid);
        if (!isExist)
        {
            return -1; // Account not found
        }

        var getAccount = _accountRepository.GetByGuid(updateAccountDto.Guid);

        var account = new Account
        {
            Guid = updateAccountDto.Guid,
            Password = Hashing.HashPassword(updateAccountDto.Password),
            IsDeleted = (bool)updateAccountDto.IsDeleted,
            Otp = (int)updateAccountDto.Otp,
            IsUsed = (bool)updateAccountDto.IsUsed,
            ExpiredTime = (DateTime)updateAccountDto.ExpiredTime,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate
        };

        var isUpdate = _accountRepository.Update(account);
        if (!isUpdate)
        {
            return 0; // Account not updated
        }

        return 1;
    }

    public int DeleteAccount(Guid guid)
    {
        var isExist = _accountRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Account not found
        }

        var account = _accountRepository.GetByGuid(guid);
        var isDelete = _accountRepository.Delete(account!);
        if (!isDelete)
        {
            return 0; // Account not deleted
        }

        return 1;
    }
}