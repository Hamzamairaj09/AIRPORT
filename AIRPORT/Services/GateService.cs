using AIRPORT.Models;
using System.Collections.Generic;
using System.Linq;

namespace AIRPORT.Services
{
    public class GateService
    {
        private List<ScheduledFlight> _schedule = new List<ScheduledFlight>();

        // 1. Add a flight to the schedule
        public void AddFlight(string name, int start, int end)
        {
            int newId = _schedule.Count + 1;
            _schedule.Add(new ScheduledFlight
            {
                Id = newId,
                FlightName = name,
                StartTime = start,
                EndTime = end
            });
        }
        // Add this to GateService.cs
        public void CancelFlight(int id)
        {
            var flight = _schedule.FirstOrDefault(f => f.Id == id);
            if (flight != null) _schedule.Remove(flight);
        }

        // 2. Get the list (for UI)
        public List<ScheduledFlight> GetSchedule()
        {
            return _schedule;
        }

        // 3. THE ALGORITHM: Assign Gates using Graph Coloring
        public void AssignGates()
        {
            // Sort flights by start time (Greedy approach helper)
            var sortedFlights = _schedule.OrderBy(f => f.StartTime).ToList();

            // Gates are just integers: 1, 2, 3...
            // We try to fit the flight into the first available gate

            foreach (var flight in sortedFlights)
            {
                int gateNumber = 1;
                while (true)
                {
                    if (IsGateAvailable(gateNumber, flight))
                    {
                        flight.AssignedGate = gateNumber;
                        break;
                    }
                    gateNumber++; // Try next gate
                }
            }
        }

        // Helper: Checks if this gate is free during the flight's time
        private bool IsGateAvailable(int gate, ScheduledFlight currentFlight)
        {
            // Find all flights currently assigned to this gate
            var flightsOnGate = _schedule.Where(f => f.AssignedGate == gate).ToList();

            foreach (var f in flightsOnGate)
            {
                // Check for Overlap
                // (StartA < EndB) and (StartB < EndA) means they overlap
                if (currentFlight.StartTime < f.EndTime && f.StartTime < currentFlight.EndTime)
                {
                    return false; // Conflict found!
                }
            }
            return true; // No conflict
        }
    }
}