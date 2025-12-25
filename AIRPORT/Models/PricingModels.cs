namespace AIRPORT.Models
{
    public class Seat
    {
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
    }

    public class Flight
    {
        public string FlightCode { get; set; }
        public List<Seat> Seats { get; set; } = new List<Seat>();
    }

    // A DTO (Data Transfer Object) for sending booking requests
    public class BookingRequest
    {
        public int SeatNumber { get; set; }
    }

    // A DTO for showing stats
    public class BookingStats
    {
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int BookedSeats { get; set; }
        public double TotalRevenue { get; set; }
    }
}