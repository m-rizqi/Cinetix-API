namespace Cinetix_Api.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public string Fullname { get; set; }
        public Cinema Cinema { get; set; }
        public Seat Seat { get; set; }
        public long DateAndTime { get; set; }
        public int UserId { get; set; }
    }
}
