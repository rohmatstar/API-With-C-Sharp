using API.Contracts;
using API.Models;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UniversityController : GeneralController<University>
    {
        private readonly IUniversityRepository _repository;

        public UniversityController(IUniversityRepository repository) : base(repository)
        {
            _repository = repository;
        }

        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            var university = _repository.GetByName(name);
            if (!university.Any())
            {
                return NotFound(new ResponseHandler<University>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No One Universities Found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<University>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Universities Found",
                Data = (IEnumerable<University>)university
            });
        }
    }
}