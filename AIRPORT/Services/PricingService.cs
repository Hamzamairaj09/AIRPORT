using AIRPORT.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIRPORT.Services
{
    public class PricingService
    {
        private readonly Flight _flight;
        private double _totalRevenue = 0;
        private const double BasePrice = 5000;

        public PricingService()
        {
            _flight = new Flight { FlightCode = "PK-301" };
            _flight.Seats = new List<Seat>(); // Ensure list is initialized
            for (int i = 1; i <= 20; i++)
            {
                _flight.Seats.Add(new Seat { SeatNumber = i, IsBooked = false });
            }
        }

        public List<Seat> GetSeats()
        {
            return _flight.Seats;
        }

        // Logic: Calculate Dynamic Price
        public double CalculatePrice(int availableSeats, int totalSeats)
        {
            double demandRatio = (double)(totalSeats - availableSeats) / totalSeats;
            double dynamicPrice = BasePrice + (BasePrice * demandRatio);
            return Math.Round(dynamicPrice, 2);
        }

        // --- UPDATED METHOD: Returns the Price (double) instead of string ---
        public double BookSeat(int seatNo)
        {
            var seat = _flight.Seats.FirstOrDefault(s => s.SeatNumber == seatNo);

            // Return -1 if invalid or taken
            if (seat == null || seat.IsBooked) return -1;

            // Calculate Price before booking
            int available = _flight.Seats.Count(s => !s.IsBooked);
            double finalPrice = CalculatePrice(available, _flight.Seats.Count);

            // Confirm Booking
            seat.IsBooked = true;
            _totalRevenue += finalPrice;

            return finalPrice; // Return the actual price
        }

        public BookingStats GetStatistics()
        {
            int available = _flight.Seats.Count(s => !s.IsBooked);
            return new BookingStats
            {
                TotalSeats = _flight.Seats.Count,
                AvailableSeats = available,
                BookedSeats = _flight.Seats.Count - available,
                TotalRevenue = _totalRevenue
            };
        }
    }
}