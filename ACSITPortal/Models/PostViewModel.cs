using ACSITPortal.Entities;

namespace ACSITPortal.Models
{
    public class PostViewModel
    {
        public Post? Post { get; set; }
        public User? User { get; set; }

        public List<Entities.Thread>? Threads { get; set; }
    }
}
