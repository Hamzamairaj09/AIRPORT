namespace AIRPORT.Models
{
    public class FlightRoute
    {
        public string To { get; set; }
        public int Distance { get; set; }

        public FlightRoute(string to, int distance)
        {
            To = to;
            Distance = distance;
        }
    }
}
