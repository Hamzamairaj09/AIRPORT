using System.Collections.Generic;

namespace AIRPORT.Models
{
    public class PathRoute
    {
        public List<string> Path { get; set; }
        public int Cost { get; set; }

        public PathRoute(List<string> path, int cost)
        {
            Path = path;
            Cost = cost;
        }
    }
}
