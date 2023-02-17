namespace ACSITPortal.Entities
{
    public class Thread
    {
        // Thread Information
        public int ThreadId { get; set; }
        public string ThreadTitle { get; set; }
        public string ThreadContent { get; set; }

        // Metadata
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        // Relationships
        public int PostId { get; set; } // NAV to Post
        public ICollection<Reply>? Replies { get; set; }
    }
}
