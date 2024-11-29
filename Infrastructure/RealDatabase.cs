

using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class RealDatabase : DbContext
    {
        public RealDatabase(DbContextOptions<RealDatabase> options)
        : base(options)
        {

        }
        protected RealDatabase() { }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
