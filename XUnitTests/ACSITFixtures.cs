using Microsoft.EntityFrameworkCore;
using ACSITPortal.Data;
using ACSITPortal.Entities;

namespace ACSITPortalTests
{
    public class ACSITFixtures
    {
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public ACSITFixtures()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Users.Add(
                            new User
                            {
                                UserLogin = "TestUser",
                                UserPassword = "Qw3rTux6Yzv",
                                UserSalt = "123",
                                Email = "test@test.com",
                                DateCreated = DateTime.Now,
                                Messages = new List<Message> { new Message { MessageTitle = "Title", MessageContent = "Content", MessageSentDate = DateTime.Now, RecepientId = 99999, SenderId = 99999 } },
                                ActionToken = "A",
                                TokenExpires= DateTime.Now.AddMinutes(5),
                                Posts= new List<Post> { new Post { PostTitle = "Title", PostContent = "Content", DateCreated = DateTime.Now, DateUpdated = DateTime.Now, Reports = 0, Threads = Enumerable.Empty<ACSITPortal.Entities.Thread>().ToList(), UserId = 99999 } },
                                VerificationExpires= DateTime.Now.AddMinutes(5),
                                Verified = true
                            });
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public ACSITPortalContext CreateContext()
        {
            return new ACSITPortalContext(new DbContextOptionsBuilder<ACSITPortalContext>()
            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ACSITPortal;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
            .Options);
        }
    }
}
