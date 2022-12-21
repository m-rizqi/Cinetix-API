namespace Cinetix_Api.Models
{
    public class Cinema
    {
        public Cinema() { }

        public Cinema(int id, string name, List<Seat> seats)
        {
            Id = id;
            Name = name;
            Seats = seats;
        }

        public Cinema(string name, List<Seat> seats)
        {
            Name = name;
            Seats = seats;
        }

        public Cinema(string name)
        {
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
