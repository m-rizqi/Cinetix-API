namespace Cinetix_Api.Models
{
    public class MovieWithGenres
    {
        public MovieWithGenres(int movieId, int genreId)
        {
            MovieId = movieId;
            GenreId = genreId;
        }
        public MovieWithGenres()
        {
        }

        public int Id { get; set; }
        public int MovieId { get; set; }
        public int GenreId { get; set; }
    }
}
