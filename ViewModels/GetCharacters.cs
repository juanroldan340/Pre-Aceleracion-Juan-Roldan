using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels
{
    public class GetCharacters
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        
        [Display(Name = "Imagen")]
        public string Image { get; set; }
    }
}
