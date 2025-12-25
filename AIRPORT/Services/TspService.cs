using System.Collections.Generic;
using System.Linq;

namespace AIRPORT.Services
{
    public class TspService
    {
        // 1. Full Distance Matrix (Ensures no "missing path" errors)
        private readonly Dictionary<(string, string), int> _distances = new()
        {
            // Domestic
            { ("KHI", "LHR"), 1200 }, { ("LHR", "KHI"), 1200 },
            { ("KHI", "ISL"), 1400 }, { ("ISL", "KHI"), 1400 },
            { ("KHI", "MUL"), 900 },  { ("MUL", "KHI"), 900 },
            { ("LHR", "ISL"), 300 },  { ("ISL", "LHR"), 300 },
            { ("LHR", "MUL"), 350 },  { ("MUL", "LHR"), 350 },
            { ("ISL", "MUL"), 500 },  { ("MUL", "ISL"), 500 },
            // International
            { ("KHI", "DXB"), 1180 }, { ("DXB", "KHI"), 1180 },
            { ("LHR", "DXB"), 3400 }, { ("DXB", "LHR"), 3400 },
            { ("DXB", "IST"), 1800 }, { ("IST", "DXB"), 1800 },
            { ("IST", "NYC"), 5000 }, { ("NYC", "IST"), 5000 },
            { ("LHR", "NYC"), 3450 }, { ("NYC", "LHR"), 3450 },
            { ("DXB", "NYC"), 6800 }, { ("NYC", "DXB"), 6800 },
            { ("KHI", "IST"), 2400 }, { ("IST", "KHI"), 2400 },
            { ("IST", "LHR"), 1560 }, { ("LHR", "IST"), 1560 },
            { ("KHI", "NYC"), 7300 }, { ("NYC", "KHI"), 7300 }
        };

        // 2. Flight Numbers (For Buttons)
        private readonly Dictionary<(string, string), string> _flightNos = new()
        {
            { ("KHI", "LHR"), "PK-302" }, { ("LHR", "KHI"), "PK-303" },
            { ("KHI", "ISL"), "PK-368" }, { ("ISL", "KHI"), "PK-369" },
            { ("KHI", "MUL"), "PK-501" }, { ("MUL", "KHI"), "PK-502" },
            { ("LHR", "ISL"), "PK-451" }, { ("ISL", "LHR"), "PK-452" },
            { ("LHR", "MUL"), "PK-777" }, { ("MUL", "LHR"), "PK-778" },
            { ("ISL", "MUL"), "PK-222" }, { ("MUL", "ISL"), "PK-223" },
            // International
            { ("KHI", "DXB"), "EK-600" }, { ("DXB", "KHI"), "EK-601" },
            { ("LHR", "DXB"), "BA-105" }, { ("DXB", "LHR"), "BA-106" },
            { ("DXB", "IST"), "TK-760" }, { ("IST", "DXB"), "TK-761" },
            { ("IST", "LHR"), "TK-199" }, { ("LHR", "IST"), "TK-198" },
            { ("LHR", "NYC"), "VS-001" }, { ("NYC", "LHR"), "VS-002" },
            { ("DXB", "NYC"), "EK-201" }, { ("NYC", "DXB"), "EK-202" },
            { ("IST", "NYC"), "TK-003" }, { ("NYC", "IST"), "TK-004" }
        };

        public object OptimizeRoute(List<string> selectedCities)
        {
            if (selectedCities == null || selectedCities.Count < 2)
                return new { route = new List<string>(), flightCodes = new List<string>(), totalDistance = 0 };

            string startNode = selectedCities[0];
            List<string> bestPath = new List<string>();
            int minDistance = int.MaxValue;

            var others = selectedCities.Skip(1).ToList();
            var permutations = GetPermutations(others, others.Count);

            foreach (var perm in permutations)
            {
                var currentPath = new List<string> { startNode };
                currentPath.AddRange(perm);
                currentPath.Add(startNode);

                int currentDist = CalculatePathDistance(currentPath);
                if (currentDist < minDistance)
                {
                    minDistance = currentDist;
                    bestPath = currentPath;
                }
            }

            // Extract Flight Codes
            List<string> codes = new List<string>();
            for (int i = 0; i < bestPath.Count - 1; i++)
            {
                if (_flightNos.TryGetValue((bestPath[i], bestPath[i + 1]), out string c)) codes.Add(c);
                else codes.Add("CHARTER");
            }

            return new { route = bestPath, flightCodes = codes, totalDistance = minDistance };
        }

        private int CalculatePathDistance(List<string> path)
        {
            int dist = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                if (_distances.TryGetValue((path[i], path[i + 1]), out int d)) dist += d;
                else dist += 99999;
            }
            return dist;
        }

        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}