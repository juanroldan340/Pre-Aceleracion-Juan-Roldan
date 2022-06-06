using DisneyAPI.Models;

namespace DisneyWebAPI.Models
{
    public class CharacterMovieOrSerie
    {
        public int? CharactersId { get; set; }
        public int? MoviesOrSeriesId { get; set; }

        public Character Character { get; set; }
        public MovieOrSerie MovieOrSerie { get; set; }
    }
}
