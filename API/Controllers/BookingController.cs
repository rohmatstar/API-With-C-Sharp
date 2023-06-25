using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BookingController : GeneralController<Booking>
    {
        public BookingController(IBookingRepository repository) : base(repository) { }
    }
}