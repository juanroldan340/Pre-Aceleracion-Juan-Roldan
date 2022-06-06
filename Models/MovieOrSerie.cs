using DisneyWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class MovieOrSerie
    {
        [Key]
        public int? MovieOrSerieId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; } = null;
        public int? GenreId { get; set; } = null;

        [Range(0, 5)]
        public int Qualification { get; set; }

        public List<CharacterMovieOrSerie>? Character { get; set; } = null!;

    }
}
