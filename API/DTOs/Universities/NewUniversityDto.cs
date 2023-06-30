using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Universities;

public class NewUniversityDto
{
    public Guid Guid { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public string Name { get; set; }
}