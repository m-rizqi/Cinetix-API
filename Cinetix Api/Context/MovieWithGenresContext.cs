using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class MovieWithGenresContext : DbContext
    {
        public MovieWithGenresContext(DbContextOptions<MovieWithGenresContext> options) : base(options)
        {
        }
        public DbSet<MovieWithGenres> MovieWithGenres { get; set; }
    }
}
