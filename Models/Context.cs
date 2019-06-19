using Microsoft.EntityFrameworkCore;

namespace Wedding_Planner.Models {
    public class Context : DbContext {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }
    }
}