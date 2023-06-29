using API.DTOs.Bookings;
using API.Models;

namespace API.Contracts;

public interface IBookingService
{
    IEnumerable<GetBookingDto> GetBooking();
    List<BookingDetailsDto>? GetBookingDetails();
}