using Cinetix_Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Cinetix_Api.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
