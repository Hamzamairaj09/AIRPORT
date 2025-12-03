// Controllers/FlightController.cs
using FlightSearchApi.Models;
using FlightSearchApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlightGraphService _service;

        public FlightController(FlightGraphService service)
        {
            _service = service;
        }

        [HttpGet("shortest-route")]
        public IActionResult FindRoute([FromQuery] string from, [FromQuery] string to)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                return BadRequest("Please provide 'from' and 'to' city codes.");

            var result = _service.GetShortestPath(from.ToUpper(), to.ToUpper());

            if (result.Status == "Success")
                return Ok(result);

            return NotFound(result);
        }
    }
}