using Microsoft.EntityFrameworkCore;

namespace Cinetix_Api.Models
{
    public class MovieWithCinemas
    {
        public MovieWithCinemas(int movieId, int cinemaId)
        {
            MovieId = movieId;
            CinemaId = cinemaId;
        }

        public MovieWithCinemas()
        {
        }

        public int Id { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
    }
}
