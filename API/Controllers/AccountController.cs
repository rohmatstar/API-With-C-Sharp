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
using System.Xml.Linq;
using System.Security.Principal;
using API.DTOs.AccountRoles;
using API.Repositories;

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
        private readonly RoleService _RoleService;

        public AccountController(AccountService service, EmployeeService employeeService, EducationService educationService, UniversityService universityService, RoleService roleService)
        {
            _service = service;
            _EmployeeService = employeeService;
            _EducationService = educationService;
            _UniversityService = universityService;
            _RoleService = roleService;
        }

        [HttpPost("forget-password")]
        public IActionResult ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            // Get Employee By Email
            var getEmployee = _EmployeeService.GetByEmail(forgetPasswordDto.Email)!;
            if (getEmployee is null)
            {
                return NotFound(new ResponseHandler<ForgetPasswordDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No Account Found"
                });
            }

            // Generate OTP
            int generatedOtp = _service.GenerateOtp();

            // Get Account By Employee.Guid
            var getAccount = _service.GetAccount(getEmployee.Guid);

            // Update Otp, Expired Time, isUsed in Account
            var otpExpiredTime = DateTime.Now.AddMinutes(5);
            var updateAccountDto = new UpdateAccountDto
            {
                Guid = getAccount!.Guid,
                Password = getAccount.Password,
                IsDeleted = (bool)getAccount.IsDeleted,
                Otp = generatedOtp,
                IsUsed = false,
                ExpiredTime = otpExpiredTime
            };

            var updateResult = _service.UpdateAccount(updateAccountDto);
            if (updateResult == 0)
            {
                return NotFound(new ResponseHandler<ForgetPasswordDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to Setting OTP in Related Account"
                });
            }

            // Success to Create OTP and Update the Account Model
            return Ok(new ResponseHandler<IEnumerable<OtpResponseDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "OTP Sent",
                Data = new List<OtpResponseDto> { new OtpResponseDto {
                    Guid = getAccount.Guid,
                    Email = getEmployee.Email,
                    Otp = generatedOtp,
                    ExpiredTime = otpExpiredTime
                } }
            }) ;
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
                registerEducation.Guid = CreateEmployee!.Guid;

                var CreateEducation = _EducationService.CreateEducation(registerEducation);
                var isEducationCreated = CreateEducation is not null;

                if (isEducationCreated)
                {
                    // Account
                    var registerAccount = new NewAccountDto();
                    registerAccount.Password = registerDto.Password;
                    registerAccount.Guid = CreateEducation!.UniversityGuid;
                    registerAccount.IsDeleted = false;
                    registerAccount.Otp = 0;
                    registerAccount.IsUsed = false;
                    registerAccount.ExpiredTime = DateTime.Now;

                    var CreateAccount = _service.CreateAccount(registerAccount);
                    var isAccountCreated = CreateAccount is not null;

                    // University + Validation University
                    var isUniversityExist = _UniversityService.GetByCodeAndName(registerDto.UniversityCode, registerDto.UniversityName) is null;

                    if (isUniversityExist)
                    {
                        return BadRequest(new ResponseHandler<RegisterDto>
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Failed to register. Check your request"
                        });
                    }

                    var registerUniversity = new NewUniversityDto();
                    registerUniversity.Code = registerDto.UniversityCode;
                    registerUniversity.Name = registerDto.UniversityName;

                    var CreateUniversity = _UniversityService.CreateUniversity(registerUniversity);
                    var isUniversityCreated = CreateUniversity is not null;

                    if (isAccountCreated && isUniversityCreated)
                    {

                        // Account Roles
                        var userRoleGuid = _RoleService.GetRole().FirstOrDefault(ur => ur.Name == "User");
                        var registerAccountRoles = new NewAccountRoleDto();
                        registerAccountRoles.AccountGuid = CreateAccount.Guid;
                        registerAccountRoles.RoleGuid = userRoleGuid.Guid;

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

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var loginStatus = _service.Login(loginDto);
            if (loginStatus == "0")
            {
                return BadRequest(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Login Failed"
                });
            }

            return Ok(new ResponseHandler<dynamic>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Login Successfully",
                Data = new { token = loginStatus }
            });
        }

        [HttpPut("change-password")]
        public IActionResult Update(ChangePasswordDto changePasswordDto)
        {
            var update = _service.ChangePassword(changePasswordDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Email not Found"
                });
            }
            if (update is 0)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Otp doesn't match"
                });
            }
            if (update is 1)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Otp has been used"
                });
            }
            if (update is 2)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Otp alredy expired"
                });
            }
            return Ok(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
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