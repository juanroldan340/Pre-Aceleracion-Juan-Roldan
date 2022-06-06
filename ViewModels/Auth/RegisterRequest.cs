using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.Auth
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Email { get; set; }
    }
}
