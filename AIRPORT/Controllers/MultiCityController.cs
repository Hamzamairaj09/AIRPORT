using Microsoft.AspNetCore.Mvc;
using AIRPORT.Services;
using System.Collections.Generic;

namespace AIRPORT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultiCityController : ControllerBase
    {
        private readonly TspService _service;

        public MultiCityController(TspService service)
        {
            _service = service;
        }

        // --- THIS MUST BE [HttpPost], NOT [HttpGet] ---
        [HttpPost("optimize")]
        public IActionResult Optimize([FromBody] List<string> cities)
        {
            // 1. Validation: Crash prevention
            if (cities == null || cities.Count < 2)
                return BadRequest("Please select at least 2 cities.");

            // 2. Call the Logic
            var result = _service.OptimizeRoute(cities);

            // 3. Return Success
            return Ok(result);
        }
    }
}