using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Ingrese Usuario válido.")]
        [Display(Name = "Usuario")]
        [MinLength(6)]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Ingrese una contraseña válida.")]
        [Display(Name = "Contraseña")]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
