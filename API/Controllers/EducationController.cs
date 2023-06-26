using API.Contracts;
using API.DTOs.Educations;
using API.Models;
using API.Services;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EducationController : ControllerBase
    {
        private readonly EducationService _service;

        public EducationController(EducationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetEducation();

            if (!entities.Any())
            {
                return NotFound(new ResponseHandler<GetEducationDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetEducationDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var education = _service.GetEducation(guid);
            if (education is null)
            {
                return NotFound(new ResponseHandler<GetEducationDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<GetEducationDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = education
            });
        }

        [HttpPost]
        public IActionResult Create(NewEducationDto newEducationDto)
        {
            var createdEducation = _service.CreateEducation(newEducationDto);
            if (createdEducation is null)
            {
                return BadRequest(new ResponseHandler<GetEducationDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandler<GetEducationDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdEducation
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateEducationDto updateEducationDto)
        {
            var update = _service.UpdateEducation(updateEducationDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandler<UpdateEducationDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new ResponseHandler<UpdateEducationDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new ResponseHandler<UpdateEducationDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteEducation(guid);

            if (delete is -1)
            {
                return NotFound(new ResponseHandler<GetEducationDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new ResponseHandler<GetEducationDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new ResponseHandler<GetEducationDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }

        [HttpGet("search/{name}")]
        public IActionResult GetByName(string name)
        {
            var educations = _service.GetEducation(name);
            if (!educations.Any())
            {
                return NotFound(new ResponseHandler<GetEducationDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No educations found with the given name"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetEducationDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Educations found",
                Data = educations
            });
        }
    }
}