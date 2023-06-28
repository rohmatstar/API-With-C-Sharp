using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Universities;
using API.Models;
using API.Repositories;
using API.Utilities;
using API.Utilities.Enums;
using System.Net;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
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