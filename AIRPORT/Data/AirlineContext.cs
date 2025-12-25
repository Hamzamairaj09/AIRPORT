using Microsoft.EntityFrameworkCore;

namespace AIRPORT.Data
{
    public class AirlineContext : DbContext
    {
        public AirlineContext(DbContextOptions<AirlineContext> options) : base(options)
        {
        }
    }
}
