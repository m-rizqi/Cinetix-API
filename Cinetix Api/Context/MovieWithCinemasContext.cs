using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class MovieWithCinemasContext : DbContext
    {
        public MovieWithCinemasContext(DbContextOptions<MovieWithCinemasContext> options) : base(options)
        {
        }
        public DbSet<MovieWithCinemas> MovieWithCinemas { get; set; }
    }
}
