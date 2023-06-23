using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _repository;

    public BookingController(IBookingRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var bookings = _repository.GetAll();

        if (!bookings.Any())
        {
            return NotFound();
        }

        return Ok(bookings);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var bookings = _repository.GetByGuid(guid);
        if (bookings is null)
        {
            return NotFound();
        }

        return Ok(bookings);
    }

    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        var createdBooking = _repository.Create(booking);
        return Ok(createdBooking);
    }

    [HttpPut]
    public IActionResult Update(Booking booking)
    {
        var isUpdated = _repository.Update(booking);
        if (!isUpdated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _repository.Delete(guid);
        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}