using API.Utilities.Enums;

namespace API.DTOs.Bookings;

public class NewBookingDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusLevel Status { get; set; }
    public string? remarks { get; set; }
}