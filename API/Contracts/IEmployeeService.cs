using API.DTOs.Employees;

namespace API.Contracts;

public interface IEmployeeService
{
    IEnumerable<GetEmployeeDto> GetEmployee();
}