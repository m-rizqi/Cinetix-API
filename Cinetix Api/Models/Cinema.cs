namespace Cinetix_Api.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
