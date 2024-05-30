using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models
{
    public class AuthenticationRequest
    {
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [RegularExpression(@"^(?=.*\d).+$", ErrorMessage = "Password must contain at least one digit.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
