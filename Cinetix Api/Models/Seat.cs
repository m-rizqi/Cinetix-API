using Microsoft.VisualBasic;

namespace Cinetix_Api.Models
{
    public class Seat
    {
        public Seat() { }

        public Seat(int id, string seatNumber, bool isBooked, DateTime dateAndTime, int bookedUserId, int cinemaId)
        {
            Id = id;
            SeatNumber = seatNumber;
            IsBooked = isBooked;
            DateAndTime = dateAndTime;
            BookedUserId = bookedUserId;
            CinemaId = cinemaId;
        }

        public Seat(string seatNumber, bool isBooked, DateTime dateAndTime, int bookedUserId, int cinemaId)
        {
            SeatNumber = seatNumber;
            IsBooked = isBooked;
            DateAndTime = dateAndTime;
            BookedUserId = bookedUserId;
            CinemaId = cinemaId;
        }

        public Seat(string seatNumber, bool isBooked, DateTime dateAndTime)
        {
            SeatNumber = seatNumber;
            IsBooked = isBooked;
            DateAndTime = dateAndTime;
        }

        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public DateTime DateAndTime { get; set; }
        public int BookedUserId { get; set; }
        public int CinemaId { get; set; }
    }
}
