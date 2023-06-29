using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using API.Utilities.Enums;
using static API.DTOs.Bookings.BookingDetailsDto;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public IEnumerable<GetBookingDto>? GetBooking()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return null; // No bookings found
        }

        var toDto = bookings.Select(booking =>
                                            new GetBookingDto
                                            {
                                                Guid = booking.Guid,
                                                StartDate = booking.StartDate,
                                                EndDate = booking.EndDate,
                                                Status = booking.Status,
                                                remarks = booking.Remarks
                                            }).ToList();

        return toDto; // Bookings found
    }

    public List<BookingDetailsDto>? GetBookingDetails()
    {
        var bookings = _bookingRepository.GetBookingDetails();
        var bookingDetails = bookings.Select(b => new BookingDetailsDto
        {
            Guid = b.Guid,
            BookedNIK = b.BookedNIK,
            BookedBy = b.BookedBy,
            RoomName = b.RoomName,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Status = b.Status,
            Remarks = b.Remarks
        }).ToList();

        return bookingDetails;
    }

    public GetBookingDto? GetBooking(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return null; // Booking not found
        }

        var toDto = new GetBookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            remarks = booking.Remarks
        };

        return toDto; // Bookings found
    }

    public GetBookingDto? CreateBooking(NewBookingDto newBookingDto)
    {
        var booking = new Booking
        {
            StartDate = newBookingDto.StartDate,
            EndDate = (DateTime)newBookingDto.EndDate,
            Status = newBookingDto.Status,
            Remarks = newBookingDto.remarks,
            Guid = new Guid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdBooking = _bookingRepository.Create(booking);
        if (createdBooking is null)
        {
            return null; // Booking not created
        }

        var toDto = new GetBookingDto
        {
            Guid = createdBooking.Guid,
            StartDate = createdBooking.StartDate,
            EndDate = createdBooking.EndDate,
            Status = createdBooking.Status,
            remarks = createdBooking.Remarks
        };

        return toDto; // Booking created
    }

    public int UpdateBooking(UpdateBookingDto updateBookingDto)
    {
        var isExist = _bookingRepository.IsExist(updateBookingDto.Guid);
        if (!isExist)
        {
            return -1; // Booking not found
        }

        var getBooking = _bookingRepository.GetByGuid(updateBookingDto.Guid);

        var booking = new Booking
        {
            Guid = updateBookingDto.Guid,
            StartDate = updateBookingDto.StartDate,
            EndDate = (DateTime)updateBookingDto.EndDate,
            Status = updateBookingDto.Status,
            Remarks = updateBookingDto.remarks,
            ModifiedDate = DateTime.Now,
            CreatedDate = getBooking!.CreatedDate
        };

        var isUpdate = _bookingRepository.Update(booking);
        if (!isUpdate)
        {
            return 0; // Booking not updated
        }

        return 1;
    }

    public int DeleteBooking(Guid guid)
    {
        var isExist = _bookingRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // Booking not found
        }

        var booking = _bookingRepository.GetByGuid(guid);
        var isDelete = _bookingRepository.Delete(booking!);
        if (!isDelete)
        {
            return 0; // Booking not deleted
        }

        return 1;
    }
}