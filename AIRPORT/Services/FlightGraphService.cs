
using FlightSearchApi.Models;

namespace FlightSearchApi.Services
{
    public class FlightGraphService
    {
       
        private readonly Dictionary<string, List<FlightEdge>> _flightGraph;

        public FlightGraphService()
        {
            _flightGraph = new Dictionary<string, List<FlightEdge>>();
            LoadMockData(); 
        }

        public RouteResult GetShortestPath(string start, string end)
        {
           
            if (!_flightGraph.ContainsKey(start) || !_flightGraph.ContainsKey(end))
                return new RouteResult { Status = "Invalid City Code" };

            
            var distances = new Dictionary<string, int>();
            var previousNodes = new Dictionary<string, string>();
            var priorityQueue = new PriorityQueue<string, int>(); 
            var visited = new HashSet<string>();

            
            foreach (var city in _flightGraph.Keys)
            {
                distances[city] = int.MaxValue;
            }

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

                        // If we found a cheaper way to get to the neighbor, update it
                        if (newPrice < distances[flight.To])
                        {
                            distances[flight.To] = newPrice;
                            previousNodes[flight.To] = currentCity;
                            priorityQueue.Enqueue(flight.To, newPrice);
                        }
                    }
                }
            }

            // 4. Reconstruct the Path
            return BuildPath(previousNodes, start, end, distances);
        }

        private RouteResult BuildPath(Dictionary<string, string> previous, string start, string end, Dictionary<string, int> distances)
        {
            if (distances[end] == int.MaxValue)
                return new RouteResult { Status = "No Route Found" };

            var path = new List<string>();
            var current = end;

            while (current != null && current != start)
            {
                path.Add(current);
                previous.TryGetValue(current, out current);
            }
            path.Add(start);
            path.Reverse(); // Reverse to get Start -> End

            return new RouteResult
            {
                Route = path,
                TotalPrice = distances[end],
                Status = "Success"
            };
        }

        private void LoadMockData()
        {
            // This data ensures Dijkstra picks the indirect route because it is cheaper.

            // Route A: Direct (Expensive)
            AddFlight("KHI", "LHR", 800);

            // Route B: Via Dubai (Mid-range)
            AddFlight("KHI", "DXB", 300);
            AddFlight("DXB", "LHR", 400); // Total: 700

            // Route C: Via Istanbul (Cheapest) - Dijkstra should pick this!
            AddFlight("KHI", "IST", 250);
            AddFlight("IST", "LHR", 200); // Total: 450

            // Add return or other routes
            AddFlight("LHR", "NYC", 600);
        }

        private void AddFlight(string from, string to, int price)
        {
            // Ensure nodes exist in dictionary
            if (!_flightGraph.ContainsKey(from)) _flightGraph[from] = new List<FlightEdge>();
            if (!_flightGraph.ContainsKey(to)) _flightGraph[to] = new List<FlightEdge>();

            _flightGraph[from].Add(new FlightEdge { From = from, To = to, Price = price });
        }
    }
}