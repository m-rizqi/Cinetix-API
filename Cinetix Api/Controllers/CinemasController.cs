using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinetix_Api.Context;
using Cinetix_Api.Models;

namespace Cinetix_Api.Controllers
{
    [Route("api/cinemas")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        private readonly CinemaContext _cinemaContext;
        private readonly SeatContext _seatContext;
        private readonly SeatsController seatsController;

        public CinemasController(CinemaContext cinemaContext, SeatContext seatContext)
        {
            _cinemaContext = cinemaContext;
            _seatContext = seatContext;
            seatsController = new SeatsController(seatContext);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cinema>>> GetCinemas()
        {
            var cinemas = await _cinemaContext.Cinemas.ToListAsync();
            foreach(var cinema in cinemas)
            {
                var seats = _seatContext.Seats.ToList().Where(seat => seat.CinemaId.Equals(cinema.Id));
                cinema.Seats = seats.ToList();
            }
            return cinemas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cinema>> GetCinema(int id)
        {
            var cinema = await _cinemaContext.Cinemas.FindAsync(id);

            if (cinema == null)
            {
                return NotFound();
            }
            var seats = _seatContext.Seats.ToList().Where(seat => seat.CinemaId.Equals(cinema.Id));
            cinema.Seats = seats.ToList();

            return cinema;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Cinema>> PutCinema(int id, Cinema cinema)
        {
            if (id != cinema.Id)
            {
                return BadRequest();
            }

            _cinemaContext.Entry(cinema).State = EntityState.Modified;

            try
            {
                await _cinemaContext.SaveChangesAsync();
                foreach (var seat in cinema.Seats)
                {
                    if (seatsController.SeatExists(seat.Id))
                    {
                        await seatsController.PutSeat(seat.Id, seat);
                    }
                    else
                    {
                        seat.CinemaId = cinema.Id;
                        await seatsController.PostSeat(seat);
                    }
                }
                return cinema;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cinema>> PostCinema(Cinema cinema)
        {
            _cinemaContext.Cinemas.Add(cinema);
            await _cinemaContext.SaveChangesAsync();
            foreach(var seat in cinema.Seats)
            {
                seat.CinemaId = cinema.Id;
                await seatsController.PostSeat(seat);
            }
            
            return CreatedAtAction("GetCinema", new { id = cinema.Id }, cinema);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(int id)
        {
            var cinema = await _cinemaContext.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return NotFound();
            }

            _cinemaContext.Cinemas.Remove(cinema);
            await _cinemaContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("isExist/{id}")]
        public bool CinemaExists(int id)
        {
            return _cinemaContext.Cinemas.Any(e => e.Id == id);
        }
    }
}
