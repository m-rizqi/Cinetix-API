using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class SeatContext : DbContext
    {
        public SeatContext(DbContextOptions<SeatContext> options) : base(options)
        {
        }
        public DbSet<Seat> Seats { get; set; }
    }
}
