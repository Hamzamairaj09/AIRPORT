namespace AIRPORT.Models
{
    public class ScheduledFlight
    {
        public int Id { get; set; }
        public string FlightName { get; set; } // e.g., "PK-302"
        public int StartTime { get; set; }     // e.g., 1000 (10:00 AM)
        public int EndTime { get; set; }       // e.g., 1100 (11:00 AM)
        public int AssignedGate { get; set; } = -1; // -1 means no gate yet
    }
}