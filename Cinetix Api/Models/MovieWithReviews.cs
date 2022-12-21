namespace Cinetix_Api.Models
{
    public class MovieWithReviews
    {
        public MovieWithReviews(int movieId, int reviewId)
        {
            MovieId = movieId;
            ReviewId = reviewId;
        }

        public MovieWithReviews()
        {
        }

        public int Id { get; set; }
        public int MovieId { get; set; }
        public int ReviewId { get; set; }
    }
}
