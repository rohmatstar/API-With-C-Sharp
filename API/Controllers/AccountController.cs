using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Educations;
using API.DTOs.Universities;
using API.Models;
using API.Services;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;
        private readonly EmployeeService _EmployeeService;
        private readonly EducationService _EducationService;
        private readonly UniversityService _UniversityService;

        public AccountController(AccountService service, EmployeeService employeeService, EducationService educationService, UniversityService universityService)
        {
            _service = service;
            _EmployeeService = employeeService;
            _EducationService = educationService;
            _UniversityService = universityService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            // Employee
            var registerEmployee = new NewEmployeeDto();
            registerEmployee.FirstName = registerDto.FirstName;
            registerEmployee.LastName = registerDto.LastName;
            registerEmployee.BirthDate = registerDto.BirthDate;
            registerEmployee.Gender = registerDto.Gender;
            registerEmployee.HiringDate = registerDto.HiringDate;
            registerEmployee.Email = registerDto.Email;
            registerEmployee.PhoneNumber = registerDto.PhoneNumber;

            var CreateEmployee = _EmployeeService.CreateEmployee(registerEmployee);
            var isEmployeeCreated = CreateEmployee is not null;

            if (isEmployeeCreated)
            {
                // Education
                var registerEducation = new NewEducationDto();
                registerEducation.Major = registerDto.Major;
                registerEducation.Degree = registerDto.Degree;
                registerEducation.Gpa = registerDto.Gpa;

                var CreateEducation = _EducationService.CreateEducation(registerEducation);
                var isEducationCreated = CreateEducation is not null;

                if (isEducationCreated)
                {
                    // University
                    var registerUniversity = new NewUniversityDto();
                    registerUniversity.Code = registerDto.UniversityCode;
                    registerUniversity.Name = registerDto.UniversityName;

                    var CreateUniversity = _UniversityService.CreateUniversity(registerUniversity);
                    var isUniversityCreated = CreateUniversity is not null;

                    if (isUniversityCreated)
                    {
                        // Account
                        var registerAccount = new NewAccountDto();
                        registerAccount.Password = registerDto.Password;

                        var CreateAccount = _service.CreateAccount(registerAccount);
                        var isAccountCreated = CreateAccount is not null;

                        var isPasswordMatch = registerDto.Password == registerDto.ConfirmPassword;

                        if (isPasswordMatch)
                        {
                            return Ok(new ResponseHandler<RegisterDto>
                            {
                                Code = StatusCodes.Status200OK,
                                Status = HttpStatusCode.OK.ToString(),
                                Message = "Successfully Registered",
                                Data = registerDto
                            });
                        }
                    }
                }
            }

            return BadRequest(new ResponseHandler<RegisterDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Failed to register. Check your request"
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetAccount();

            if (entities is null)
            {
                return NotFound(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetAccountDto>>
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
            var account = _service.GetAccount(guid);
            if (account is null)
            {
                return NotFound(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = account
            });
        }

        [HttpPost]
        public IActionResult Create(NewAccountDto newAccountDto)
        {
            var createdAccount = _service.CreateAccount(newAccountDto);
            if (createdAccount is null)
            {
                return BadRequest(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdAccount
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateAccountDto updateAccountDto)
        {
            var update = _service.UpdateAccount(updateAccountDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandler<UpdateAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new ResponseHandler<UpdateAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new ResponseHandler<UpdateAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteAccount(guid);

            if (delete is -1)
            {
                return NotFound(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
    }
}