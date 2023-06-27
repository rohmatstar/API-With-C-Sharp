using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    IEnumerable<Employee> GetByName(string name);
}
