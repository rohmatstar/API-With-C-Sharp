using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountRoleController : GeneralController<AccountRole>
    {
        public AccountRoleController(IAccountRoleRepository repository) : base(repository) { }

    }
}