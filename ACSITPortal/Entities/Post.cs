namespace ACSITPortal.Entities
{
    public class Post
    {
        // Post Information
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        // Metadata
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        // Relationships
        public int UserId { get; set; } // NAV to User
        public ICollection<Thread>? Threads { get; set; }
    }
}
