using Microsoft.AspNetCore.Mvc;
using AIRPORT.Services;
using AIRPORT.Models;

namespace AIRPORT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GateController : ControllerBase
    {
        private readonly GateService _service;

        public GateController(GateService service)
        {
            _service = service;
        }

        // POST: Add a new flight to the list
        [HttpPost("add")]
        public IActionResult AddFlight(string name, int start, int end)
        {
            _service.AddFlight(name, start, end);
            return Ok($"Flight {name} added to schedule.");
        }
        // Add this to GateController.cs
        [HttpDelete("cancel/{id}")]
        public IActionResult CancelFlight(int id)
        {
            _service.CancelFlight(id);
            return Ok($"Flight {id} cancelled.");
        }

        // GET: Run the algorithm and return results
        [HttpGet("schedule")]
        public IActionResult GetGateSchedule()
        {
            // Run the Graph Coloring logic
            _service.AssignGates();

            // Return the list with assigned gates
            return Ok(_service.GetSchedule());
        }
    }
}