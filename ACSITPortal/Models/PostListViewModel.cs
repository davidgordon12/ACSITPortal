using ACSITPortal.Entities;

namespace ACSITPortal.Models
{
    public class PostListViewModel
    {
        public ICollection<Post>? Posts { get; set; }
        public User? User { get; set; }
    }
}
