using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Educations;

public class NewEducationDto
{
    [Required]
    public string Major { get; set; }

    [Required]
    public string Degree { get; set; }

    [Required]
    [Range(0, 4)]
    public double Gpa { get; set; }

}