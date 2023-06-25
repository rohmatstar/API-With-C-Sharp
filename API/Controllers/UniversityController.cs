using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UniversityController : GeneralController<University>
    {
        public UniversityController(IUniversityRepository repository) : base(repository) { }
    }
}