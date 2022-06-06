using DisneyAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels
{
    public class UpdateCharacter
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public int CharacterId { get; set; }
        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Edad")]
        public int Age { get; set; }

        [Display(Name = "Peso")]
        public double Weight { get; set; }

        [Display(Name = "Historia")]
        public string History { get; set; }

        [Required(ErrorMessage = "El Id de Película o Serie es obligatorio.")]
        public int MovieOrSerieId { get; set; }
    }
}
