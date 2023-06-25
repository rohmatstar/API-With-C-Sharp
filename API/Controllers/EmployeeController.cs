using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EmployeeController : GeneralController<Employee>
    {
        public EmployeeController(IEmployeeRepository repository) : base(repository) { }
    }
}