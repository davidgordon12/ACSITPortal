using System.ComponentModel.DataAnnotations;

namespace ACSITPortal.Models
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(16, ErrorMessage = "Please enter a name less than 16 characters")]
        public string? UserLogin { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(16, ErrorMessage = "Please a 10-digit phone number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(55, ErrorMessage = "Please enter a name less than 55 characters")]
        public string? UserPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(55, ErrorMessage = "Please enter a password less than 55 characters")]
        [Compare("UserPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
