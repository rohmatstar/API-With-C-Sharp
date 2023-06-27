using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employees;

public class UpdateEmployeeDto
{
    [Required]
    public Guid Guid { get; set; }

    [Required]
    public string Nik { get; set; }

    [Required]
    public string FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    [Timestamp]
    public DateTime BirthDate { get; set; }

    [Required]
    public GenderEnum Gender { get; set; }

    [Required]
    [Timestamp]
    public DateTime HiringDate { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}