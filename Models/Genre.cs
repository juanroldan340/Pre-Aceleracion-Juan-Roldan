using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class Genre : GenericModel
    {
        [Key]
        public int? GenreId { get; set; }
        public List<MovieOrSerie>? MovieOrSerie { get; set; } = null!;
    }
}
