using F1Backend.Models;
using Microsoft.EntityFrameworkCore;
namespace F1Backend.Data
{
    public class F1DbContext: DbContext
    {
        public  F1DbContext(DbContextOptions<F1DbContext> options):base(options) {

        }
        public DbSet<Accomplishments> accomplishments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
