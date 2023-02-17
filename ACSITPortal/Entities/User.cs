namespace ACSITPortal.Entities
{
    public class User
    {
        // Account information
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }

        // Metadata
        public DateTime DateCreated { get; set; }

        // Relations
        public ICollection<Post>? Posts { get; set; }
    }
}
