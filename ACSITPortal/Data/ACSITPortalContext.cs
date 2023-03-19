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

        public void EnableIdentityInsert()
        {
            using (var transaction = Database.BeginTransaction())
            {
                Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Users] ON;");
                transaction.Commit();
            };
        }

        public void DisableIdentityInsert()
        {
            using (var transaction = Database.BeginTransaction())
            {
                Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Users] OFF;");
                transaction.Commit();
            };
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Entities.Thread> Threads { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}
