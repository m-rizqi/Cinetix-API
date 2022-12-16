using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
    }
}
