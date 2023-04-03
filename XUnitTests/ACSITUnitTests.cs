using ACSITPortal.Entities;
using ACSITPortal.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ACSITPortalTests
{
    public class Tests : IClassFixture<ACSITFixtures>
    {
        public Tests(ACSITFixtures fixture) => _fixture = fixture;

        ACSITFixtures _fixture { get; set; }

        [Fact]
        public void CreateUser_ShouldReturnAllPosts()
        {
            // Arrange
            using var context = _fixture.CreateContext();
            var posts = new List<Post>();

            // Act
            var result = context.Posts.Where(p => p.PostId > 0).ToList();

            // Assert
            Assert.NotNull(result);
        }
    }
}