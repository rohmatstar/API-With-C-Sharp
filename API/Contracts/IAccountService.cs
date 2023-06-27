using API.DTOs.Accounts;

namespace API.Contracts;

public interface IAccountService
{
    IEnumerable<GetAccountDto> GetAccount();
}