using System.Collections.Generic;

namespace AIRPORT.Models
{
    public class FlightEdge
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Price { get; set; }
        public string FlightNo { get; set; } // <--- Stores "PK-302", "EK-600" etc.
    }

    public class RouteResult
    {
        public List<string> Route { get; set; } = new List<string>();
        public List<string> FlightCodes { get; set; } = new List<string>(); // <--- List of flight numbers for the route
        public int TotalPrice { get; set; }
        public string Status { get; set; }
    }
}