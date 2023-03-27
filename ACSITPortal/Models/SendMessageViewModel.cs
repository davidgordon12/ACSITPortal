using System.ComponentModel.DataAnnotations;

namespace ACSITPortal.Models
{
    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        public string? Recepient { get; set; }

        [Required(ErrorMessage = "Please enter a subject")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Please enter a message")]
        public string? Content { get; set; }
    }
}