using Microsoft.EntityFrameworkCore;
using ACSITPortal.Entities;

namespace ACSITPortal.Data
{
    public class ACSITPortalContext : DbContext
    {
        public ACSITPortalContext(DbContextOptions<ACSITPortalContext> options)
            : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Entities.Thread> Threads { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}
