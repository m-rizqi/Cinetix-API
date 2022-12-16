using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options)
        {
        }
        public DbSet<Review> Review { get; set; }
    }
}
