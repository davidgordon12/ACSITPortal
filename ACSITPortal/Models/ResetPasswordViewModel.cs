using System.ComponentModel.DataAnnotations;

namespace ACSITPortal.Models
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Please enter a password less than 100 characters")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Please enter a password less than 100 characters")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set;}
    }
}
