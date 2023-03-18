namespace ACSITPortal.Entities
{
    public class User
    {
        // Account information
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserSalt { get; set; }
        public string Email { get; set; }

        // Metadata
        public DateTime DateCreated { get; set; }
        public string? ActionToken { get; set; }
        public DateTime? TokenExpires { get; set; }

        // Relations
        public ICollection<Post>? Posts { get; set; }
    }
}
