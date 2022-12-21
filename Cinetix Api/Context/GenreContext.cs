using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class GenreContext : DbContext
    {
        public GenreContext(DbContextOptions<GenreContext> options) : base(options)
        {
        }
        public DbSet<Genre> Genres { get; set; }
    }
}
