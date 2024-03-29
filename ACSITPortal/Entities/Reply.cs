﻿namespace ACSITPortal.Entities
{
    public class Reply
    {
        // Reply Information
        public int ReplyId { get; set; }
        public string ReplyContent { get; set; }

        // Metadata
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        // Relationships
        public int UserId { get; set; } // NAV to User
        public int ThreadId { get; set; } // NAV to Thread
    }
}
