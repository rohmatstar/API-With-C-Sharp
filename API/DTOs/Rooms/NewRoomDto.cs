using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Rooms;

public class NewRoomDto
{
    [Required]
    public Guid Guid { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [MinLength(1)]
    public int Floor { get; set; }

    [Required]
    [MinLength(1)]
    public int Capacity { get; set; }
}