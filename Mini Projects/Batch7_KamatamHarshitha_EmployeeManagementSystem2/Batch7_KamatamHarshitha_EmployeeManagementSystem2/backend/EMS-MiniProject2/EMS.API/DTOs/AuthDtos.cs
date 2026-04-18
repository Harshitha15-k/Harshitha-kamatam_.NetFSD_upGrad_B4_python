using System.ComponentModel.DataAnnotations;

namespace EMS.API.DTOs
{
    public class AuthRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public string Role { get; set; } = "Viewer"; // Defaults to Viewer if not provided
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}