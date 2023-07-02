using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class ChangePasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int Otp { get; set; }
        [PasswordPolicy]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
