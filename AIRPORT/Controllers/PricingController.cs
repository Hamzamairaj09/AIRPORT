using AIRPORT.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIRPORT.Controllers
{
    public class BookingRequest
    {
        public int SeatNumber { get; set; }
        public string FlightName { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly PricingService _service;

        public PricingController(PricingService service)
        {
            _service = service;
        }

        [HttpPost("book")]
        public IActionResult BookSeat([FromBody] BookingRequest request)
        {
            // FIX: Call BookSeat directly. It now returns the price (double).
            double price = _service.BookSeat(request.SeatNumber);

            // If price is -1, it means the seat is invalid or taken
            if (price == -1)
            {
                return BadRequest(new { message = "Seat is invalid or already booked." });
            }

            return Ok(new
            {
                message = $"Success! Seat {request.SeatNumber} booked for Flight {request.FlightName}.",
                price = $"${price}"
            });
        }
    }
}