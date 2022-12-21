using Cinetix_Api.Context;
using Cinetix_Api.Controllers;
using Cinetix_Api.Models;
using Cinetix_Api.Request;
using Cinetix_Api.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Cinetix_Api.Test.Controllers
{
    [TestClass]
    public class MoviesControllerTest
    {
        private DbContextOptions<MovieContext> movieOptions;
        private DbContextOptions<MovieWithCinemasContext> movieWithCinemasOptions;
        private DbContextOptions<MovieWithGenresContext> movieWithGenresOptions;
        private DbContextOptions<MovieWithReviewsContext> movieWithReviewsOptions;
        private DbContextOptions<CinemaContext> cinemaOptions;
        private DbContextOptions<GenreContext> genreOptions;
        private DbContextOptions<ReviewContext> reviewOptions;
        private DbContextOptions<SeatContext> seatOptions;
        private MovieContext movieContext;
        private MovieWithCinemasContext movieWithCinemasContext;
        private MovieWithGenresContext movieWithGenresContext;
        private MovieWithReviewsContext movieWithReviewsContext;
        private CinemaContext cinemaContext;
        private GenreContext genreContext;
        private ReviewContext reviewContext;
        private SeatContext seatContext;
        private MoviesController moviesController;
        private Movie movie1;
        private Movie movie2;
        private Genre genre1;
        private Genre genre2;
        private Seat seat1;
        private Seat seat2;
        private Seat seat3;
        private Seat seat4;
        private Cinema cinema1;
        private Cinema cinema2;

        public MoviesControllerTest()
        {
            movieOptions = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            movieWithCinemasOptions = new DbContextOptionsBuilder<MovieWithCinemasContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            movieWithGenresOptions = new DbContextOptionsBuilder<MovieWithGenresContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            movieWithReviewsOptions = new DbContextOptionsBuilder<MovieWithReviewsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            cinemaOptions = new DbContextOptionsBuilder<CinemaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            genreOptions = new DbContextOptionsBuilder<GenreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            reviewOptions = new DbContextOptionsBuilder<ReviewContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            seatOptions = new DbContextOptionsBuilder<SeatContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            movieContext = new MovieContext(movieOptions);
            movieWithCinemasContext = new MovieWithCinemasContext(movieWithCinemasOptions);
            movieWithGenresContext = new MovieWithGenresContext(movieWithGenresOptions);
            movieWithReviewsContext = new MovieWithReviewsContext(movieWithReviewsOptions);
            cinemaContext = new CinemaContext(cinemaOptions);
            genreContext = new GenreContext(genreOptions);
            reviewContext = new ReviewContext(reviewOptions);
            seatContext = new SeatContext(seatOptions);

            movieContext.Database.EnsureCreated();
            movieWithCinemasContext.Database.EnsureCreated();
            movieWithGenresContext.Database.EnsureCreated();
            movieWithReviewsContext.Database.EnsureCreated();
            cinemaContext.Database.EnsureCreated();
            genreContext.Database.EnsureCreated();
            reviewContext.Database.EnsureCreated();

            moviesController = new MoviesController(
                movieContext, 
                genreContext, 
                reviewContext, 
                cinemaContext, 
                seatContext,
                movieWithCinemasContext,
                movieWithGenresContext,
                movieWithReviewsContext
                );

            movie1 = new Movie("Movie 1", "Overview 1", "image1.com", DateTime.Now, 9);
            movie2 = new Movie("Movie 2", "Overview 2", "image1.com", DateTime.Now, 9);
            seat1 = new Seat("Seat-1", false, DateTime.Now);
            seat2 = new Seat("Seat-2", false, DateTime.Now);
            seat3 = new Seat("Seat-3", false, DateTime.Now);
            seat4 = new Seat("Seat-4", false, DateTime.Now);
            genre1 = new Genre("Action");
            genre2 = new Genre("Romance");
            cinema1 = new Cinema("Cinema1");
            cinema2 = new Cinema("Cinema2");
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            seat3.CinemaId = 2;
            seat4.CinemaId = 2;
            cinema1.Seats = new List<Seat>
            {
                seat1, seat2
            };
            cinema2.Seats = new List<Seat>
            {
                seat3, seat4
            };
        }

        [TestMethod]
        public void GetMovies_Should_Return_Movies()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.Movies.Add(movie2);
            movieContext.SaveChanges();
            // Act
            var response = moviesController.GetMovies().Result.Value;
            // Assert
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual(true, response.Contains(movie1));
            Assert.AreEqual(true, response.Contains(movie2));
        }

        [TestMethod]
        public void GetMovie_Should_Return_Movie()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.SaveChanges();
            // Act
            var response = moviesController.GetMovie(1).Result.Value;
            // Assert
            Assert.AreEqual(movie1, response);
        }

        [TestMethod]
        public void GetMovie_Should_Return_NotFound()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.SaveChanges();
            // Act
            var response = moviesController.GetMovie(2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutMovie_Should_Return_Movie()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.SaveChanges();
            movie1.Title = "Movie 101";
            movie1.Overview = "Overview 101";
            movie1.Rating = 8;
            // Act
            var putResponse = moviesController.PutMovie(1, movie1);
            var getResponse = moviesController.GetMovie(1);
            // Assert
            Assert.AreEqual(movie1, putResponse.Result.Value);
            Assert.AreEqual(movie1, getResponse.Result.Value);
        }

        [TestMethod]
        public void PutMovie_Should_Return_BadRequest()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.SaveChanges();
            movie1.Title = "Movie 101";
            movie1.Overview = "Overview 101";
            movie1.Rating = 8;
            // Act
            var putResponse = moviesController.PutMovie(2, movie1);
            // Assert
            BadRequestResult actual = putResponse.Result.Result as BadRequestResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual(null, putResponse.Result.Value);
        }

        [TestMethod]
        public void PutMovie_Should_Return_NotFound()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.SaveChanges();
            movie1.Title = "Movie 101";
            movie1.Overview = "Overview 101";
            movie1.Rating = 8;
            movie2.Id = 2;
            // Act
            var response = moviesController.PutMovie(2, movie2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PostMovie_Should_Return_Movie()
        {
            // Arrange
            // Act
            var postResponse = moviesController.PostMovie(movie1);
            var getResponse = moviesController.GetMovie(1);
            // Assert
            CreatedAtActionResult actual = postResponse.Result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.Created, actual.StatusCode);
            Assert.AreEqual(movie1, actual.Value);
            Assert.AreEqual(movie1, getResponse.Result.Value);
        }

        [TestMethod]
        public void DeleteMovie_Should_Return_NoContent()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.SaveChanges();
            // Act
            var deleteResponse = moviesController.DeleteMovie(1);
            var getReponse = moviesController.GetMovie(1);
            // Assert
            NoContentResult actual = deleteResponse.Result as NoContentResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actual.StatusCode);

            NotFoundResult actualMovie1 = getReponse.Result.Result as NotFoundResult;
            Assert.IsNotNull(actualMovie1);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actualMovie1.StatusCode);
            Assert.AreEqual(null, getReponse.Result.Value);
        }

        [TestMethod]
        public void DeleteMovie_Should_Return_NotFound()
        {
            // Arrange
            movieContext.Movies.Add(movie1);
            movieContext.SaveChanges();
            // Act
            var deleteResponse = moviesController.DeleteMovie(2);
            var getResponse = moviesController.GetMovie(1);
            // Assert
            NotFoundResult actual = deleteResponse.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);

            Assert.AreEqual(movie1, getResponse.Result.Value);
        }
    }
}