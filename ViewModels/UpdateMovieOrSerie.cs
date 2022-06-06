using DisneyAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels
{
    public class UpdateMovieOrSerie
    {
        [Required]
        public int MovieOrSerieId { get; set; }
        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Title { get; set; }

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; } = null;

        [Display(Name = "Género")]
        public int? GenreId { get; set; } = null;

        [Range(0, 5)]
        [Display(Name = "Calificación")]
        public int Qualification { get; set; }

        [Required(ErrorMessage = "El Id de personaje es obligatorio.")]
        public int CharacterId { get; set; }
    }
}
