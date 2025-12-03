// Models/FlightModels.cs
using System.Collections.Generic;

namespace FlightSearchApi.Models
{
    // Represents a single flight connection (Edge)
    public class FlightEdge
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Price { get; set; } // This is the "Weight" of the edge
    }

    // Represents the final output for Hamza's UI
    public class RouteResult
    {
        public List<string> Route { get; set; } = new List<string>();
        public int TotalPrice { get; set; }
        public string Status { get; set; }
    }
}