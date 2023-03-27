namespace ACSITPortal.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string? MessageTitle { get; set; }
        public string? MessageContent { get; set; }

        // Metadata
        public DateTime MessageSentDate { get; set; }
        public int SenderId { get; set; }
        public int RecepientId { get; set; }
    }
}
