using API.DTOs.Bookings;

namespace API.Contracts;

public interface IBookingService
{
    IEnumerable<GetBookingDto> GetBooking();
}