namespace Cinetix_Api.Models
{
    public class Movie
    {
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
