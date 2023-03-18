using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace ACSITPortal.Models
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(16, ErrorMessage = "Please enter a name less than 16 characters")]
        public string? UserLogin { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Your email must be less than 100 characters")]
        [RegularExpression("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])", ErrorMessage = "Please enter a valid Email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Please enter a password less than 100 characters")]
        public string? UserPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(55, ErrorMessage = "Please enter a password less than 55 characters")]
        [Compare("UserPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
