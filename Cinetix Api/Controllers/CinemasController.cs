using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinetix_Api.Context;
using Cinetix_Api.Models;

namespace Cinetix_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        private readonly CinemaContext _cinemaContext;
        private readonly SeatContext _seatContext;

        public CinemasController(CinemaContext cinemaContext, SeatContext seatContext)
        {
            _cinemaContext = cinemaContext;
            _seatContext = seatContext;
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
        public async Task<IActionResult> PutCinema(int id, Cinema cinema)
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
                    _seatContext.Entry(seat).State = EntityState.Modified;
                    await _seatContext.SaveChangesAsync();
                }
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

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Cinema>> PostCinema(Cinema cinema)
        {
            _cinemaContext.Cinemas.Add(cinema);
            await _cinemaContext.SaveChangesAsync();
            foreach(var seat in cinema.Seats)
            {
                seat.CinemaId = cinema.Id;
                _seatContext.Seats.Add(seat);
                await _seatContext.SaveChangesAsync();
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

        private bool CinemaExists(int id)
        {
            return _cinemaContext.Cinemas.Any(e => e.Id == id);
        }
    }
}
