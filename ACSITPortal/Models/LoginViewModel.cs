using System.ComponentModel.DataAnnotations;

namespace ACSITPortal.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(16, ErrorMessage = "Please enter a name less than 16 characters")]
        public string? UserLogin { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Please enter a name less than 100 characters")]
        public string? UserPassword { get; set; }
    }
}
