using Microsoft.VisualBasic;

namespace Cinetix_Api.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public DateTime DateAndTime { get; set; }
        public int BookedUserId { get; set; }
    }
}
