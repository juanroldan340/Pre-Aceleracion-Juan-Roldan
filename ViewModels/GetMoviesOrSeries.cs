using System;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels
{
    public class GetMoviesOrSeries
    {
        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime? CreationDate { get; set; }
    }
}
