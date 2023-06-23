using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class AccountRoleRepository : IAccountRoleRepository
{
    private readonly BookingDbContext _context;

    public AccountRoleRepository(BookingDbContext context)
    {
        _context = context;
    }

    public ICollection<AccountRole> GetAll()
    {
        return _context.Set<AccountRole>().ToList();
    }

    public AccountRole? GetByGuid(Guid guid)
    {
        return _context.Set<AccountRole>().Find(guid);
    }

    public AccountRole Create(AccountRole university)
    {
        try
        {
            _context.Set<AccountRole>().Add(university);
            _context.SaveChanges();
            return university;
        }
        catch
        {
            return new AccountRole();
        }
    }

    public bool Update(AccountRole university)
    {
        try
        {
            _context.Set<AccountRole>().Update(university);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Guid guid)
    {
        try
        {
            var university = GetByGuid(guid);
            if (university is null)
            {
                return false;
            }

            _context.Set<AccountRole>().Remove(university);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}