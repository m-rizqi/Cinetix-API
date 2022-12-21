using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class MovieWithReviewsContext : DbContext
    {
        public MovieWithReviewsContext(DbContextOptions<MovieWithReviewsContext> options) : base(options)
        {
        }
        public DbSet<MovieWithReviews> MovieWithReviews { get; set; }
    }
}
