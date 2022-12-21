using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinetix_Api.Context;
using Cinetix_Api.Models;

namespace Cinetix_Api.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly MovieWithCinemasContext _movieWithCinemasContext;
        private readonly MovieWithGenresContext _movieWithGenresContext;
        private readonly MovieWithReviewsContext _movieWithReviewsContext;
        private readonly GenresController genresController;
        private readonly ReviewsController reviewsController;
        private readonly CinemasController cinemasController;

        public MoviesController(MovieContext context,
                                GenreContext genreContext,
                                ReviewContext reviewContext,
                                CinemaContext cinemaContext,
                                SeatContext seatContext,
                                MovieWithCinemasContext movieWithCinemasContext,
                                MovieWithGenresContext movieWithGenresContext,
                                MovieWithReviewsContext movieWithReviewsContext
                                )
        {
            _context = context;
            _movieWithCinemasContext = movieWithCinemasContext;
            _movieWithGenresContext = movieWithGenresContext;
            _movieWithReviewsContext = movieWithReviewsContext;
            genresController = new GenresController(genreContext);
            reviewsController = new ReviewsController(reviewContext);
            cinemasController = new CinemasController(cinemaContext, seatContext);
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var cinemas = await _context.Movies.ToListAsync();
            foreach(var cinema in cinemas)
            {
                cinema.Genres = await GetGenres(cinema.Id);
                cinema.Cinemas = await GetCinemas(cinema.Id);
                cinema.Reviews = await GetReviews(cinema.Id);
            }
            return cinemas;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return await GetMovieWithCinemasGenresReviews(movie);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                movie = await MoviePutOrPostCinemasGenresReviews(movie);
                return movie;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            movie = await MoviePutOrPostCinemasGenresReviews(movie);
            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        private async Task<List<Cinema>> GetCinemas(int movieId)
        {
            var cinemas = new List<Cinema>();
            var movieWithCinemas = _movieWithCinemasContext.MovieWithCinemas.ToList().Where(e => e.MovieId.Equals(movieId));
            foreach (var movieWithCinema in movieWithCinemas)
            {
                var cinema = (await cinemasController.GetCinema(movieWithCinema.CinemaId)).Value;
                cinemas.Add(cinema);
            }
            return cinemas;
        }

        private async Task<List<Genre>> GetGenres(int movieId)
        {
            var genres = new List<Genre>();
            var movieWithGenres = _movieWithGenresContext.MovieWithGenres.ToList().Where(e => e.MovieId.Equals(movieId));
            foreach(var movieWithGenre in movieWithGenres)
            {
                var genre = (await genresController.GetGenre(movieWithGenre.GenreId)).Value;
                genres.Add(genre);
            }
            return genres;
        }

        private async Task<List<Review>> GetReviews(int movieId)
        {
            var reviews = new List<Review>();
            var movieWithReviews = _movieWithReviewsContext.MovieWithReviews.ToList().Where(e => e.MovieId.Equals(movieId));
            foreach (var movieWithReview in movieWithReviews)
            {
                var review = (await reviewsController.GetReview(movieWithReview.ReviewId)).Value;
                reviews.Add(review);
            }
            return reviews;
        }

        private async Task<Movie> GetMovieWithCinemasGenresReviews(Movie movie)
        {
            movie.Genres = await GetGenres(movie.Id);
            movie.Cinemas = await GetCinemas(movie.Id);
            movie.Reviews = await GetReviews(movie.Id);
            return movie;
        }

        private async Task<Movie> MoviePutOrPostCinemasGenresReviews(Movie movie)
        {
            foreach (var cinema in movie.Cinemas)
            {
                if (cinemasController.CinemaExists(cinema.Id))
                {
                    await cinemasController.PutCinema(cinema.Id, cinema);
                }
                else
                {
                    var postResponse = await cinemasController.PostCinema(cinema);
                    var postCinema = (postResponse.Result as CreatedAtActionResult).Value as Cinema;
                    _movieWithCinemasContext.Add(new MovieWithCinemas(movie.Id, postCinema.Id));
                    _movieWithCinemasContext.SaveChanges();
                }
            }
            foreach (var genre in movie.Genres)
            {
                if (genresController.GenreExists(genre.Id))
                {
                    await genresController.PutGenre(genre.Id, genre);
                }
                else
                {
                    var postResponse = await genresController.PostGenre(genre);
                    var postGenre = (postResponse.Result as CreatedAtActionResult).Value as Genre;
                    _movieWithGenresContext.Add(new MovieWithGenres(movie.Id, postGenre.Id));
                    _movieWithGenresContext.SaveChanges();
                }

            }
            foreach (var review in movie.Reviews)
            {
                if (reviewsController.ReviewExists(review.Id))
                {
                    await reviewsController.PutReview(review.Id, review);
                }
                else
                {
                    var postResponse = await reviewsController.PostReview(review);
                    var postReview = (postResponse.Result as CreatedAtActionResult).Value as Review;
                    _movieWithReviewsContext.Add(new MovieWithReviews(movie.Id, postReview.Id));
                    _movieWithReviewsContext.SaveChanges();
                }
            }
            return movie;
        }
    }
}
