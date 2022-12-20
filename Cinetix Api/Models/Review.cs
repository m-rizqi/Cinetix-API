namespace Cinetix_Api.Models
{
    public class Review
    {
        public Review() { }

        public Review(int id, string author, string resume)
        {
            Id = id;
            Author = author;
            Resume = resume;
        }

        public Review(string author, string resume)
        {
            Author = author;
            Resume = resume;
        }

        public int Id { get; set; }
        public string Author { get; set; }
        public string Resume { get; set; }
    }
}
