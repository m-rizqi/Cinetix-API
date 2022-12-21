namespace Cinetix_Api.Models
{
    public class Movie
    {
        public Movie() { }

        public Movie(string title, string overview, string imageUrl, DateTime releaseDate, double rating, List<Genre> genres, List<Review> reviews, List<Cinema> cinemas)
        {
            Title = title;
            Overview = overview;
            ImageUrl = imageUrl;
            ReleaseDate = releaseDate;
            Rating = rating;
            Genres = genres;
            Reviews = reviews;
            Cinemas = cinemas;
        }

        public Movie(string title, string overview, string imageUrl, DateTime releaseDate, double rating)
        {
            Title = title;
            Overview = overview;
            ImageUrl = imageUrl;
            ReleaseDate = releaseDate;
            Rating = rating;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double Rating { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Cinema> Cinemas { get; set; }
    }
}
