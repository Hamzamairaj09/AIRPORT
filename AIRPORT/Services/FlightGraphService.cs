using AIRPORT.Models;
using System.Collections.Generic;
using System.Linq;

namespace AIRPORT.Services
{
    public class FlightGraphService
    {
        private readonly Dictionary<string, List<FlightEdge>> _flightGraph;

        // --- MAP COORDINATES (Embedded Data) ---
        private readonly Dictionary<string, double[]> _airportCoords = new()
        {
            { "KHI", new double[] { 24.8607, 67.0011 } },
            { "LHR", new double[] { 31.5204, 74.3587 } },
            { "ISL", new double[] { 33.6844, 73.0479 } },
            { "MUL", new double[] { 30.1575, 71.5249 } },
            { "DXB", new double[] { 25.2532, 55.3657 } },
            { "IST", new double[] { 41.2753, 28.7519 } },
            { "NYC", new double[] { 40.6413, -73.7781 } }
        };

        public FlightGraphService()
        {
            _flightGraph = new Dictionary<string, List<FlightEdge>>();
            LoadMockData();
        }

        public List<string> GetAvailableAirports() => _flightGraph.Keys.ToList();

        public Dictionary<string, double[]> GetAirportLocations() => _airportCoords;

        public RouteResult GetShortestPath(string start, string end)
        {
            if (!_flightGraph.ContainsKey(start) || !_flightGraph.ContainsKey(end))
                return new RouteResult { Status = "Invalid City" };

            var distances = new Dictionary<string, int>();

            // KEY CHANGE: We store the 'FlightEdge' (the whole flight info), not just the city name string.
            // This allows BuildPath to access the FlightNo later.
            var previousNodes = new Dictionary<string, FlightEdge>();

            var priorityQueue = new PriorityQueue<string, int>();
            var visited = new HashSet<string>();

            foreach (var city in _flightGraph.Keys) distances[city] = int.MaxValue;

            distances[start] = 0;
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                if (!priorityQueue.TryDequeue(out string currentCity, out int currentPrice)) break;
                if (currentCity == end) break;
                if (visited.Contains(currentCity)) continue;
                visited.Add(currentCity);

                if (_flightGraph.TryGetValue(currentCity, out var neighbors))
                {
                    foreach (var flight in neighbors)
                    {
                        int newPrice = distances[currentCity] + flight.Price;
                        if (newPrice < distances[flight.To])
                        {
                            distances[flight.To] = newPrice;
                            previousNodes[flight.To] = flight; // Store the FLIGHT object here
                            priorityQueue.Enqueue(flight.To, newPrice);
                        }
                    }
                }
            }
            return BuildPath(previousNodes, start, end, distances);
        }

        // --- UPDATED BUILD PATH METHOD (EMBEDDED HERE) ---
        private RouteResult BuildPath(Dictionary<string, FlightEdge> previous, string start, string end, Dictionary<string, int> distances)
        {
            if (distances[end] == int.MaxValue) return new RouteResult { Status = "No Route Found" };

            var path = new List<string>();
            var flightCodes = new List<string>();
            var current = end;

            while (current != start)
            {
                path.Add(current);
                if (previous.TryGetValue(current, out var edge))
                {
                    // This is where we extract the flight number (e.g. "PK-302")
                    flightCodes.Add(edge.FlightNo);
                    current = edge.From;
                }
                else break;
            }
            path.Add(start);

            // Reverse lists so they go from Start -> End
            path.Reverse();
            flightCodes.Reverse();

            return new RouteResult
            {
                Route = path,
                FlightCodes = flightCodes, // Sends ["PK-302", "EK-600"] to frontend
                TotalPrice = distances[end],
                Status = "Success"
            };
        }

        private void LoadMockData()
        {
            // Adding Flights with specific Flight Numbers
            AddFlight("KHI", "LHR", 120, "PK-302");
            AddFlight("KHI", "DXB", 90, "EK-600");
            AddFlight("DXB", "LHR", 110, "BA-101");
            AddFlight("KHI", "IST", 100, "TK-708");
            AddFlight("IST", "LHR", 80, "TK-199");
            AddFlight("LHR", "NYC", 250, "VS-001");

            // Domestic Connections
            AddFlight("KHI", "ISL", 150, "PK-368");
            AddFlight("ISL", "MUL", 80, "PK-555");
            AddFlight("MUL", "LHR", 70, "PK-777");
        }

        private void AddFlight(string from, string to, int price, string flightNo)
        {
            if (!_flightGraph.ContainsKey(from)) _flightGraph[from] = new List<FlightEdge>();
            if (!_flightGraph.ContainsKey(to)) _flightGraph[to] = new List<FlightEdge>();

            _flightGraph[from].Add(new FlightEdge { From = from, To = to, Price = price, FlightNo = flightNo });
        }
    }
}