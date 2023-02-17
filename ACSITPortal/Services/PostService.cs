using ACSITPortal.Data;
using ACSITPortal.Entities;

namespace ACSITPortal.Services
{
    public class PostService
    {
        private readonly ACSITPortalContext _context;

        public List<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public Post GetPostById(int id)
        {
            try
            {
                return _context.Posts
                    .Where(p => p.PostId == id)
                    .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public bool CreatePost(Post post)
        {
            try
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePost(Post post)
        {
            try
            {
                _context.Posts.Update(post);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeletePost(Post post)
        {
            try
            {
                _context.Posts.Update(post);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
