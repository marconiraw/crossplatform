using System.ComponentModel.DataAnnotations;

namespace WebApplicationLab5.Models
{
    public class UserModel
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(500)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}",
            ErrorMessage = "Password must be 8-16 characters with at least 1 digit, 1 uppercase letter, 1 special character.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}